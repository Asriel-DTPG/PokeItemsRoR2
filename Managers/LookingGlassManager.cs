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

            if(!Chainloader.PluginInfos.ContainsKey(LookingGlassGUID))
                return;

            Log.Info("Looking Glass detected. Waiting till ItemCatalog is available.");
            ItemCatalog.availability.CallWhenAvailable(RegisterLG);
        }

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

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        private static void RegisterItems()
        {
            RegisterLeftovers();
        }

        private static ItemStatsDef GetItemStats(ItemDef itemDef)
        {
            if (itemDef == null)
                return null;
            
            if (itemDef.itemIndex == ItemIndex.None)
            {
                Log.Warning($"Looking Glass: ItemDef '{itemDef.name}' still has ItemIndex.None. " + "Skipping registration.");
                return null;
            }
            
            int itemIndex = (int) itemDef.itemIndex;

            if (ItemDefinitions.allItemDefinitions.TryGetValue(itemIndex, out ItemStatsDef existingStats))
                return existingStats;

            ItemStatsDef stats = new ItemStatsDef();

            ItemDefinitions.allItemDefinitions[itemIndex] = stats;

            return stats;
        }

        private static void RegisterLeftovers()
        {
            if (Leftovers.itemDef == null)
                return;

            ItemStatsDef stats = GetItemStats(Leftovers.itemDef);

            if (stats == null)
                return;

            stats.descriptions.Add("Regeneration Bonus: ");
            stats.valueTypes.Add(ItemStatsDef.ValueType.Health);
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
    }
}
