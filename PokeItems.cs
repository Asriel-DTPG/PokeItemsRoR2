using BepInEx;
using PokeItems.Items;
using R2API;
using R2API.Utils;
using RoR2;
using RoR2.ExpansionManagement;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace PokeItems
{
    // This attribute specifies that we have a dependency on a given BepInEx Plugin,
    // We need the R2API ItemAPI dependency because we are using for adding our item to the game.
    // You don't need this if you're not using R2API in your plugin,
    // it's just to tell BepInEx to initialize R2API before this plugin so it's safe to use R2API.
    [BepInDependency(ItemAPI.PluginGUID)]

    // This one is because we use a .language file for language tokens
    // More info in https://risk-of-thunder.github.io/R2Wiki/Mod-Creation/Assets/Localization/
    [BepInDependency(LanguageAPI.PluginGUID)]

    // This attribute is required, and lists metadata for your plugin.
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]

    // Compatibility
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.EveryoneNeedSameModVersion)]

    // This is the main declaration of our plugin class.
    // BepInEx searches for all classes inheriting from BaseUnityPlugin to initialize on startup.
    // BaseUnityPlugin itself inherits from MonoBehaviour,
    // so you can use this as a reference for what you can declare and use in your plugin class
    // More information in the Unity Docs: https://docs.unity3d.com/ScriptReference/MonoBehaviour.html
    public class PokeItems : BaseUnityPlugin
    {

        // The Plugin GUID should be a unique ID for this plugin,
        // which is human readable (as it is used in places like the config).
        // If we see this PluginGUID as it is on thunderstore,
        // we will deprecate this mod.
        // Change the PluginAuthor and the PluginName !
        public const string PluginGUID = PluginAuthor + "." + PluginName;
        public const string PluginAuthor = "Asriel_DTPG";
        public const string PluginName = "PokeItems";
        public const string PluginVersion = "0.0.1";

        public static ExpansionDef sotvDLC;
        public static ExpansionDef sotsDLC;

        // The Awake() method is run at the very start when the game is initialized.
        public void Awake()
        {
            sotvDLC = Addressables.LoadAssetAsync<ExpansionDef>("RoR2/DLC1/Common/DLC1.asset").WaitForCompletion();
            sotsDLC = Addressables.LoadAssetAsync<ExpansionDef>("RoR2/DLC2/Common/DLC2.asset").WaitForCompletion();

            // Init our logging class so that we can properly log for debugging
            Log.Init(Logger);

            // Items
            Leftovers.Init();
            FlameOrb.Init();
            PoppedAirBalloon.Init();
            AirBalloon.Init();

            Log.Message("PokeItems mod is ready!");
        }

        // The Update() method is run on every frame of the game.
        private void Update()
        {
            // This if statement checks if the player has currently pressed the desired key.

            // Air Balloon
            if (Input.GetKeyDown(KeyCode.F2))
            {
                // Get the player body to use a position:
                var transform = PlayerCharacterMasterController.instances[0].master.GetBodyObject().transform;

                // And then drop our defined item in front of the player.

                Log.Info($"Player pressed key. Spawning our custom item at coordinates {transform.position}");
                PickupDropletController.CreatePickupDroplet(PickupCatalog.FindPickupIndex(AirBalloon.itemDef.itemIndex), transform.position, transform.forward * 30f);
            }

            // Flame Orb
            if (Input.GetKeyDown(KeyCode.F3))
            {
                // Get the player body to use a position:
                var transform = PlayerCharacterMasterController.instances[0].master.GetBodyObject().transform;

                // And then drop our defined item in front of the player.

                Log.Info($"Player pressed key. Spawning our custom item at coordinates {transform.position}");
                PickupDropletController.CreatePickupDroplet(PickupCatalog.FindPickupIndex(FlameOrb.itemDef.itemIndex), transform.position, transform.forward * 30f);
            }

            // Leftovers
            if (Input.GetKeyDown(KeyCode.F4))
            {
                // Get the player body to use a position:
                var transform = PlayerCharacterMasterController.instances[0].master.GetBodyObject().transform;

                // And then drop our defined item in front of the player.

                Log.Info($"Player pressed key. Spawning our custom item at coordinates {transform.position}");
                PickupDropletController.CreatePickupDroplet(PickupCatalog.FindPickupIndex(Leftovers.itemDef.itemIndex), transform.position, transform.forward * 30f);
            }
        }
    }
}
