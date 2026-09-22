using BepInEx;
using BepInEx.Configuration;
using PokeItems.Buffs;
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

    // Optional Dependencies
    [BepInDependency("com.rune580.riskofoptions", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency("droppod.lookingglass", BepInDependency.DependencyFlags.SoftDependency)]

    public class PokeItems : BaseUnityPlugin
    {
        public const string PluginGUID = PluginAuthor + "." + PluginName;
        public const string PluginAuthor = "DTPGStudios";
        public const string PluginName = "PokeItems";
        public const string PluginVersion = "0.4.0";

        public static PluginInfo PInfo { get; private set; }
        public static ConfigFile PConfig { get; private set; }

        public static ExpansionDef sotvDLC;
        public static ExpansionDef sotsDLC;

        public static bool isUnfinishedEnabled { get; private set; }

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

            // Risk Of Options (Optional)
            RiskOfOptionsManager.Init();

            // Items
            Leftovers.Init();
            FlameOrb.Init();
            AirBalloonBroken.Init();
            AirBalloon.Init();
            ExpShare.Init();
            AmuletCoin.Init();
            HeavyDutyBoots.Init();
            ChoiceBuffs.Init();
            ChoiceManager.Init();
            ChoiceBand.Init();
            ChoiceSpecs.Init();
            ChoiceScarf.Init();

            isUnfinishedEnabled = ConfigManager.UnfinishedItemsEnabled.Value;

            // Looking Glass (Optional)
            LookingGlassManager.Init();

            // Unfinished Items
            if (isUnfinishedEnabled)
            {
                
            }

            // Log that the mod is ready
            Log.Message("PokeItems mod is ready!");
        }
    }
}
