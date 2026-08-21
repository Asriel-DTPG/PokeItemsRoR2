using PokeItems.Managers;
using R2API;
using RoR2;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace PokeItems.Items
{
    internal class ChoiceBand
    {
        public static ItemDef itemDef;

        // Item Settings
        private static ItemTier tier = ItemTier.Lunar; // 1 = WHITE; 2 = GREEN; 3 = RED
        public static float damageBonus = 100f; // Initial damage bonus for Primary and Secondary
        public static float damageBonusPerExtraStack = 50f; // Extra damage bonus per stack for Primary and Secondary

        public static void Init()
        {
            // Create the itemDef via ItemManager
            itemDef = ItemManager.CreateItemDef("ChoiceBand", tier, true, false,
                [ItemTag.Damage, ItemTag.CanBeTemporary],
                damageBonus, damageBonusPerExtraStack, ChoiceManager.cooldownPenaltyPerStack);

            // Add the functionality
            On.RoR2.HealthComponent.TakeDamage += ChoiceBandDamageHook;
        }

        // Check skill if it's Primary and Secondary, and if so, multiply the damage
        private static void ChoiceBandDamageHook(
            On.RoR2.HealthComponent.orig_TakeDamage orig,
            HealthComponent self,
            DamageInfo damageInfo)
        {
            // Environmental/indirect damage has no attacker
            if (damageInfo.attacker == null)
            {
                orig(self, damageInfo);
                return;
            }

            // Mandatory check for attacker body with inventory
            CharacterBody attackerBody = damageInfo.attacker.GetComponent<CharacterBody>();

            if (attackerBody == null || attackerBody.inventory == null)
            {
                orig(self, damageInfo);
                return;
            }

            // Get the count of how many of this item is in the inventory
            int itemCount = attackerBody.inventory.GetItemCountEffective(itemDef);

            if (itemCount <= 0)
            {
                orig(self, damageInfo);
                return;
            }

            DamageSource damageSource = damageInfo.damageType.damageSource;

            // Check if either skill is Primary or Secondary
            bool isPrimary = (damageSource & DamageSource.Primary) != 0;
            bool isSecondary = (damageSource & DamageSource.Secondary) != 0;

            if (!isPrimary && !isSecondary)
            {
                orig(self, damageInfo);
                return;
            }

            // Check if custom values are enabled
            float damBonus = ConfigManager.GetFloatValue(
                ConfigManager.ChoiceBand_DamageBonus,
                damageBonus);
            float damExtraBonus = ConfigManager.GetFloatValue(
                ConfigManager.ChoiceBand_DamageBonusPerExtraStack,
                damageBonusPerExtraStack);

            // Get bonus percentage to then multiply damage
            float bonusPercent = MathUtility.GetLinearWithExtraStacking(damBonus, damExtraBonus, itemCount);
            float multiplier = 1f + bonusPercent / 100f;
            damageInfo.damage *= multiplier;

            // Continue vanilla logic
            orig(self, damageInfo);
        }
    }
}
