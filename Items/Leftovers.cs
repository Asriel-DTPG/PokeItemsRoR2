using PokeItems.Managers;
using R2API;
using RoR2;

namespace PokeItems.Items
{
    internal class Leftovers
    {
        public static ItemDef itemDef;

        // Item Settings
        private static ItemTier tier = ItemTier.Tier2; // 1 = WHITE; 2 = GREEN; 3 = RED
        public static float regenBonus = 2f; // Initial HP Regen bonus value
        public static float regenBonusPercent = 1f; // Initial HP Regen percent (max HP) bonus value
        public static float regenBonusPerExtraStack = 1.5f; // HP Regen percent (max HP) bonus value for extra stacks
        public static float regenBonusPercentPerExtraStack = 0.5f;

        public static void Init()
        {
            // Create the itemDef via ItemManager
            itemDef = ItemManager.CreateItemDef("Leftovers", tier, true, false,
                [ItemTag.Healing, ItemTag.CanBeTemporary],
                regenBonus, regenBonusPercent, regenBonusPerExtraStack, regenBonusPercentPerExtraStack);

            // Add the functionality
            RecalculateStatsAPI.GetStatCoefficients += LeftoversHook;
        }

        // Apply health regeneration bonus
        private static void LeftoversHook(CharacterBody body, RecalculateStatsAPI.StatHookEventArgs args)
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
            float totalRegen = ConfigManager.GetFloatValue(
                ConfigManager.Leftovers_RegenBonus,
                regenBonus);
            float totalRegenPercent = ConfigManager.GetFloatValue(
                ConfigManager.Leftovers_RegenBonusPercent,
                regenBonusPercent) *
                body.maxHealth / 100f;
            float totalExtraRegen = ConfigManager.GetFloatValue(
                ConfigManager.Leftovers_RegenBonusPerExtraStack,
                regenBonusPerExtraStack);
            float totalExtraRegenPercent = ConfigManager.GetFloatValue(
                ConfigManager.Leftovers_RegenBonusPercentPerExtraStack,
                regenBonusPercentPerExtraStack) *
                body.maxHealth / 100f;

            // Add HP/s per stack
            args.baseRegenAdd += MathUtility.GetLinearWithExtraStacking(
                totalRegen + totalRegenPercent,
                totalExtraRegen + totalExtraRegenPercent,
                itemCount);
        }
    }
}
