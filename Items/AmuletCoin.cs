using PokeItems.Managers;
using R2API;
using RoR2;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;

namespace PokeItems.Items
{
    internal class AmuletCoin
    {
        public static ItemDef itemDef;

        // Item Settings
        private static ItemTier tier = ItemTier.Tier3; // 1 = WHITE; 2 = GREEN; 3 = RED
        public static float goldMulPercentPerStack = 100f; // Gold bonus multiplier

        public static void Init()
        {
            // Create the itemDef via ItemManager
            itemDef = ItemManager.CreateItemDef("ExpShare", tier, true, false,
                [ItemTag.Healing, ItemTag.CanBeTemporary],
                goldMulPercentPerStack);

            // Add the functionality
            On.RoR2.DeathRewards.OnKilledServer += AmuletCoinHook;
        }

        // Apply gold bonus
        private static void AmuletCoinHook(
            On.RoR2.DeathRewards.orig_OnKilledServer orig,
            DeathRewards self,
            DamageReport damageReport)
        {
            CharacterBody attackerBody = damageReport.attackerBody;
            
            // Mandatory check
            if (attackerBody != null && attackerBody.inventory != null)
            {
                // Get the count of how many of this item is in the inventory
                int itemCount = attackerBody.inventory.GetItemCountEffective(itemDef);

                // Ignore this if this item is not in inventory
                if (itemCount > 0)
                {
                    // Set multiplier of the gold gain
                    float multiplier = 1f + (MathUtility.GetLinearStacking(goldMulPercentPerStack, itemCount) / 100f);

                    // Multiply the gold gain
                    self.goldReward = (uint)Mathf.RoundToInt(self.goldReward * multiplier);
                }
            }

            orig(self, damageReport);
        }
    }
}
