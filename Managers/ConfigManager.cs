using BepInEx;
using BepInEx.Configuration;
using System.IO;
using PokeItems.Items;

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

        // EXP SHARE
        public static ConfigEntry<float> ExpShare_ExpPercentBonusPerStack;
        public static ConfigEntry<float> ExpShare_ExpPercentBonusPerExtraStack;
        public static ConfigEntry<float> ExpShare_ExpRateBonus;

        // AMULET COIN
        public static ConfigEntry<float> AmuletCoin_GoldMulPercentPerStack;

        // HEAVY-DUTY BOOTS
        public static ConfigEntry<float> HeavyDutyBoots_ArmorBonusPerStack;
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
                Leftovers.regenBonusPerStack,
                "Health regeneration applied from initial stack."
            );
            Leftovers_RegenBonusPerExtraStack = Config.Bind(
                "Leftovers",
                "Regeneration Bonus Per Extra Stack",
                Leftovers.regenBonusPerExtraStack,
                "Health regeneration applied per extra stack."
            );

            // FLAME ORB CONFIG
            FlameOrb_ProcPercentPerStack = Config.Bind(
                "Flame Orb",
                "Burn Chance Percent Per Stack",
                FlameOrb.procPercentPerStack,
                "Chance to inflict burn per stack."
            );
            FlameOrb_DebuffDuration = Config.Bind(
                "Flame Orb",
                "Burn Duration",
                FlameOrb.debuffDuration,
                "How long the burn debuff lasts."
            );

            // AIR BALLOON CONFIG
            AirBalloon_FallSpeedLimit = Config.Bind(
                "Air Balloon",
                "Fall Speed Limit",
                AirBalloon.fallSpeedLimit,
                "Limit applied to falling speed (m/s)."
            );
            AirBalloon_FallPercentReductionPerExtraStack = Config.Bind(
                "Air Balloon",
                "Fall Percent Reduction Per Extra Stack",
                AirBalloon.fallPercentReductionPerExtraStack,
                "Reduction percent over falling speed limit exponentially applied."
            );
            AirBalloon_HpThresholdPercent = Config.Bind(
                "Air Balloon",
                "HP Threshold Percent",
                AirBalloon.hpThresholdPercent,
                "How low the HP can deplete before the balloon pops."
            );

            // EXP SHARE CONFIG
            ExpShare_ExpPercentBonusPerStack = Config.Bind(
                "EXP Share",
                "Initial EXP Percent Bonus",
                ExpShare.expPercentBonusPerStack,
                "Extra percent of EXP is applied from initial stack."
            );
            ExpShare_ExpPercentBonusPerExtraStack = Config.Bind(
                "EXP Share",
                "EXP Percent Bonus Per Extra Stack",
                ExpShare.expPercentBonusPerExtraStack,
                "Extra percent of EXP is applied per extra stack."
            );
            ExpShare_ExpRateBonus = Config.Bind(
                "EXP Share",
                "Required EXP Percent Rate",
                ExpShare.expRateBonus,
                "Percent amount of required EXP gained per minute."
            );

            // AMULET COIN CONFIG
            AmuletCoin_GoldMulPercentPerStack = Config.Bind(
                "Amulet Coin",
                "Gold Percent Bonus",
                AmuletCoin.goldMulPercentPerStack,
                "Extra percent of gold is applied per stack."
            );

            // HEAVY-DUTY BOOTS CONFIG
            HeavyDutyBoots_ArmorBonusPerStack = Config.Bind(
                "Heavy-Duty Boots",
                "Initial Armor Bonus",
                HeavyDutyBoots.armorBonusPerStack,
                "Extra percent of EXP is applied from initial stack."
            );
            HeavyDutyBoots_ArmorBonusPerExtraStack = Config.Bind(
                "Heavy-Duty Boots",
                "Armor Bonus Per Extra Stack",
                HeavyDutyBoots.armorBonusPerExtraStack,
                "How long the burn debuff lasts."
            );

            // CHOICE ITEMS CONFIG
            ChoiceItems_CooldownPenaltyPerStack = Config.Bind(
                "Choice Items",
                "Cooldown Penalty Per Stack",
                ChoiceManager.cooldownPenaltyPerStack,
                "How slow the non-preferred skill cooldown recharges."
            );

            // CHOICE BAND CONFIG
            ChoiceBand_DamageBonus = Config.Bind(
                "Choice Band",
                "Initial Damage Bonus",
                ChoiceBand.damageBonus,
                "Initial damage bonus percent for Primary and Secondary skills."
            );
            ChoiceBand_DamageBonusPerExtraStack = Config.Bind(
                "Choice Band",
                "Damage Bonus Per Extra Stack",
                ChoiceBand.damageBonusPerExtraStack,
                "Damage bonus percent for Primary and Secondary skills per extra stack."
            );

            // CHOICE SPECS CONFIG
            ChoiceSpecs_DamageBonus = Config.Bind(
                "Choice Specs",
                "Initial Damage Bonus",
                ChoiceSpecs.damageBonus,
                "Initial damage bonus percent for Utility and Special skills."
            );
            ChoiceSpecs_DamageBonusPerExtraStack = Config.Bind(
                "Choice Specs",
                "Damage Bonus Per Extra Stack",
                ChoiceSpecs.damageBonusPerExtraStack,
                "Damage bonus percent for Utility and Special skills per extra stack."
            );

            // CHOICE SCARF CONFIG
            ChoiceScarf_MovementBonus = Config.Bind(
                "Choice Scarf",
                "Initial Movement Bonus",
                ChoiceScarf.movementBonus,
                "Initial movement bonus percent."
            );
            ChoiceScarf_MovementBonusPerExtraStack = Config.Bind(
                "Choice Scarf",
                "Movement Bonus Per Extra Stack",
                ChoiceScarf.movementBonusPerExtraStack,
                "Movement bonus percent per extra stack."
            );
        }

        // Fetch float value from either cfg or default depending on the setting enabled
        public static float GetFloatValue(ConfigEntry<float> config, float defaultValue)
        {
            if (CustomValuesEnabled.Value)
                return config.Value;

            return defaultValue;
        }
    }
}
