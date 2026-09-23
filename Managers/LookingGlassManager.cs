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
            RegisterExpShare();
            RegisterAmuletCoin();
            RegisterHeavyDutyBoots();
            RegisterChoiceBand();
            RegisterChoiceSpecs();
            RegisterChoiceScarf();
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
                    procPercent, stackCount, procChance);

                return new List<float>
                {
                    chance / 100f
                };
            };
        }

        // Register stats for Air Balloon
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

            stats.calculateValuesNew = (luck, stackCount, procChance) =>
            {
                float fallSpeedLimit = ConfigManager.GetFloatValue(
                    ConfigManager.AirBalloon_FallSpeedLimit,
                    AirBalloon.fallSpeedLimit);

                float fallReduction = ConfigManager.GetFloatValue(
                    ConfigManager.AirBalloon_FallPercentReductionPerExtraStack,
                    AirBalloon.fallPercentReductionPerExtraStack);

                float value = fallSpeedLimit * MathUtility.GetExponentialPercentReductionStacking(fallReduction, stackCount - 1);

                return new List<float>
                {
                    value
                };
            };
        }

        // Register stats for EXP Share
        private static void RegisterExpShare()
        {
            if (ExpShare.itemDef == null)
                return;

            ItemStatsDef stats = GetItemStats(ExpShare.itemDef);

            if (stats == null)
                return;

            stats.descriptions.Add("EXP Bonus: ");
            stats.valueTypes.Add(ItemStatsDef.ValueType.Utility);
            stats.measurementUnits.Add(ItemStatsDef.MeasurementUnits.Percentage);

            stats.calculateValuesNew = (luck, stackCount, procChance) =>
            {
                float expBonus = ConfigManager.GetFloatValue(
                    ConfigManager.ExpShare_ExpPercentBonusPerStack,
                    ExpShare.expPercentBonusPerStack);

                float expExtraBonus = ConfigManager.GetFloatValue(
                    ConfigManager.ExpShare_ExpPercentBonusPerExtraStack,
                    ExpShare.expPercentBonusPerExtraStack);

                float bonus = MathUtility.GetLinearWithExtraStacking(
                    expBonus, expExtraBonus, stackCount);

                return new List<float>
                {
                    bonus / 100f
                };
            };
        }

        // Register stats for Amulet Coin
        private static void RegisterAmuletCoin()
        {
            if (AmuletCoin.itemDef == null)
                return;

            ItemStatsDef stats = GetItemStats(AmuletCoin.itemDef);

            if (stats == null)
                return;

            stats.descriptions.Add("Gold Bonus: ");
            stats.valueTypes.Add(ItemStatsDef.ValueType.Utility);
            stats.measurementUnits.Add(ItemStatsDef.MeasurementUnits.Percentage);

            stats.calculateValuesNew = (luck, stackCount, procChance) =>
            {
                float goldBonus = ConfigManager.GetFloatValue(
                    ConfigManager.AmuletCoin_GoldMulPercentPerStack,
                    AmuletCoin.goldMulPercentPerStack);

                return new List<float>
                {
                    goldBonus * stackCount / 100f
                };
            };
        }

        // Register stats for Heavy Duty Boots
        private static void RegisterHeavyDutyBoots()
        {
            if (HeavyDutyBoots.itemDef == null)
                return;

            ItemStatsDef stats = GetItemStats(HeavyDutyBoots.itemDef);

            if (stats == null)
                return;

            stats.descriptions.Add("Armor Bonus: ");
            stats.valueTypes.Add(ItemStatsDef.ValueType.Armor);
            stats.measurementUnits.Add(ItemStatsDef.MeasurementUnits.Number);

            stats.calculateValuesNew = (luck, stackCount, procChance) =>
            {
                float armorBonus = ConfigManager.GetFloatValue(
                    ConfigManager.HeavyDutyBoots_ArmorBonusPerStack,
                    HeavyDutyBoots.armorBonusPerStack);

                float armorExtraBonus = ConfigManager.GetFloatValue(
                    ConfigManager.HeavyDutyBoots_ArmorBonusPerExtraStack,
                    HeavyDutyBoots.armorBonusPerExtraStack);

                float armor = MathUtility.GetLinearWithExtraStacking(
                    armorBonus,
                    armorExtraBonus,
                    stackCount);

                return new List<float>
                {
                    armor
                };
            };
        }

        // Register stats for Choice Band
        private static void RegisterChoiceBand()
        {
            if (ChoiceBand.itemDef == null)
                return;

            ItemStatsDef stats = GetItemStats(ChoiceBand.itemDef);

            if (stats == null)
                return;

            stats.descriptions.Add("Primary/Secondary Damage Bonus: ");
            stats.valueTypes.Add(ItemStatsDef.ValueType.Damage);
            stats.measurementUnits.Add(ItemStatsDef.MeasurementUnits.Percentage);

            stats.calculateValuesNew = (luck, stackCount, procChance) =>
            {
                float damageBonus = ConfigManager.GetFloatValue(
                    ConfigManager.ChoiceBand_DamageBonus,
                    ChoiceBand.damageBonus);

                float damageExtraBonus = ConfigManager.GetFloatValue(
                    ConfigManager.ChoiceBand_DamageBonusPerExtraStack,
                    ChoiceBand.damageBonusPerExtraStack);

                float value = MathUtility.GetLinearWithExtraStacking(
                    damageBonus, damageExtraBonus, stackCount);

                return new List<float>
                {
                    value / 100f
                };
            };
        }

        // Register stats for Choice Specs
        private static void RegisterChoiceSpecs()
        {
            if (ChoiceSpecs.itemDef == null)
                return;

            ItemStatsDef stats = GetItemStats(ChoiceSpecs.itemDef);

            if (stats == null)
                return;

            stats.descriptions.Add("Utility/Special Damage Bonus: ");
            stats.valueTypes.Add(ItemStatsDef.ValueType.Damage);
            stats.measurementUnits.Add(ItemStatsDef.MeasurementUnits.Percentage);

            stats.calculateValuesNew = (luck, stackCount, procChance) =>
            {
                float damageBonus = ConfigManager.GetFloatValue(
                    ConfigManager.ChoiceSpecs_DamageBonus,
                    ChoiceSpecs.damageBonus);

                float damageExtraBonus = ConfigManager.GetFloatValue(
                    ConfigManager.ChoiceSpecs_DamageBonusPerExtraStack,
                    ChoiceSpecs.damageBonusPerExtraStack);

                float value = MathUtility.GetLinearWithExtraStacking(
                    damageBonus, damageExtraBonus, stackCount);

                return new List<float>
                {
                    value / 100f
                };
            };
        }

        // Register stats for Choice Scarf
        private static void RegisterChoiceScarf()
        {
            if (ChoiceScarf.itemDef == null)
                return;

            ItemStatsDef stats = GetItemStats(ChoiceScarf.itemDef);

            if (stats == null)
                return;

            stats.descriptions.Add("Movement Bonus: ");
            stats.valueTypes.Add(ItemStatsDef.ValueType.Utility);
            stats.measurementUnits.Add(ItemStatsDef.MeasurementUnits.Percentage);

            stats.calculateValuesNew = (luck, stackCount, procChance) =>
            {
                float movementBonus = ConfigManager.GetFloatValue(
                    ConfigManager.ChoiceScarf_MovementBonus,
                    ChoiceScarf.movementBonus);

                float movementExtraBonus = ConfigManager.GetFloatValue(
                    ConfigManager.ChoiceScarf_MovementBonusPerExtraStack,
                    ChoiceScarf.movementBonusPerExtraStack);

                float value = MathUtility.GetLinearWithExtraStacking(
                    movementBonus, movementExtraBonus, stackCount);

                return new List<float>
                {
                    value / 100f
                };
            };
        }
    }
}
