using BepInEx;
using BepInEx.Configuration;
using PokeItems.Items;
using PokeItems.Managers;
using R2API;
using R2API.Utils;
using RoR2;
using RoR2.ExpansionManagement;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace PokeItems
{
    // Dependencies
    [BepInDependency(ItemAPI.PluginGUID)]
    [BepInDependency(LanguageAPI.PluginGUID)]
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.EveryoneNeedSameModVersion)]

    public class PokeItems : BaseUnityPlugin
    {
        public const string PluginGUID = PluginAuthor + "." + PluginName;
        public const string PluginAuthor = "DTPGStudios";
        public const string PluginName = "PokeItems";
        public const string PluginVersion = "0.2.0";

        public static PluginInfo PInfo { get; private set; }
        public static ConfigFile PConfig { get; private set; }

        public static ExpansionDef sotvDLC;
        public static ExpansionDef sotsDLC;

        // The Awake() method is run at the very start when the game is initialized.
        public void Awake()
        {
            // Get the plugin info
            PInfo = Info;

            // Get the config file
            PConfig = Config;

            // Get DLC variables
            sotvDLC = Addressables.LoadAssetAsync<ExpansionDef>("RoR2/DLC1/Common/DLC1.asset").WaitForCompletion();
            sotsDLC = Addressables.LoadAssetAsync<ExpansionDef>("RoR2/DLC2/Common/DLC2.asset").WaitForCompletion();

            // Init our logging class so that we can properly log for debugging
            Log.Init(Logger);

            // Config
            ConfigManager.Init();
            
            // Assets
            AssetManager.Init();

            // Items
            Leftovers.Init();
            FlameOrb.Init();
            AirBalloonBroken.Init();
            AirBalloon.Init();

            // Unfinished Items
            if (ConfigManager.UnfinishedItemsEnabled.Value)
            {
                ExpShare.Init();
                AmuletCoin.Init();
                HeavyDutyBoots.Init();
            }

            // Log that the mod is ready
            Log.Message("PokeItems mod is ready!");
        }

        // The Update() method is run on every frame of the game.
        private void Update()
        {
            // Ignore everything in update if Spawn Mode is disabled.
            if (!ConfigManager.SpawnModeEnabled.Value)
                return;
            
            // This if statement checks if the player has currently pressed the desired key.

            // Air Balloon
            if (Input.GetKeyDown(KeyCode.F2))
            {
                // Get the player body to use a position:
                var transform = PlayerCharacterMasterController.instances[0].master.GetBodyObject().transform;

                // And then drop our defined item in front of the player.

                Log.Info($"Player pressed key. Spawning custom item at coordinates {transform.position}");
                PickupDropletController.CreatePickupDroplet(PickupCatalog.FindPickupIndex(AirBalloon.itemDef.itemIndex), transform.position, transform.forward * 30f);
            }

            // Flame Orb
            if (Input.GetKeyDown(KeyCode.F3))
            {
                // Get the player body to use a position:
                var transform = PlayerCharacterMasterController.instances[0].master.GetBodyObject().transform;

                // And then drop our defined item in front of the player.

                Log.Info($"Player pressed key. Spawning custom item at coordinates {transform.position}");
                PickupDropletController.CreatePickupDroplet(PickupCatalog.FindPickupIndex(FlameOrb.itemDef.itemIndex), transform.position, transform.forward * 30f);
            }

            // Leftovers
            if (Input.GetKeyDown(KeyCode.F4))
            {
                // Get the player body to use a position:
                var transform = PlayerCharacterMasterController.instances[0].master.GetBodyObject().transform;

                // And then drop our defined item in front of the player.

                Log.Info($"Player pressed key. Spawning custom item at coordinates {transform.position}");
                PickupDropletController.CreatePickupDroplet(PickupCatalog.FindPickupIndex(Leftovers.itemDef.itemIndex), transform.position, transform.forward * 30f);
            }

            // EXP Share
            if (Input.GetKeyDown(KeyCode.F5) && ConfigManager.UnfinishedItemsEnabled.Value)
            {
                // Get the player body to use a position:
                var transform = PlayerCharacterMasterController.instances[0].master.GetBodyObject().transform;

                // And then drop our defined item in front of the player.

                Log.Info($"Player pressed key. Spawning custom item at coordinates {transform.position}");
                PickupDropletController.CreatePickupDroplet(PickupCatalog.FindPickupIndex(ExpShare.itemDef.itemIndex), transform.position, transform.forward * 30f);
            }

            // Amulet Coin
            if (Input.GetKeyDown(KeyCode.F6) && ConfigManager.UnfinishedItemsEnabled.Value)
            {
                // Get the player body to use a position:
                var transform = PlayerCharacterMasterController.instances[0].master.GetBodyObject().transform;

                // And then drop our defined item in front of the player.

                Log.Info($"Player pressed key. Spawning custom item at coordinates {transform.position}");
                PickupDropletController.CreatePickupDroplet(PickupCatalog.FindPickupIndex(AmuletCoin.itemDef.itemIndex), transform.position, transform.forward * 30f);
            }

            // Heavy Duty Boots
            if (Input.GetKeyDown(KeyCode.F7) && ConfigManager.UnfinishedItemsEnabled.Value)
            {
                // Get the player body to use a position:
                var transform = PlayerCharacterMasterController.instances[0].master.GetBodyObject().transform;

                // And then drop our defined item in front of the player.

                Log.Info($"Player pressed key. Spawning custom item at coordinates {transform.position}");
                PickupDropletController.CreatePickupDroplet(PickupCatalog.FindPickupIndex(HeavyDutyBoots.itemDef.itemIndex), transform.position, transform.forward * 30f);
            }
        }
    }
}
