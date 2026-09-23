using LookingGlass.ItemStatsNameSpace;
using PokeItems.Items;
using PokeItems.Managers;
using BepInEx.Bootstrap;
using RiskOfOptions;
using RoR2;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace PokeItems.Managers
{
    internal class LookingGlassManager
    {
        private const string LookingGlassGUID = "droppod.lookingglass";
        private static bool initialized = false;
        private static bool registered = false;
        
        public static void Init()
        {
            if (initialized)
                return;

            initialized = true;

            // Ignore all of this if Looking Glass is not installed
            if (!Chainloader.PluginInfos.ContainsKey(LookingGlassGUID))
                return;

            Log.Info("Looking Glass detected. Waiting till ItemCatalog is available.");
            ItemCatalog.availability.CallWhenAvailable(RegisterLG);
        }

        // Register custom item stats via Looking Glass
        private static void RegisterLG()
        {
            Log.Info("Registering PokeItems stats to Looking Glass.");

            try
            {
                // Attempt to reveal item stats via Looking Glass
                RegisterItems();

                Log.Info("Looking Glass integration initialized successfully.");
            }
            catch (Exception e)
            {
                Log.Error("Failed to initialize Looking Glass integration:\n" + e);
            }
        }

        // This will prevent the attempt to resolve dll for RiskOfOptions before confirming that the plugin exists
        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        private static void RegisterItems()
        {
            RegisterLeftovers();
            RegisterFlameOrb();
            RegisterAirBalloon();
        }

        // Get or create designated item stats
        private static ItemStatsDef GetItemStats(ItemDef itemDef)
        {
            // Mandatory checks
            if (itemDef == null)
                return null;
            
            if (itemDef.itemIndex == ItemIndex.None)
            {
                Log.Warning($"Looking Glass: ItemDef '{itemDef.name}' still has ItemIndex.None. " + "Skipping registration.");
                return null;
            }
            
            int itemIndex = (int) itemDef.itemIndex;

            // If item stats exist, get the existing stats instead
            if (ItemDefinitions.allItemDefinitions.TryGetValue(itemIndex, out ItemStatsDef existingStats))
                return existingStats;

            ItemStatsDef stats = new ItemStatsDef();

            ItemDefinitions.allItemDefinitions[itemIndex] = stats;

            return stats;
        }

        // Register stats for Leftovers
        private static void RegisterLeftovers()
        {
            if (Leftovers.itemDef == null)
                return;

            ItemStatsDef stats = GetItemStats(Leftovers.itemDef);

            if (stats == null)
                return;

            stats.descriptions.Add("Regeneration Bonus: ");
            stats.valueTypes.Add(ItemStatsDef.ValueType.Healing);
            stats.measurementUnits.Add(ItemStatsDef.MeasurementUnits.FlatHealing);

            stats.calculateValuesNew = (luck, stackCount, procChance) =>
            {
                float regenBonus = ConfigManager.GetFloatValue(
                    ConfigManager.Leftovers_RegenBonusPerStack,
                    Leftovers.regenBonusPerStack);

                float regenExtraBonus = ConfigManager.GetFloatValue(
                    ConfigManager.Leftovers_RegenBonusPerExtraStack,
                    Leftovers.regenBonusPerExtraStack);

                float value = MathUtility.GetLinearWithExtraStacking(
                    regenBonus, regenExtraBonus, stackCount);

                return new List<float>
                {
                    value
                };
            };
        }

        // Register stats for Flame Orb
        private static void RegisterFlameOrb()
        {
            if (FlameOrb.itemDef == null)
                return;

            ItemStatsDef stats = GetItemStats(FlameOrb.itemDef);

            if (stats == null)
                return;

            stats.descriptions.Add("Burn Chance: ");
            stats.valueTypes.Add(ItemStatsDef.ValueType.Damage);
            stats.measurementUnits.Add(ItemStatsDef.MeasurementUnits.Percentage);

            stats.calculateValuesNew = (luck, stackCount, procChance) =>
            {
                float procPercent = ConfigManager.GetFloatValue(
                    ConfigManager.FlameOrb_ProcPercentPerStack,
                    FlameOrb.procPercentPerStack);

                float chance = MathUtility.GetLinearStacking(
                    procPercent, stackCount, procChance) / 100f;

                return new List<float>
                {
                    chance
                };
            };
        }

        // Register stats for Flame Orb
        private static void RegisterAirBalloon()
        {
            if (AirBalloon.itemDef == null)
                return;

            ItemStatsDef stats = GetItemStats(AirBalloon.itemDef);

            if (stats == null)
                return;

            stats.descriptions.Add("Fall Speed Limit: ");
            stats.valueTypes.Add(ItemStatsDef.ValueType.Utility);
            stats.measurementUnits.Add(ItemStatsDef.MeasurementUnits.Meters);

            stats.descriptions.Add("HP Threshold: ");
            stats.valueTypes.Add(ItemStatsDef.ValueType.Health);
            stats.measurementUnits.Add(ItemStatsDef.MeasurementUnits.Percentage);

            stats.calculateValuesNew = (luck, stackCount, procChance) =>
            {
                float fallSpeedLimit = ConfigManager.GetFloatValue(
                    ConfigManager.AirBalloon_FallSpeedLimit,
                    AirBalloon.fallSpeedLimit);

                float fallReduction = ConfigManager.GetFloatValue(
                    ConfigManager.AirBalloon_FallPercentReductionPerExtraStack,
                    AirBalloon.fallPercentReductionPerExtraStack);

                float hpThreshold = ConfigManager.GetFloatValue(
                    ConfigManager.AirBalloon_HpThresholdPercent,
                    AirBalloon.hpThresholdPercent);

                float fallValue = fallSpeedLimit * MathUtility.GetExponentialPercentReductionStacking(fallReduction, stackCount - 1);

                return new List<float>
                {
                    fallValue,
                    hpThreshold
                };
            };
        }
    }
}
