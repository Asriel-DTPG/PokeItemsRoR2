using BepInEx;
using BepInEx.Bootstrap;
using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

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
            // Get Risk Of Options assembly
            Assembly assembly = Chainloader.PluginInfos[RiskOfOptionsGUID].Instance.GetType().Assembly;

            Type modSettingsManager = assembly.GetType("RiskOfOptions.ModSettingsManager");
            Type checkBoxOption = assembly.GetType("RiskOfOptions.Options.CheckBoxOption");
            Type sliderOption = assembly.GetType("RiskOfOptions.Options.SliderOption");

            if (modSettingsManager == null ||
                checkBoxOption == null ||
                sliderOption == null)
            {
                throw new Exception("Unable to find the required Risk Of Options types.");
            }

            // Find AddOption
            MethodInfo addOption = modSettingsManager.GetMethod("AddOption", BindingFlags.Public | BindingFlags.Static);

            if (addOption == null)
            {
                throw new Exception("Unable to find RiskOfOptions.ModSettingsManager.AddOption.");
            }

            // General
            AddBool(addOption, checkBoxOption, ConfigManager.CustomValuesEnabled);
            AddBool(addOption, checkBoxOption, ConfigManager.SpawnModeEnabled);
            AddBool(addOption, checkBoxOption, ConfigManager.UnfinishedItemsEnabled);

            // Leftovers
            AddFloat(addOption, sliderOption, ConfigManager.Leftovers_RegenBonusPerStack);
            AddFloat(addOption, sliderOption, ConfigManager.Leftovers_RegenBonusPerExtraStack);

            // Flame Orb
            AddFloat(addOption, sliderOption, ConfigManager.FlameOrb_ProcPercentPerStack);
            AddFloat(addOption, sliderOption, ConfigManager.FlameOrb_DebuffDuration);

            // Air Balloon
            AddFloat(addOption, sliderOption, ConfigManager.AirBalloon_FallSpeedLimit);
            AddFloat(addOption, sliderOption, ConfigManager.AirBalloon_FallPercentReductionPerExtraStack);
            AddFloat(addOption, sliderOption, ConfigManager.AirBalloon_HpThresholdPercent);

            // EXP Share
            AddFloat(addOption, sliderOption, ConfigManager.ExpShare_ExpPercentBonusPerStack);
            AddFloat(addOption, sliderOption, ConfigManager.ExpShare_ExpPercentBonusPerExtraStack);
            AddFloat(addOption, sliderOption, ConfigManager.ExpShare_ExpRateBonus);

            // Amulet Coin
            AddFloat(addOption, sliderOption, ConfigManager.AmuletCoin_GoldMulPercentPerStack);

            // Heavy-Duty Boots
            AddFloat(addOption, sliderOption, ConfigManager.HeavyDutyBoots_ArmorBonusPerStack);
            AddFloat(addOption, sliderOption, ConfigManager.HeavyDutyBoots_ArmorBonusPerExtraStack);
        }

        // Add Invoke to Bool
        private static void AddBool(MethodInfo addOption, Type optionType, ConfigEntry<bool> config)
        {
            if (config == null)
                return;

            object option = Activator.CreateInstance(optionType, config);

            addOption.Invoke(null, [option]);
        }

        // Add Invoke to Float
        private static void AddFloat(MethodInfo addOption, Type optionType, ConfigEntry<float> config)
        {
            if (config == null)
                return;

            object option = Activator.CreateInstance(optionType, config);

            addOption.Invoke(null, [option]);
        }
    }
}
