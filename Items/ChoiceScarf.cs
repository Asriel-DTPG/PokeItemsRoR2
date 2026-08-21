using PokeItems.Managers;
using R2API;
using RoR2;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace PokeItems.Items
{
    internal class ChoiceScarf
    {
        public static ItemDef itemDef;

        // Item Settings
        private static ItemTier tier = ItemTier.Lunar; // 1 = WHITE; 2 = GREEN; 3 = RED
        public static float movementBonus = 30f; // Initial movement bonus
        public static float movementBonusPerExtraStack = 20f; // Extra movement bonus per stack

        public static void Init()
        {
            // Create the itemDef via ItemManager
            itemDef = ItemManager.CreateItemDef("ChoiceScarf", tier, true, false,
                [ItemTag.Utility, ItemTag.CanBeTemporary],
                movementBonus, movementBonusPerExtraStack, ChoiceManager.cooldownPenaltyPerStack);

            // Add the functionality
            RecalculateStatsAPI.GetStatCoefficients += ChoiceScarfMovementHook;
        }

        // Check skill if it's Primary and Secondary, and if so, multiply the damage
        private static void ChoiceScarfMovementHook(CharacterBody body, RecalculateStatsAPI.StatHookEventArgs args)
        {
            // Mandatory check
            if (body == null || !body.inventory)
                return;

            // Get the count of how many of this item is in the inventory
            int itemCount = body.inventory.GetItemCountEffective(itemDef);

            // Ignore this if this item is not in inventory
            if (itemCount <= 0)
                return;

            // Check if custom values are enabled
            float moveBonus = ConfigManager.GetFloatValue(
                ConfigManager.ChoiceScarf_MovementBonus,
                movementBonus);
            float moveExtraBonus = ConfigManager.GetFloatValue(
                ConfigManager.ChoiceScarf_MovementBonusPerExtraStack,
                movementBonusPerExtraStack);

            // Calculate bonus percent of movement
            float bonusPercent = MathUtility.GetLinearWithExtraStacking(movementBonus, moveBonus, moveExtraBonus);

            // Apply movement bonus
            args.moveSpeedMultAdd += bonusPercent / 100f;
        }
    }
}
