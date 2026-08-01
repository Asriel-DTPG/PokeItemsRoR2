using PokeItems.Managers;
using R2API;
using RoR2;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace PokeItems.Items
{
    internal class HeavyDutyBoots
    {
        public static ItemDef itemDef;

        // Item Settings
        private static ItemTier tier = ItemTier.Tier2; // 1 = WHITE; 2 = GREEN; 3 = RED
        public static float armorBonusPerStack = 20f; // Armor bonus value
        public static float armorBonusPerExtraStack = 10f; // Armor bonus value for extra stacks

        private static readonly Dictionary<CharacterBody, bool> isGroundedDict = new();

        public static void Init()
        {
            // Create the itemDef via ItemManager
            itemDef = ItemManager.CreateItemDef("HeavyDutyBoots", tier, true, false,
                [ItemTag.Utility, ItemTag.CanBeTemporary],
                armorBonusPerStack, armorBonusPerExtraStack);

            // Add the functionality
            RecalculateStatsAPI.GetStatCoefficients += HeavyDutyBootsHook;
            On.RoR2.CharacterMotor.FixedUpdate += HeavyDutyBootsGroundedHook;
            On.RoR2.CharacterBody.OnDestroy += DictionaryDestroy;
        }

        // Apply armor regeneration bonus if grounded
        private static void HeavyDutyBootsHook(CharacterBody body, RecalculateStatsAPI.StatHookEventArgs args)
        {
            // Mandatory check
            if (body == null || body.inventory == null)
                return;

            // Get the count of how many of this item is in the inventory
            int itemCount = body.inventory.GetItemCountEffective(itemDef);

            // Ignore this if this item is not in inventory
            if (itemCount <= 0)
                return;

            CharacterMotor motor = body.characterMotor;

            bool grounded = motor == null || motor.isGrounded;

            if (!grounded)
                return;

            // Check if custom values are enabled
            if (ConfigManager.CustomValuesEnabled.Value)
            {
                armorBonusPerStack = ConfigManager.HeavyDutyBoots_ArmorBonusPerStack.Value;
                armorBonusPerExtraStack = ConfigManager.HeavyDutyBoots_ArmorBonusPerExtraStack.Value;
            }

            // Add HP/s per stack
            args.armorAdd += MathUtility.GetLinearWithExtraStacking(armorBonusPerStack, armorBonusPerExtraStack, itemCount);
        }

        // Check for grounded state
        private static void HeavyDutyBootsGroundedHook(
            On.RoR2.CharacterMotor.orig_FixedUpdate orig,
            CharacterMotor self)
        {
            orig(self);

            CharacterBody body = self.body;

            // Mandatory check
            if (body == null || body.inventory == null)
                return;

            // Get the count of how many of this item is in the inventory
            int itemCount = body.inventory.GetItemCountEffective(itemDef);

            // Ignore this if this item is not in inventory
            if (itemCount <= 0)
                return;

            // Grab grounded state
            bool isCurrentlyGrounded = self.isGrounded;

            // If state isn't saved to dictionary yet, do so now and check next time
            if (!isGroundedDict.TryGetValue(body, out bool groundedBefore))
            {
                isGroundedDict[body] = isCurrentlyGrounded;
                return;
            }

            // Check if grounded state changes, and if so, recalculate
            if (groundedBefore != isCurrentlyGrounded)
            {
                isGroundedDict[body] = isCurrentlyGrounded;

                body.MarkAllStatsDirty();
            }
        }

        // Destroy reference from dictionary if it no longer exists
        private static void DictionaryDestroy(
            On.RoR2.CharacterBody.orig_OnDestroy orig,
            CharacterBody self)
        {
            isGroundedDict.Remove(self);
            orig(self);
        }
    }
}
