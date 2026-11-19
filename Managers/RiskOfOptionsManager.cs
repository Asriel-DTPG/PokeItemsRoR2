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

            Log.Info("Risk of Options detected. Adding PokeItems configuration to mod menu.");
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

                Log.Info("Risk of Options integration initialized successfully.");
            }
            catch (Exception e)
            {
                Log.Error("Failed to initialize Risk of Options integration:\n" + e);
            }
        }

        // Look through each Config entry and translate values into RiskOfOption values
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void RegisterOptions()
        {
            foreach (ConfigDefinition def in ConfigManager.ConfigDefinitions)
            {
                ConfigEntryBase entry = def.Entry;

                if (entry is ConfigEntry<bool> boolEntry)
                    ModSettingsManager.AddOption(new CheckBoxOption(boolEntry));

                else if (entry is ConfigEntry<float> floatEntry)
                    ModSettingsManager.AddOption(new SliderOption(floatEntry, new SliderConfig
                    {
                        min = def.MinValue,
                        max = def.MaxValue
                    }));


                else if (entry is ConfigEntry<int> intEntry)
                    ModSettingsManager.AddOption(new IntSliderOption(intEntry, new IntSliderConfig
                    {
                        min = (int)def.MinValue,
                        max = (int)def.MaxValue
                    }));

                else
                    Log.Warning("Risk of Options: Unsupported config type for " + $"'{entry.Definition.Section}/{entry.Definition.Key}'.");
            }
        }
    }
}
