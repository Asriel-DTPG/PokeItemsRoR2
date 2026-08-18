using PokeItems.Managers;
using R2API;
using RoR2;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace PokeItems.Items
{
    internal class ChoiceSpecs
    {
        public static ItemDef itemDef;

        // Item Settings
        private static ItemTier tier = ItemTier.Lunar; // 1 = WHITE; 2 = GREEN; 3 = RED
        public static float damageBonus = 100f; // Initial damage bonus for Utility and Special
        public static float damageBonusPerExtraStack = 50f; // Extra damage bonus per stack for Utility and Special

        public static void Init()
        {
            // Create the itemDef via ItemManager
            itemDef = ItemManager.CreateItemDef("ChoiceSpecs", tier, true, false,
                [ItemTag.Damage, ItemTag.CanBeTemporary],
                damageBonus, damageBonusPerExtraStack, ChoiceManager.cooldownPenaltyPerStack);

            // Add the functionality
            On.RoR2.HealthComponent.TakeDamage += ChoiceSpecsDamageHook;
        }

        // Check skill if it's Primary and Secondary, and if so, multiply the damage
        private static void ChoiceSpecsDamageHook(
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

            if (attackerBody != null || attackerBody.inventory == null)
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

            // Check if either skill is Utility or Special
            bool isUtility = (damageSource & DamageSource.Utility) != 0;
            bool isSpecial = (damageSource & DamageSource.Special) != 0;

            if (!isUtility && !isSpecial)
            {
                orig(self, damageInfo);
                return;
            }

            // Get bonus percentage to then multiply damage
            float bonusPercent = MathUtility.GetLinearWithExtraStacking(damageBonus, damageBonusPerExtraStack, itemCount);
            float multiplier = 1f + bonusPercent / 100f;
            damageInfo.damage *= multiplier;

            // Continue vanilla logic
            orig(self, damageInfo);
        }
    }
}
