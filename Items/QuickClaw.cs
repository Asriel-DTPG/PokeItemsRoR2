using PokeItems.Managers;
using RoR2;
using System.Collections.Generic;
using UnityEngine.Networking;

namespace PokeItems.Items
{
    internal class QuickClaw
    {
        public static ItemDef itemDef;
        private static readonly Dictionary<GenericSkill, float> activeRechargeMultipliers = new();
        private static readonly Dictionary<GenericSkill, int> startingStock = new();

        // Item Settings
        private static ItemTier tier = ItemTier.Tier2; // 1 = WHITE; 2 = GREEN; 3 = RED
        public static float procChance = 10f; // Initial proc chance
        public static float procChancePerExtraStack = 5f; // Proc chance per extra stack
        public static float cooldownReductionPercent = 50f; // Cooldown reduction of a used move

        public static void Init()
        {
            // Create the itemDef via ItemManager
            itemDef = ItemManager.CreateItemDef("QuickClaw", tier, true, false,
                [ItemTag.Utility, ItemTag.CanBeTemporary],
                procChance, procChancePerExtraStack, cooldownReductionPercent);

            // Initialize the functionality
            On.RoR2.GenericSkill.ExecuteIfReady += QuickClawSkillHook;
            On.RoR2.GenericSkill.RunRecharge += QuickClawRechargeHook;
            On.RoR2.CharacterBody.OnDestroy += QuickClawBodyDestroy;
        }

        // When skill is successfully used
        private static bool QuickClawSkillHook(
            On.RoR2.GenericSkill.orig_ExecuteIfReady orig,
            GenericSkill self)
        {
            // Execute the original method first (whether the skill executed successfully)
            bool executed = orig(self);
            
            // Network server dependant
            if (!NetworkServer.active)
                return executed;

            // Mandatory checks
            if (self == null)
                return executed;

            CharacterBody body = self.characterBody;

            if (body == null || body.inventory == null)
                return executed;

            // If skill did not execute successfully, ignore
            if (!executed)
                return executed;

            int itemCount = body.inventory.GetItemCountEffective(itemDef);

            if (itemCount <= 0)
                return executed;

            // Ignore skills without a cooldown
            if (self.baseRechargeInterval <= 0f)
                return executed;

            // Prevent multiple Quick Claw effects from stacking on the same cooldown
            if (activeRechargeMultipliers.ContainsKey(self))
                return executed;

            // Roll total proc chance, and ignore if it fails
            float totalChance = MathUtility.GetLinearWithExtraStacking(procChance, procChancePerExtraStack, itemCount);

            if (!Util.CheckRoll(totalChance, body.master))
                return executed;

            // This is in case the cooldown reduction is non-existent
            if (cooldownReductionPercent <= 0f)
                return executed;

            float rechargeMultiplier = 1f / (1f - cooldownReductionPercent / 100f);

            // Safety check against invalid values
            if (float.IsNaN(rechargeMultiplier) ||
                float.IsInfinity(rechargeMultiplier) ||
                rechargeMultiplier <= 1f)
                return executed;

            // Record the state
            activeRechargeMultipliers[self] = rechargeMultiplier;

            // ExecuteIfReady has already consumed the stock, so this is the stock count that exists after the skill was used. This will be used to watch the stock until it increases.
            startingStock[self] = self.stock;

            return executed;
        }

        // Cooldown recharge function
        private static void QuickClawRechargeHook(
            On.RoR2.GenericSkill.orig_RunRecharge orig,
            GenericSkill self,
            float dt)
        {
            if (!NetworkServer.active || self == null)
            {
                orig(self, dt);
                return;
            }

            // Check whether this skill has Quick Claw effect
            if (!activeRechargeMultipliers.TryGetValue(
                self, out float multiplier))
            {
                orig(self, dt);
                return;
            }

            // Check whether one stock has already recharged. If so, complete the effect
            if (startingStock.TryGetValue(
                self, out int initialStock))
            {
                if (self.stock > initialStock)
                {
                    RemoveQuickClawState(self);

                    orig(self, dt);
                    return;
                }
            }

            // Accelerate recharge
            float modifiedDeltaTime = dt * multiplier;

            orig(self, modifiedDeltaTime);
        }

        // Character cleanup
        private static void QuickClawBodyDestroy(
            On.RoR2.CharacterBody.orig_OnDestroy orig,
            CharacterBody self)
        {
            if (self != null)
            {
                List<GenericSkill> skillsToRemove = new();

                foreach (GenericSkill skill in activeRechargeMultipliers.Keys)
                {
                    if (skill == null || skill.characterBody == self)
                        skillsToRemove.Add(skill);
                }

                foreach (GenericSkill skill in skillsToRemove)
                {
                    RemoveQuickClawState(skill);
                }
            }
        }

        // Helper function to clean up state
        private static void RemoveQuickClawState(GenericSkill skill)
        {
            if (skill == null)
                return;

            activeRechargeMultipliers.Remove(skill);
            startingStock.Remove(skill);
        }
    }
}
