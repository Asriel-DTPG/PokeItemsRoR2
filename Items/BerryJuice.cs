using PokeItems.Managers;
using R2API;
using RoR2;
using System.Collections.Generic;
using UnityEngine;

namespace PokeItems.Items
{
    internal class BerryJuice
    {
        public static ItemDef itemDef;
        public static BuffDef berryLevel;

        // Item Settings
        private static ItemTier tier = ItemTier.Tier3; // 1 = WHITE; 2 = GREEN; 3 = RED
        public static float regenPercent = 2f; // Initial regen percent
        public static float regenPercentPerExtraStack = 0.5f; // Regen percent per extra stack
        public static int itemCap = 6; // Initial max capacity
        public static int itemCapPerExtraStack = 4; // Max capacity per extra stack

        public static void Init()
        {
            // Create the itemDef via ItemManager
            itemDef = ItemManager.CreateItemDef("BerryJuice", tier, true, false,
                [ItemTag.Healing, ItemTag.CanBeTemporary],
                regenPercent, regenPercentPerExtraStack, itemCap, itemCapPerExtraStack);

            // Create the buffDef via BuffManager
            berryLevel = BuffManager.CreateBuffDef("BerryLevel", Color.white, true, false, false);

            // Initialize the functionality
            RecalculateStatsAPI.GetStatCoefficients += BerryJuiceRegenHook;
        }

        // Main function
        private static void BerryJuiceRegenHook(
            CharacterBody body,
            RecalculateStatsAPI.StatHookEventArgs args)
        {
            // Mandatory checks
            if (body == null || body.inventory == null)
                return;

            int itemCount = body.inventory.GetItemCountEffective(itemDef);

            if (itemCount <= 0)
                return;

            // Get the count of unused/consumed (NoTier) items in inventory
            int unusableItemCount = GetNoTierItemCount(body);

            if (unusableItemCount <= 0)
                return;

            // Get max count of unused/consumed (NoTier) items
            int totalCap = (int)MathUtility.GetLinearWithExtraStacking(itemCap, itemCapPerExtraStack, itemCount);

            if (totalCap <= 0)
                return;

            // Calculate regeneration rate
            float regenRate = MathUtility.GetLinearWithExtraStacking(regenPercent, regenPercentPerExtraStack, itemCount);

            // Calculate regeneration amount based on max HP percentage and max capacity
            float totalRegen = body.maxHealth * (regenRate / 100f) * Mathf.Min(unusableItemCount, totalCap);

            // Add to character
            args.baseRegenAdd += totalRegen;
        }

        // Helper function to get count of NoTier items
        private static int GetNoTierItemCount(CharacterBody body)
        {
            Inventory inventory = body.inventory;

            if (inventory == null)
                return 0;

            int total = 0;

            HashSet<ItemIndex> processedItems = new();

            // Iterate through items in inventory
            foreach (ItemIndex itemIndex in inventory.itemAcquisitionOrder)
            {
                // Add to processed item so that it prevents duplicate stacks
                if (!processedItems.Add(itemIndex))
                    continue;

                ItemDef itemDef = ItemCatalog.GetItemDef(itemIndex);

                if (itemDef == null)
                    continue;

                // Ignore if it's not a NoTier
                if (itemDef.tier != ItemTier.NoTier)
                    continue;

                // Add to total based on stacks of that item
                int count = inventory.GetItemCountEffective(itemIndex);

                if (count <= 0)
                    continue;

                total += count;
            }

            return total;
        }
    }
}
