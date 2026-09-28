using PokeItems.Managers;
using RoR2;

namespace PokeItems.Items
{
    internal class AirBalloonBroken
    {
        public static ItemDef itemDef;

        // Item Settings
        private static ItemTier tier = ItemTier.NoTier; // 1 = WHITE; 2 = GREEN; 3 = RED

        public static void Init()
        {
            // Create the itemDef via ItemManager
            itemDef = ItemManager.CreateItemDef("AirBalloonBroken", tier, false, false,
                []);
        }
    }
}
