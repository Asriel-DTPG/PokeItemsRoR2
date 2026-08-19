using BepInEx;
using BepInEx.Bootstrap;
using BepInEx.Configuration;
using System;
using RiskOfOptions;
using RiskOfOptions.Options;
using RiskOfOptions.OptionConfigs;

namespace PokeItems.Managers
{
    internal static class RiskOfOptionsManager
    {
        private const string RiskOfOptionsGUID = "com.rune580.riskofoptions";

        private static bool initialized = false;

        public static void Init()
        {
            if (initialized)
                return;

            initialized = true;

            // Ignore all of this if Risk Of Options is not installed
            if (!Chainloader.PluginInfos.ContainsKey(RiskOfOptionsGUID))
                return;
            
            try
            {
                RegisterOptions();

                Log.Info("Risk of Options detected. Added PokeItems configuration to mod menu.");
            }
            catch (Exception e)
            {
                Log.Error("Failed to initialize Risk of Options integration:\n" + e);
            }
        }

        private static void RegisterOptions()
        {

            // General
            ModSettingsManager.AddOption(
                new CheckBoxOption(
                    ConfigManager.CustomValuesEnabled
                )
            );
            ModSettingsManager.AddOption(
                new CheckBoxOption(
                    ConfigManager.SpawnModeEnabled
                )
            );
            ModSettingsManager.AddOption(
                new CheckBoxOption(
                    ConfigManager.UnfinishedItemsEnabled
                )
            );

            // Leftovers
            ModSettingsManager.AddOption(
                new SliderOption(
                    ConfigManager.Leftovers_RegenBonusPerStack,
                    new SliderConfig
                    {
                        min = 0f,
                        max = 100f
                    }
                )
            );
            ModSettingsManager.AddOption(
                new SliderOption(
                    ConfigManager.Leftovers_RegenBonusPerExtraStack,
                    new SliderConfig
                    {
                        min = 0f,
                        max = 100f
                    }
                )
            );

            // Flame Orb
            ModSettingsManager.AddOption(
                new SliderOption(
                    ConfigManager.FlameOrb_ProcPercentPerStack,
                    new SliderConfig
                    {
                        min = 0f,
                        max = 100f
                    }
                )
            );
            ModSettingsManager.AddOption(
                new SliderOption(
                    ConfigManager.FlameOrb_DebuffDuration,
                    new SliderConfig
                    {
                        min = 0f,
                        max = 30f
                    }
                )
            );

            // Air Balloon
            ModSettingsManager.AddOption(
                new SliderOption(
                    ConfigManager.AirBalloon_FallSpeedLimit,
                    new SliderConfig
                    {
                        min = 0f,
                        max = 200f
                    }
                )
            );
            ModSettingsManager.AddOption(
                new SliderOption(
                    ConfigManager.AirBalloon_FallPercentReductionPerExtraStack,
                    new SliderConfig
                    {
                        min = 0f,
                        max = 100f
                    }
                )
            );
            ModSettingsManager.AddOption(
                new SliderOption(
                    ConfigManager.AirBalloon_HpThresholdPercent,
                    new SliderConfig
                    {
                        min = 0f,
                        max = 100f
                    }
                )
            );

            // EXP Share
            ModSettingsManager.AddOption(
                new SliderOption(
                    ConfigManager.ExpShare_ExpPercentBonusPerStack,
                    new SliderConfig
                    {
                        min = 0f,
                        max = 1000f
                    }
                )
            );
            ModSettingsManager.AddOption(
                new SliderOption(
                    ConfigManager.ExpShare_ExpPercentBonusPerExtraStack,
                    new SliderConfig
                    {
                        min = 0f,
                        max = 1000f
                    }
                )
            );
            ModSettingsManager.AddOption(
                new SliderOption(
                    ConfigManager.ExpShare_ExpRateBonus,
                    new SliderConfig
                    {
                        min = 0f,
                        max = 100f
                    }
                )
            );

            // Amulet Coin
            ModSettingsManager.AddOption(
                new SliderOption(
                    ConfigManager.AmuletCoin_GoldMulPercentPerStack,
                    new SliderConfig
                    {
                        min = 0f,
                        max = 1000f
                    }
                )
            );

            // Heavy-Duty Boots
            ModSettingsManager.AddOption(
                new SliderOption(
                    ConfigManager.HeavyDutyBoots_ArmorBonusPerStack,
                    new SliderConfig
                    {
                        min = 0f,
                        max = 1000f
                    }
                )
            );
            ModSettingsManager.AddOption(
                new SliderOption(
                    ConfigManager.HeavyDutyBoots_ArmorBonusPerExtraStack,
                    new SliderConfig
                    {
                        min = 0f,
                        max = 1000f
                    }
                )
            );
        }
    }
}
