using System;
using RoR2;
using UnityEngine;
using System.Collections;
using PokeItems.Items;

namespace PokeItems.Managers
{
    internal static class ChoiceManager
    {
        public static BuffDef choicePrimaryLock;
        public static BuffDef choiceSecondaryLock;
        public static BuffDef choiceUtilityLock;
        public static BuffDef choiceSpecialLock;

        public static BuffDef[] choiceLocks;

        // Item Settings
        public static float cooldownPenaltyPerStack = 20f;
        public static float cooldownPenaltyPercentLimit = 1f;

        public static void Init()
        {
            // Create Choice buffs
            choicePrimaryLock = BuffManager.CreateBuffDef("PrimaryLock", Color.white, false, true, false);
            choiceSecondaryLock = BuffManager.CreateBuffDef("SecondaryLock", Color.white, false, true, false);
            choiceUtilityLock = BuffManager.CreateBuffDef("UtilityLock", Color.white, false, true, false);
            choiceSpecialLock = BuffManager.CreateBuffDef("SpecialLock", Color.white, false, true, false);

            choiceLocks =
            [
                choicePrimaryLock,
                choiceSecondaryLock,
                choiceUtilityLock,
                choiceSpecialLock
            ];

            // Add functionality
            On.RoR2.GenericSkill.OnExecute += ChoiceSkillHook;
            On.RoR2.GenericSkill.RunRecharge += ChoiceCooldownHook;
            On.RoR2.Stage.Start += ChoiceStageStartHook;
            On.RoR2.CharacterMaster.OnBodyStart += ChoiceBodyStartHook;
        }

        // Remove any and all choice locks
        public static void RemoveAllChoiceLocks(CharacterBody body)
        {
            foreach (BuffDef buff in choiceLocks)
            {
                if (body.HasBuff(buff))
                    body.RemoveBuff(buff);
            }
        }

        // Checks if player has any choice lock
        public static bool HasChoiceLock(CharacterBody body)
        {
            return
                body.HasBuff(choicePrimaryLock) ||
                body.HasBuff(choiceSecondaryLock) ||
                body.HasBuff(choiceUtilityLock) ||
                body.HasBuff(choiceSpecialLock);
        }

        // Record the chosen skill into a choice lock if it doesn't exist
        private static void ChoiceSkillHook(
            On.RoR2.GenericSkill.orig_OnExecute orig,
            GenericSkill self)
        {
            // Run vanilla logic
            orig(self);

            CharacterBody body = self.characterBody;

            // Mandatory check
            if (body == null || body.inventory == null)
                return;

            // Get the count of how many of choice items is in the inventory
            if (GetTotalChoiceStacks(body) <= 0)
                return;

            // Proceed if they do not have a choice lock
            if (HasChoiceLock(body))
                return;

            // Add choice lock based on chosen skill
            if (body.skillLocator.primary == self)
                body.AddBuff(choicePrimaryLock);
            else if (body.skillLocator.secondary == self)
                body.AddBuff(choiceSecondaryLock);
            else if (body.skillLocator.utility == self)
                body.AddBuff(choiceUtilityLock);
            else if (body.skillLocator.special == self)
                body.AddBuff(choiceSpecialLock);
        }

        // Modify the recharge time based on non-chosen skill and number of choice items
        private static void ChoiceCooldownHook(
            On.RoR2.GenericSkill.orig_RunRecharge orig,
            GenericSkill self,
            float rechargeTime)
        {
            // Check if this skill has no meaningful recharge
            if (rechargeTime <= 0f)
            {
                orig(self, rechargeTime);
                return;
            }

            CharacterBody body = self.characterBody;

            // Mandatory check
            if (body == null || body.inventory == null)
            {
                orig(self, rechargeTime);
                return;
            }

            // Get total Choice item stacks
            int choiceStacks = GetTotalChoiceStacks(body);

            if (!HasChoiceLock(body) || IsChosenSkill(body, self))
            {
                orig(self, rechargeTime);
                return;
            }

            // Check if custom values are enabled
            float cooldownPenalty = ConfigManager.GetFloatValue(
                ConfigManager.ChoiceItems_CooldownPenaltyPerStack,
                cooldownPenaltyPerStack);

            // Calculate percentage reduction exponentially (Choice locks themselves can still cause effects without choice items)
            float multiplier = MathUtility.GetExponentialPercentReductionStacking(cooldownPenalty, Math.Max(1, choiceStacks));

            // Set limit to multiplier
            multiplier = Mathf.Max(multiplier, cooldownPenaltyPercentLimit / 100f);

            // Set new recharge time
            float newRechargeTime = rechargeTime * multiplier;

            // Continue vanilla logic
            orig(self, newRechargeTime);
        }

        // Reset choice lock on new stage
        private static IEnumerator ChoiceStageStartHook(
            On.RoR2.Stage.orig_Start orig,
            Stage self)
        {
            // Run vanilla logic
            yield return orig(self);

            foreach (CharacterMaster master in CharacterMaster.readOnlyInstancesList)
            {
                CharacterBody body = master.GetBody();

                if (body != null)
                    RemoveAllChoiceLocks(body);
            }
        }

        // Reset choice lock on respawn
        private static void ChoiceBodyStartHook(
            On.RoR2.CharacterMaster.orig_OnBodyStart orig,
            CharacterMaster self,
            CharacterBody body)
        {
            // Run vanilla logic
            orig(self, body);

            if (body != null)
                RemoveAllChoiceLocks(body);
        }

        // Check if this skill has a choice lock associated with it
        private static bool IsChosenSkill(
            CharacterBody body,
            GenericSkill skill)
        {
            if (body.skillLocator.primary == skill)
                return body.HasBuff(choicePrimaryLock);

            if (body.skillLocator.secondary == skill)
                return body.HasBuff(choiceSecondaryLock);

            if (body.skillLocator.utility == skill)
                return body.HasBuff(choiceUtilityLock);

            if (body.skillLocator.special == skill)
                return body.HasBuff(choiceSpecialLock);

            return false;
        }

        // Get the total number of choice item stacks
        public static int GetTotalChoiceStacks(CharacterBody body)
        {
            Inventory inventory = body.inventory;

            if (inventory == null)
                return 0;

            return
                inventory.GetItemCountEffective(ChoiceBand.itemDef) +
                inventory.GetItemCountEffective(ChoiceSpecs.itemDef) +
                inventory.GetItemCountEffective(ChoiceScarf.itemDef);
        }
    }
}
