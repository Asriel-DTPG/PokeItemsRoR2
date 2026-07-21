using R2API;
using RoR2;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace PokeItems.Items
{
    internal class Leftovers
    {
        public static ItemDef itemDef;

        // Item Settings
        private static ItemTier tier = ItemTier.Tier2; // 1 = WHITE; 2 = GREEN; 3 = RED
        public static float regenBonusPerStack = 4f; // HP Regen bonus value
        public static float regenBonusPerExtraStack = 2f; // HP Regen bonus value for extra stacks

        public static void Init()
        {
            // Create the itemDef via ItemManager
            itemDef = ItemManager.CreateItemDef("Leftovers", tier, true, false,
                [ItemTag.Healing, ItemTag.CanBeTemporary],
                regenBonusPerStack, regenBonusPerExtraStack);

            // Add the functionality
            RecalculateStatsAPI.GetStatCoefficients += StatHooks;
        }

        // Apply health regeneration bonus
        private static void StatHooks(CharacterBody body, RecalculateStatsAPI.StatHookEventArgs args)
        {
            // Mandatory check
            if (body == null || !body.inventory)
                return;

            // Get the count of how many of this item is in the inventory
            int itemCount = body.inventory.GetItemCountEffective(itemDef);

            // Ignore this if this item is not in inventory
            if (itemCount <= 0)
                return;

            // Add HP/s per stack
            args.baseRegenAdd += MathUtility.GetLinearWithExtraStacking(regenBonusPerStack, regenBonusPerExtraStack, itemCount);
        }
    }
}
