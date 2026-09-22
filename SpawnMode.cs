using PokeItems.Items;
using PokeItems.Managers;
using UnityEngine;
using RoR2;

namespace PokeItems
{
    internal class SpawnMode
    {
        public static int pageNo = 0;
        private static int maxPages = 2;

        public static void Update()
        {
            // Ignore everything in update if Spawn Mode is disabled.
            if (!ConfigManager.SpawnModeEnabled.Value)
                return;

            // Change the page
            if (Input.GetKeyDown(KeyCode.F1))
                changePage();
            
            // These if statement checks if the player has currently pressed the desired key.
            if (pageNo == 0)
            {
                // Air Balloon
                if (Input.GetKeyDown(KeyCode.F2))
                    spawnItem(AirBalloon.itemDef.itemIndex, false);

                // Flame Orb
                if (Input.GetKeyDown(KeyCode.F3))
                    spawnItem(FlameOrb.itemDef.itemIndex, false);

                // Leftovers
                if (Input.GetKeyDown(KeyCode.F4))
                    spawnItem(Leftovers.itemDef.itemIndex, false);

                // EXP Share
                if (Input.GetKeyDown(KeyCode.F5))
                    spawnItem(ExpShare.itemDef.itemIndex, false);

                // Amulet Coin
                if (Input.GetKeyDown(KeyCode.F6))
                    spawnItem(AmuletCoin.itemDef.itemIndex, false);

                // Heavy Duty Boots
                if (Input.GetKeyDown(KeyCode.F7))
                    spawnItem(HeavyDutyBoots.itemDef.itemIndex, false);

                // Choice Band
                if (Input.GetKeyDown(KeyCode.F8))
                    spawnItem(ChoiceBand.itemDef.itemIndex, false);

                // Choice Specs
                if (Input.GetKeyDown(KeyCode.F9))
                    spawnItem(ChoiceSpecs.itemDef.itemIndex, false);

                // Choice Scarf
                if (Input.GetKeyDown(KeyCode.F10))
                    spawnItem(ChoiceScarf.itemDef.itemIndex, false);
            }

            else if (pageNo == 1)
            {

            }
        }

        // Spawn designated item into the game
        private static void spawnItem(ItemIndex itemDex, bool isUnfinished)
        {
            // If item is unfinished, also consider setting if those items are enabled
            if (!isUnfinished || PokeItems.isUnfinishedEnabled)
            {
                // Get the player body to use a position:
                var transform = PlayerCharacterMasterController.instances[0].master.GetBodyObject().transform;

                // And then drop our defined item in front of the player.

                Log.Info($"Spawning custom item at coordinates {transform.position}");
                PickupDropletController.CreatePickupDroplet(PickupCatalog.FindPickupIndex(itemDex), transform.position, transform.forward * 30f);
            }
        }

        // Change the page (either up a page or back to start)
        private static void changePage()
        {
            if (++pageNo <= maxPages)
                pageNo -= maxPages;

            Log.Info($"Changed to page {pageNo}");
        }
    }
}
