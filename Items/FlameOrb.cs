using PokeItems.Managers;
using R2API;
using RoR2;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace PokeItems.Items
{
    internal class FlameOrb
    {
        public static ItemDef itemDef;

        // Item Settings
        private static ItemTier tier = ItemTier.Tier1; // Tier1 = WHITE; Tier2 = GREEN; Tier3 = RED; NoTier = UNUSABLE
        public static float procPercentPerStack = 10f; // Chance to proc
        public static float debuffDuration = 4f; // Duration of burn debuff
        public static float damageMul = 1f; // Damage multipler

        public static void Init()
        {
            // Create the itemDef via ItemManager
            itemDef = ItemManager.CreateItemDef("FlameOrb", tier, true, false,
                [ItemTag.Damage, ItemTag.CanBeTemporary],
                procPercentPerStack, debuffDuration);

            // Add the functionality
            On.RoR2.GlobalEventManager.OnHitEnemy += FlameOrbHook;
        }

        // Chance on inflicting burn to affected enemy
        private static void FlameOrbHook(
            On.RoR2.GlobalEventManager.orig_OnHitEnemy orig,
            GlobalEventManager self,
            DamageInfo damageInfo,
            GameObject victim)
        {
            // Run vanilla logic
            orig(self, damageInfo, victim);

            if (damageInfo.attacker == null || victim == null)
                return;

            CharacterBody attackerBody = damageInfo.attacker.GetComponent<CharacterBody>();
            CharacterBody victimBody = victim.GetComponent<CharacterBody>();

            if (attackerBody == null || victimBody == null || attackerBody.inventory == null)
                return;

            // Get the count of how many of this item is in the inventory
            int itemCount = attackerBody.inventory.GetItemCountEffective(itemDef);

            // Ignore this if this item is not in inventory
            if (itemCount <= 0)
                return;

            // Check if custom values are enabled
            if (ConfigManager.CustomValuesEnabled.Value)
            {
                procPercentPerStack = ConfigManager.FlameOrb_ProcPercentPerStack.Value;
                debuffDuration = ConfigManager.FlameOrb_DebuffDuration.Value;
            }

            // Check for proc chance
            float chance = MathUtility.GetLinearStacking(procPercentPerStack, itemCount, damageInfo.procCoefficient);

            if (Util.CheckRoll(chance, attackerBody.master))
            {
                // Initialise the burn effect for the affected enemy
                InflictDotInfo dotInfo = new InflictDotInfo
                {
                    victimObject = victim,
                    attackerObject = damageInfo.attacker,
                    dotIndex = DotController.DotIndex.Burn,
                    duration = debuffDuration,
                    damageMultiplier = damageMul
                };

                // Allows Ignition Tank to upgrade this item effect (Burn -> StrongerBurn)
                StrengthenBurnUtils.CheckDotForUpgrade(attackerBody.inventory, ref dotInfo);

                // Inflict effect
                DotController.InflictDot(ref dotInfo);
            }
        }
    }
}
