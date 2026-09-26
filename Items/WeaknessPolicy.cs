using PokeItems.Managers;
using R2API;
using RoR2;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace PokeItems.Items
{
    internal class WeaknessPolicy
    {
        public static ItemDef itemDef;

        // Item Settings
        private static ItemTier tier = ItemTier.Tier1; // 1 = WHITE; 2 = GREEN; 3 = RED
        public static int maxBuffStacks = 5; // Initial max stacks of Weakness Policy
        public static int maxBuffStacksPerExtraStack = 3; // Max stacks of Weakness Policy per extra item stack

        public static void Init()
        {
            // Create the itemDef via ItemManager
            itemDef = ItemManager.CreateItemDef("WeaknessPolicy", tier, true, false,
                [ItemTag.Damage, ItemTag.CanBeTemporary],
                WeaknessPolicyManager.damageBonus, maxBuffStacks, maxBuffStacksPerExtraStack, WeaknessPolicyManager.duration);

            // Start the manager
            WeaknessPolicyManager.Init();
        }
    }
}
