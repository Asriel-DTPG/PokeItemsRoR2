using PokeItems.Managers;
using R2API;
using RoR2;
using System.Runtime.CompilerServices;

namespace PokeItems.Items
{
    internal class Leftovers
    {
        public static ItemDef itemDef;

        // Item Settings
        private static ItemTier tier = ItemTier.Tier2; // 1 = WHITE; 2 = GREEN; 3 = RED
        public static float regenBonus = 2f; // Initial HP Regen (%HP) bonus
        public static float regenBonusPerExtraStack = 0.5f; // HP Regen (%HP) bonus per extra stack

        // Tracks the number of skills currently on cooldown for each CharacterBody
        private sealed class CooldownState
        {
            public int DepletedSkillCount;
        }

        private static readonly ConditionalWeakTable<CharacterBody, CooldownState> cooldownStates = new ConditionalWeakTable<CharacterBody, CooldownState>();

        public static void Init()
        {
            // Create the itemDef via ItemManager
            itemDef = ItemManager.CreateItemDef("Leftovers", tier, true, false,
                [ItemTag.Healing, ItemTag.CanBeTemporary],
                regenBonus, regenBonusPerExtraStack);

            // Add the functionality
            On.RoR2.CharacterBody.FixedUpdate += LeftoversFixedUpdateHook;
            RecalculateStatsAPI.GetStatCoefficients += LeftoversRegenHook;
        }

        // Monitor skill cooldowns
        private static void LeftoversFixedUpdateHook(On.RoR2.CharacterBody.orig_FixedUpdate orig, CharacterBody self)
        {
            // Run vanilla logic
            orig(self);

            // Mandatory check
            if (self == null || self.inventory == null || self.skillLocator == null)
                return;

            // Get the count of how many of this item is in the inventory
            int itemCount = self.inventory.GetItemCountEffective(itemDef);

            // Ignore this if this item is not in inventory
            if (itemCount <= 0)
            {
                CooldownState currentState = cooldownStates.GetValue(self, _ => new CooldownState());

                if (currentState.DepletedSkillCount != 0)
                {
                    currentState.DepletedSkillCount = 0;
                    self.MarkAllStatsDirty();
                }

                return;
            }

            // Gather all skills into a list
            GenericSkill[] skills = self.skillLocator.allSkills;

            if (skills == null)
                return;

            int depletedCount = 0;

            // Count skills that are currently on cooldown (if multiple charges, fully depleted)
            foreach (GenericSkill skill in skills)
            {
                if (skill != null && skill.stock <= 0)
                    depletedCount++;
            }

            CooldownState state = cooldownStates.GetValue(self, _ => new CooldownState());

            // Only recalculate stats when number of skills on cooldown changes
            if (state.DepletedSkillCount != depletedCount)
            {
                state.DepletedSkillCount = depletedCount;
                self.MarkAllStatsDirty();
            }
        }

        // Apply health regeneration bonus
        private static void LeftoversRegenHook(CharacterBody body, RecalculateStatsAPI.StatHookEventArgs args)
        {
            // Mandatory check
            if (body == null || body.inventory == null)
                return;

            // Get the count of how many of this item is in the inventory
            int itemCount = body.inventory.GetItemCountEffective(itemDef);

            // Ignore this if this item is not in inventory
            if (itemCount <= 0)
                return;

            // Check if custom values are enabled
            float totalRegen = ConfigManager.GetFloatValue(ConfigManager.Leftovers_RegenBonusPercent, regenBonus) * body.maxHealth / 100f;
            float totalExtraRegen = ConfigManager.GetFloatValue(ConfigManager.Leftovers_RegenBonusPercentPerExtraStack, regenBonusPerExtraStack) * body.maxHealth / 100f;

            // Add HP/s per stack
            args.baseRegenAdd += MathUtility.GetLinearWithExtraStacking(totalRegen, totalExtraRegen, itemCount);
        }
    }
}
