using R2API;
using RoR2;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace PokeItems.Items
{
    internal class PoppedAirBalloon
    {
        public static ItemDef itemDef;

        // Item Settings
        private static ItemTier tier = ItemTier.NoTier; // 1 = WHITE; 2 = GREEN; 3 = RED

        public static void Init()
        {
            // Create the itemDef via ItemManager
            itemDef = ItemManager.CreateItemDef("POPPEDAIRBALLOON", tier, false, false,
                []);
        }
    }
}
