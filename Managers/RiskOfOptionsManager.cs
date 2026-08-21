using BepInEx;
using BepInEx.Bootstrap;
using BepInEx.Configuration;
using System;
using System.Runtime.CompilerServices;
using RiskOfOptions;
using RiskOfOptions.Options;
using RiskOfOptions.OptionConfigs;
using UnityEngine;

namespace PokeItems.Managers
{
    internal static class RiskOfOptionsManager
    {
        private const string RiskOfOptionsGUID = "com.rune580.riskofoptions";

        private const string IconName = "ModIcon";

        private static bool initialized = false;

        public static void Init()
        {
            if (initialized)
                return;

            initialized = true;

            // Ignore all of this if Risk Of Options is not installed
            if (!Chainloader.PluginInfos.ContainsKey(RiskOfOptionsGUID))
                return;

            InitializeRiskOfOptions();
        }

        // This will prevent the attempt to resolve dll for RiskOfOptions before confirming that the plugin exists
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void InitializeRiskOfOptions()
        {
            try
            {
                // Attempt to add icon
                Sprite icon = AssetManager.bundle.LoadAsset<Sprite>(IconName + ".png");
                ModSettingsManager.SetModIcon(icon);

                // Attempt to register options according to ConfigManager
                RegisterOptions();

                Log.Info("Risk of Options detected. Added PokeItems configuration to mod menu.");
            }
            catch (Exception e)
            {
                Log.Error("Failed to initialize Risk of Options integration:\n" + e);
            }
        }

        // Make existing options according to ConfigManager visible in-game
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

            // Choice Items
            ModSettingsManager.AddOption(
                new SliderOption(
                    ConfigManager.ChoiceItems_CooldownPenaltyPerStack,
                    new SliderConfig
                    {
                        min = 0f,
                        max = 100f
                    }
                )
            );

            // Choice Band
            ModSettingsManager.AddOption(
                new SliderOption(
                    ConfigManager.ChoiceBand_DamageBonus,
                    new SliderConfig
                    {
                        min = 0f,
                        max = 1000f
                    }
                )
            );
            ModSettingsManager.AddOption(
                new SliderOption(
                    ConfigManager.ChoiceBand_DamageBonusPerExtraStack,
                    new SliderConfig
                    {
                        min = 0f,
                        max = 1000f
                    }
                )
            );

            // Choice Specs
            ModSettingsManager.AddOption(
                new SliderOption(
                    ConfigManager.ChoiceSpecs_DamageBonus,
                    new SliderConfig
                    {
                        min = 0f,
                        max = 1000f
                    }
                )
            );
            ModSettingsManager.AddOption(
                new SliderOption(
                    ConfigManager.ChoiceSpecs_DamageBonusPerExtraStack,
                    new SliderConfig
                    {
                        min = 0f,
                        max = 1000f
                    }
                )
            );

            // Choice Scarf
            ModSettingsManager.AddOption(
                new SliderOption(
                    ConfigManager.ChoiceScarf_MovementBonus,
                    new SliderConfig
                    {
                        min = 0f,
                        max = 1000f
                    }
                )
            );
            ModSettingsManager.AddOption(
                new SliderOption(
                    ConfigManager.ChoiceScarf_MovementBonusPerExtraStack,
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
