using PokeItems.Managers;
using R2API;
using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;

namespace PokeItems.Items
{
    internal class LifeOrb
    {
        public static ItemDef itemDef;

        // Item Settings
        private static ItemTier tier = ItemTier.Lunar; // 1 = WHITE; 2 = GREEN; 3 = RED
        public static float hpPercentSacrificePerStack = 2f; // HP Sacrifice per stack (Percentage)

        public static void Init()
        {
            // Create the itemDef via ItemManager
            itemDef = ItemManager.CreateItemDef("LifeOrb", tier, true, false,
                [ItemTag.Damage, ItemTag.CanBeTemporary],
                hpPercentSacrificePerStack);

            // Add the functionality
            On.RoR2.HealthComponent.TakeDamage += LifeOrbDamageHook;
        }

        private static void LifeOrbDamageHook(
            On.RoR2.HealthComponent.orig_TakeDamage orig,
            HealthComponent self,
            DamageInfo damageInfo)
        {
            // Life Orb functionality is considerably server-side.
            if (!NetworkServer.active)
            {
                orig(self, damageInfo);
                return;
            }

            // Environmental/indirect damage has no attacker
            if (damageInfo.attacker == null)
            {
                orig(self, damageInfo);
                return;
            }

            // Zero proc coefficient should be ignored
            if (damageInfo.procCoefficient <= 0f)
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

            // Ignore if attacker hits itself
            if (self.body == attackerBody)
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

            // Calculate total sacrifice decimal
            float sacrificeFraction = MathUtility.GetLinearStacking(hpPercentSacrificePerStack, itemCount, damageInfo.procCoefficient) / 100f;

            // Determine amount of HP sacrificed
            float sacrificeAmount = attackerBody.maxHealth * sacrificeFraction;

            // Do not deal invalid damage
            if (sacrificeAmount <= 0f)
            {
                orig(self, damageInfo);
                return;
            }

            // Prevent Life Orb from killing the attacker
            sacrificeAmount = Mathf.Min(sacrificeAmount, Mathf.Max(0f, attackerBody.healthComponent.combinedHealth - 1f));

            if (sacrificeAmount <= 0f)
            {
                orig(self, damageInfo);
                return;
            }

            // Add sacrificed HP directly to attack trigger
            damageInfo.damage += sacrificeAmount;

            // Apply Life Orb's HP sacrifice (Don't kill the attacker and instead cap at 1 HP)
            attackerBody.healthComponent.health = Mathf.Max(1f, attackerBody.healthComponent.health - sacrificeAmount);

            // Continue vanilla process
            orig(self, damageInfo);
        }
    }
}
