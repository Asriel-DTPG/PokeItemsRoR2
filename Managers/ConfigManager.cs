using BepInEx;
using BepInEx.Configuration;
using System.IO;

namespace PokeItems.Managers
{
    internal class ConfigManager
    {
        // GENERAL
        public static ConfigEntry<bool> CustomValuesEnabled;
        public static ConfigEntry<bool> SpawnModeEnabled;
        public static ConfigEntry<bool> UnfinishedItemsEnabled;
        
        // LEFTOVERS
        public static ConfigEntry<float> Leftovers_RegenBonusPerStack;
        public static ConfigEntry<float> Leftovers_RegenBonusPerExtraStack;

        // FLAME ORB
        public static ConfigEntry<float> FlameOrb_ProcPercentPerStack;
        public static ConfigEntry<float> FlameOrb_DebuffDuration;

        // AIR BALLOON
        public static ConfigEntry<float> AirBalloon_FallSpeedLimit;
        public static ConfigEntry<float> AirBalloon_FallPercentReductionPerExtraStack;
        public static ConfigEntry<float> AirBalloon_HpThresholdPercent;

        public static void Init()
        {
            // Get the config file
            ConfigFile Config = PokeItems.PConfig;

            // GENERAL CONFIG
            CustomValuesEnabled = Config.Bind(
                "! Important !",
                "Use Custom Values",
                false,
                "Whether items utilise user-inputted/custom values instead of the preferred values [TURN THIS ON IF YOU WANT TO CUSTOMIZE VALUES]."
            );
            SpawnModeEnabled = Config.Bind(
                "! Important !",
                "Spawn Mode",
                false,
                "Enable Spawn Mode to allow spawning custom items via F-keys."
            );
            UnfinishedItemsEnabled = Config.Bind(
                "! Important !",
                "Enable Unfinished Items",
                false,
                "Allow early-access items that are either unfinished or currently being developed. NOTE: You will need to restart your game."
            );

            // LEFTOVERS CONFIG
            Leftovers_RegenBonusPerStack = Config.Bind(
                "Leftovers",
                "Initial Regeneration Bonus",
                4f,
                "How much health regeneration is applied from initial stack."
            );
            Leftovers_RegenBonusPerExtraStack = Config.Bind(
                "Leftovers",
                "Regeneration Bonus Per Extra Stack",
                2f,
                "How much health regeneration is applied per extra stack."
            );

            // FLAME ORB CONFIG
            FlameOrb_ProcPercentPerStack = Config.Bind(
                "Flame Orb",
                "Burn Chance Percent Per Stack",
                10f,
                "Chance to inflict burn per stack."
            );
            FlameOrb_DebuffDuration = Config.Bind(
                "Flame Orb",
                "Burn Duration",
                4f,
                "How long the burn debuff lasts."
            );

            // AIR BALLOON CONFIG
            AirBalloon_FallSpeedLimit = Config.Bind(
                "Air Balloon",
                "Fall Speed Limit",
                50f,
                "How much limit is applied to falling speed (m/s)."
            );
            AirBalloon_FallPercentReductionPerExtraStack = Config.Bind(
                "Air Balloon",
                "Fall Percent Reduction Per Extra Stack",
                10f,
                "How much the reduction percent over falling speed limit is exponentially applied."
            );
            AirBalloon_HpThresholdPercent = Config.Bind(
                "Air Balloon",
                "HP Threshold Percent",
                35f,
                "How low the HP can deplete before the balloon pops."
            );
        }
    }
}
