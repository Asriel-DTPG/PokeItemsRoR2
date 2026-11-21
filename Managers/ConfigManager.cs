using System;
using System.Collections.Generic;
using BepInEx.Configuration;
using PokeItems.Items;

namespace PokeItems.Managers
{
    internal class ConfigManager
    {
        private static readonly List<ConfigDefinition> configDefinitions = new List<ConfigDefinition>();
        public static IReadOnlyList<ConfigDefinition> ConfigDefinitions => configDefinitions;

        private static bool initialized;

        // Record config option to list of entries, which can then be used for any mods that require these values, such as RiskOfOptions
        private static ConfigEntry<T> BindOption<T>(ConfigFile config, string section, string key, T defaultValue, string desc, float minValue = 0f, float maxValue = 100f)
        {
            ConfigEntry<T> entry = config.Bind(section, key, defaultValue, desc);

            // Avoid duplicate registration if initialization is repeated
            for (int i = 0; i < configDefinitions.Count; i++) {
                if (ReferenceEquals(configDefinitions[i].Entry, entry))
                    return entry;
            }

            configDefinitions.Add(new ConfigDefinition(entry, minValue, maxValue));

            return entry;
        }
        
        // GENERAL
        public static ConfigEntry<bool> CustomValuesEnabled;
        public static ConfigEntry<bool> SpawnModeEnabled;
        public static ConfigEntry<bool> UnfinishedItemsEnabled;
        
        // LEFTOVERS
        public static ConfigEntry<float> Leftovers_RegenBonus;
        public static ConfigEntry<float> Leftovers_RegenBonusPercent;
        public static ConfigEntry<float> Leftovers_RegenBonusPerExtraStack;
        public static ConfigEntry<float> Leftovers_RegenBonusPercentPerExtraStack;

        // FLAME ORB
        public static ConfigEntry<float> FlameOrb_ProcPercentPerStack;
        public static ConfigEntry<float> FlameOrb_DebuffDuration;

        // AIR BALLOON
        public static ConfigEntry<float> AirBalloon_FallSpeedLimit;
        public static ConfigEntry<float> AirBalloon_FallPercentReductionPerExtraStack;
        public static ConfigEntry<float> AirBalloon_HpThresholdPercent;

        // EXP SHARE
        public static ConfigEntry<float> ExpShare_ExpPercentBonus;
        public static ConfigEntry<float> ExpShare_ExpPercentBonusPerExtraStack;
        public static ConfigEntry<float> ExpShare_ExpRateBonus;

        // AMULET COIN
        public static ConfigEntry<float> AmuletCoin_GoldMulPercentPerStack;

        // HEAVY-DUTY BOOTS
        public static ConfigEntry<float> HeavyDutyBoots_ArmorBonus;
        public static ConfigEntry<float> HeavyDutyBoots_ArmorBonusPerExtraStack;

        // CHOICE ITEMS
        public static ConfigEntry<float> ChoiceItems_CooldownPenaltyPerStack;

        // CHOICE BAND
        public static ConfigEntry<float> ChoiceBand_DamageBonus;
        public static ConfigEntry<float> ChoiceBand_DamageBonusPerExtraStack;

        // CHOICE SPECS
        public static ConfigEntry<float> ChoiceSpecs_DamageBonus;
        public static ConfigEntry<float> ChoiceSpecs_DamageBonusPerExtraStack;

        // CHOICE SCARF
        public static ConfigEntry<float> ChoiceScarf_MovementBonus;
        public static ConfigEntry<float> ChoiceScarf_MovementBonusPerExtraStack;

        // WEAKNESS POLICY
        public static ConfigEntry<float> WeaknessPolicy_DamageBonus;
        public static ConfigEntry<int> WeaknessPolicy_MaxBuffStacks;
        public static ConfigEntry<int> WeaknessPolicy_MaxBuffStacksPerExtraStack;
        public static ConfigEntry<float> WeaknessPolicy_Duration;

        // QUICK CLAW
        public static ConfigEntry<float> QuickClaw_ProcChance;
        public static ConfigEntry<float> QuickClaw_ProcChancePerExtraStack;
        public static ConfigEntry<float> QuickClaw_CooldownReductionPercent;

        // BERRY JUICE
        public static ConfigEntry<float> BerryJuice_RegenPercent;
        public static ConfigEntry<int> BerryJuice_ItemCap;
        public static ConfigEntry<int> BerryJuice_ItemCapPerExtraStack;

        // LIFE ORB
        public static ConfigEntry<float> LifeOrb_HpPercentSacrificePerStack;
        public static ConfigEntry<float> LifeOrb_DamageOutputMultPercent;
        public static ConfigEntry<float> LifeOrb_DamageOutputMultPercentPerExtraStack;

        public static void Init()
        {
            if (initialized)
                return;

            initialized = true;
            
            // Get the config file
            ConfigFile Config = PokeItems.PConfig;

            // GENERAL CONFIG
            CustomValuesEnabled = BindOption(Config,
                "! Important !",
                "Use Custom Values",
                false,
                "Whether items utilise user-inputted/custom values instead of the preferred values [TURN THIS ON IF YOU WANT TO CUSTOMIZE VALUES]."
            );
            SpawnModeEnabled = BindOption(Config,
                "! Important !",
                "Spawn Mode",
                false,
                "Enable Spawn Mode to allow spawning custom items via F-keys."
            );
            UnfinishedItemsEnabled = BindOption(Config,
                "! Important !",
                "Enable Unfinished Items",
                false,
                "Allow early-access items that are either unfinished or currently being developed. NOTE: You will need to restart your game."
            );

            // LEFTOVERS CONFIG
            Leftovers_RegenBonus = BindOption(Config,
                "Leftovers",
                "Initial Regeneration Bonus Percent",
                Leftovers.regenBonus,
                "Health regeneration scaled by max HP (%) applied from initial stack.",
                0f, 100f
            );
            Leftovers_RegenBonusPerExtraStack = BindOption(Config,
                "Leftovers",
                "Regeneration Bonus Percent Per Extra Stack",
                Leftovers.regenBonusPerExtraStack,
                "Health regeneration scaled by max HP (%) applied per extra stack.",
                0f, 100f
            );

            // FLAME ORB CONFIG
            FlameOrb_ProcPercentPerStack = BindOption(Config,
                "Flame Orb",
                "Burn Chance Percent Per Stack",
                FlameOrb.procPercentPerStack,
                "Chance to inflict burn per stack.",
                0f, 100f
            );
            FlameOrb_DebuffDuration = BindOption(Config,
                "Flame Orb",
                "Burn Duration",
                FlameOrb.debuffDuration,
                "How long the burn debuff lasts (sec).",
                0f, 60f
            );

            // AIR BALLOON CONFIG
            AirBalloon_FallSpeedLimit = BindOption(Config,
                "Air Balloon",
                "Fall Speed Limit",
                AirBalloon.fallSpeedLimit,
                "Limit applied to falling speed (m/s).",
                0f, 200f
            );
            AirBalloon_FallPercentReductionPerExtraStack = BindOption(Config,
                "Air Balloon",
                "Fall Percent Reduction Per Extra Stack",
                AirBalloon.fallPercentReductionPerExtraStack,
                "Reduction percent over falling speed limit exponentially applied.",
                0f, 100f
            );
            AirBalloon_HpThresholdPercent = BindOption(Config,
                "Air Balloon",
                "HP Threshold Percent",
                AirBalloon.hpThresholdPercent,
                "How low the HP can deplete before the balloon pops.",
                0f, 100f
            );

            // EXP SHARE CONFIG
            ExpShare_ExpPercentBonus = BindOption(Config,
                "EXP Share",
                "Initial EXP Percent Bonus",
                ExpShare.expPercentBonus,
                "Extra percent of EXP is applied from initial stack.",
                0f, 1000f
            );
            ExpShare_ExpPercentBonusPerExtraStack = BindOption(Config,
                "EXP Share",
                "EXP Percent Bonus Per Extra Stack",
                ExpShare.expPercentBonusPerExtraStack,
                "Extra percent of EXP is applied per extra stack.",
                0f, 1000f
            );
            ExpShare_ExpRateBonus = BindOption(Config,
                "EXP Share",
                "Required EXP Percent Rate",
                ExpShare.expRateBonus,
                "Percent amount of required EXP gained per minute.",
                0f, 100f
            );

            // AMULET COIN CONFIG
            AmuletCoin_GoldMulPercentPerStack = BindOption(Config,
                "Amulet Coin",
                "Gold Percent Bonus",
                AmuletCoin.goldMulPercentPerStack,
                "Extra percent of gold is applied per stack.",
                0f, 1000f
            );

            // HEAVY-DUTY BOOTS CONFIG
            HeavyDutyBoots_ArmorBonus = BindOption(Config,
                "Heavy-Duty Boots",
                "Initial Armor Bonus",
                HeavyDutyBoots.armorBonus,
                "Extra percent of EXP is applied from initial stack.",
                0f, 1000f
            );
            HeavyDutyBoots_ArmorBonusPerExtraStack = BindOption(Config,
                "Heavy-Duty Boots",
                "Armor Bonus Per Extra Stack",
                HeavyDutyBoots.armorBonusPerExtraStack,
                "How long the burn debuff lasts.",
                0f, 1000f
            );

            // CHOICE ITEMS CONFIG
            ChoiceItems_CooldownPenaltyPerStack = BindOption(Config,
                "Choice Items",
                "Cooldown Penalty Per Stack",
                ChoiceManager.cooldownPenaltyPerStack,
                "How slow the non-preferred skill cooldown recharges.",
                0f, 100f
            );

            // CHOICE BAND CONFIG
            ChoiceBand_DamageBonus = BindOption(Config,
                "Choice Band",
                "Initial Damage Bonus",
                ChoiceBand.damageBonus,
                "Initial damage bonus percent for Primary and Secondary skills.",
                0f, 1000f
            );
            ChoiceBand_DamageBonusPerExtraStack = BindOption(Config,
                "Choice Band",
                "Damage Bonus Per Extra Stack",
                ChoiceBand.damageBonusPerExtraStack,
                "Damage bonus percent for Primary and Secondary skills per extra stack.",
                0f, 1000f
            );

            // CHOICE SPECS CONFIG
            ChoiceSpecs_DamageBonus = BindOption(Config,
                "Choice Specs",
                "Initial Damage Bonus",
                ChoiceSpecs.damageBonus,
                "Initial damage bonus percent for Utility and Special skills.",
                0f, 1000f
            );
            ChoiceSpecs_DamageBonusPerExtraStack = BindOption(Config,
                "Choice Specs",
                "Damage Bonus Per Extra Stack",
                ChoiceSpecs.damageBonusPerExtraStack,
                "Damage bonus percent for Utility and Special skills per extra stack.",
                0f, 1000f
            );

            // CHOICE SCARF CONFIG
            ChoiceScarf_MovementBonus = BindOption(Config,
                "Choice Scarf",
                "Initial Movement Bonus",
                ChoiceScarf.movementBonus,
                "Initial movement bonus percent.",
                0f, 1000f
            );
            ChoiceScarf_MovementBonusPerExtraStack = BindOption(Config,
                "Choice Scarf",
                "Movement Bonus Per Extra Stack",
                ChoiceScarf.movementBonusPerExtraStack,
                "Movement bonus percent per extra stack.",
                0f, 1000f
            );

            // WEAKNESS POLICY CONFIG
            WeaknessPolicy_DamageBonus = BindOption(Config,
                "Weakness Policy",
                "Damage Bonus",
                WeaknessPolicy.damageBonus,
                "Damage bonus percent per buff stack.",
                0f, 1000f
            );
            WeaknessPolicy_MaxBuffStacks = BindOption(Config,
                "Weakness Policy",
                "Initial Max Buff Stacks",
                WeaknessPolicy.maxBuffStacks,
                "Initial max amount of Weakness Policy buff stacks.",
                0, 100
            );
            WeaknessPolicy_MaxBuffStacksPerExtraStack = BindOption(Config,
                "Weakness Policy",
                "Max Buff Stacks Per Extra Stack",
                WeaknessPolicy.maxBuffStacksPerExtraStack,
                "Max amount per extra stack of Weakness Policy buff stacks.",
                0, 100
            );
            WeaknessPolicy_Duration = BindOption(Config,
                "Weakness Policy",
                "Buff Duration",
                WeaknessPolicy.duration,
                "Duration of buff stacks before resetting.",
                0f, 600f
            );

            // QUICK CLAW CONFIG
            QuickClaw_ProcChance = BindOption(Config,
                "Quick Claw",
                "Initial Proc Chance",
                QuickClaw.procChance,
                "Initial chance to reduce cooldown after use of that move.",
                0f, 100f
            );
            QuickClaw_ProcChancePerExtraStack = BindOption(Config,
                "Quick Claw",
                "Proc Chance Per Extra Stack",
                QuickClaw.procChancePerExtraStack,
                "Chance per extra stack to reduce cooldown after use of that move.",
                0f, 100f
            );
            QuickClaw_CooldownReductionPercent = BindOption(Config,
                "Quick Claw",
                "Cooldown Reduction Percent",
                QuickClaw.cooldownReductionPercent,
                "Percent of cooldown reduction when a move is used.",
                0f, 100f
            );

            // BERRY JUICE CONFIG
            BerryJuice_RegenPercent = BindOption(Config,
                "Berry Juice",
                "Regen Percent",
                BerryJuice.regenPercent,
                "Max HP percent of health regen per buff stack.",
                0f, 100f
            );
            BerryJuice_ItemCap = BindOption(Config,
                "Berry Juice",
                "Initial Item Cap",
                BerryJuice.itemCap,
                "Initial unusable item capacity.",
                0, 100
            );
            BerryJuice_ItemCapPerExtraStack = BindOption(Config,
                "Berry Juice",
                "Item Cap Per Extra Stack",
                BerryJuice.itemCapPerExtraStack,
                "Unusable item capacity per extra stack.",
                0, 100
            );

            // LIFE ORB CONFIG
            LifeOrb_HpPercentSacrificePerStack = BindOption(Config,
                "Life Orb",
                "HP Percent Sacrifice Per Stack",
                LifeOrb.hpPercentSacrificePerStack,
                "HP (%) to sacrifice as extra damage per stack.",
                0f, 100f
            );
            LifeOrb_DamageOutputMultPercent = BindOption(Config,
                "Life Orb",
                "Initial Damage Bonus Amplification",
                LifeOrb.damageOutputMultPercent,
                "Initial bonus damage amplification (%).",
                0f, 1000f
            );
            LifeOrb_DamageOutputMultPercentPerExtraStack = BindOption(Config,
                "Life Orb",
                "Damage Bonus Amplification Per Extra Stack",
                LifeOrb.damageOutputMultPercent,
                "Bonus damage amplification (%) per extra stack.",
                0f, 1000f
            );
        }

        // Fetch float value from either cfg or default depending on the setting enabled
        public static float GetFloatValue(ConfigEntry<float> config, float defaultValue)
        {
            return CustomValuesEnabled.Value ? config.Value : defaultValue;
        }

        // Fetch int value from either cfg or default depending on the setting enabled
        public static int GetIntValue(ConfigEntry<int> config, int defaultValue)
        {
            return CustomValuesEnabled.Value ? config.Value : defaultValue;
        }
    }
}
