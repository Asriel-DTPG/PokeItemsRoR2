using PokeItems.Managers;
using R2API;
using RoR2;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace PokeItems.Items
{
    internal class BerryJuice
    {
        public static ItemDef itemDef;
        public static BuffDef berryLevel;

        // Item Settings
        private static ItemTier tier = ItemTier.Tier3; // 1 = WHITE; 2 = GREEN; 3 = RED
        public static float regenPercent = 1.5f; // Initial regen percent
        public static int itemCap = 6; // Initial max capacity
        public static int itemCapPerExtraStack = 4; // Max capacity per extra stack

        public static void Init()
        {
            // Create the itemDef via ItemManager
            itemDef = ItemManager.CreateItemDef("BerryJuice", tier, true, false,
                [ItemTag.Healing, ItemTag.CanBeTemporary],
                regenPercent, itemCap, itemCapPerExtraStack);

            // Create the buffDef via BuffManager
            berryLevel = BuffManager.CreateBuffDef("BerryLevel", Color.white, true, false, false);

            // Initialize the functionality
            RecalculateStatsAPI.GetStatCoefficients += BerryJuiceRegenHook;
            On.RoR2.CharacterBody.OnInventoryChanged += BerryJuiceInventoryChangedHook;
        }

        // Main function for calculating regen based on buff count
        private static void BerryJuiceRegenHook(
            CharacterBody body,
            RecalculateStatsAPI.StatHookEventArgs args)
        {
            // Mandatory checks
            if (body == null || body.inventory == null)
                return;

            // Get current Berry Juice buff stacks
            int stacks = body.GetBuffCount(berryLevel);

            if (stacks <= 0)
                return;

            // Calculate regeneration rate
            float regenRate = (regenPercent * body.maxHealth / 100f) * stacks;

            // Add to regeneration stat
            args.baseRegenAdd += regenRate;
        }

        // Secondary function to set buff count based on number of unusable items
        private static void BerryJuiceInventoryChangedHook(
            On.RoR2.CharacterBody.orig_OnInventoryChanged orig,
            CharacterBody self)
        {
            // Run vanilla logic
            orig(self);

            // Network server dependant
            if (!NetworkServer.active)
                return;

            // Mandatory checks
            if (self == null || self.inventory == null)
                return;

            // Get the count of how many of this item is in the inventory
            int itemCount = self.inventory.GetItemCountEffective(itemDef);

            if (itemCount <= 0)
            {
                // Reset buff count if it still exists
                if (self.GetBuffCount(berryLevel) <= 0)
                    self.SetBuffCount(berryLevel.buffIndex, 0);

                return;
            }

            // Count valid NoTier items
            int buffCount = GetNoTierItemCount(self);

            // Set buff stacks to match item count and remove excess stacks when items are lost
            if (self.GetBuffCount(berryLevel) == buffCount)
                return;

            self.SetBuffCount(berryLevel.buffIndex, buffCount);
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
