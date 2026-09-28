using PokeItems.Managers;
using System.Collections.Generic;
using RoR2;
using UnityEngine;
using R2API;
using UnityEngine.Networking;

namespace PokeItems.Items
{
    internal class WeaknessPolicy
    {
        public static ItemDef itemDef;
        public static BuffDef weaknessPrivilege;
        private static readonly Dictionary<CharacterBody, float> resetTimers = new();

        // Item Settings
        private static ItemTier tier = ItemTier.Tier1; // 1 = WHITE; 2 = GREEN; 3 = RED
        public static float damageBonus = 10f; // Damage bonus per stack
        public static int maxBuffStacks = 5; // Initial max stacks of Weakness Policy
        public static int maxBuffStacksPerExtraStack = 3; // Max stacks of Weakness Policy per extra item stack
        public static float duration = 5f; // Duration of buff stacks

        public static void Init()
        {
            // Create the itemDef via ItemManager
            itemDef = ItemManager.CreateItemDef("WeaknessPolicy", tier, true, false,
                [ItemTag.Damage, ItemTag.CanBeTemporary],
                damageBonus, maxBuffStacks, maxBuffStacksPerExtraStack, duration);

            // Create the buffDef via BuffManager
            weaknessPrivilege = BuffManager.CreateBuffDef("WeaknessPrivilege", Color.white, true, false, false);

            // Add functionality
            RecalculateStatsAPI.GetStatCoefficients += WeaknessPolicyDamageHook;
            On.RoR2.HealthComponent.TakeDamage += WeaknessPolicyDamagedHook;
            On.RoR2.CharacterBody.FixedUpdate += WeaknessPolicyFixedUpdateHook;
            On.RoR2.CharacterBody.OnDestroy += WeaknessPolicyBodyDestroyHook;
        }

        // Damage bonus applied by Weakness Policy buffs
        private static void WeaknessPolicyDamageHook(
            CharacterBody body,
            RecalculateStatsAPI.StatHookEventArgs args)
        {
            // Mandatory checks
            if (body == null || !body.inventory)
                return;

            // Get current Weakness Policy buff stacks
            int stacks = body.GetBuffCount(weaknessPrivilege);

            if (stacks <= 0)
                return;

            // Calculate total bonus
            float totalBonus = damageBonus * stacks;

            // Convert percentage into multiplier
            args.damageMultAdd += totalBonus / 100f;
        }

        // Adding buffs based on successful damage taken
        private static void WeaknessPolicyDamagedHook(
            On.RoR2.HealthComponent.orig_TakeDamage orig,
            HealthComponent self,
            DamageInfo damageInfo)
        {
            orig(self, damageInfo);

            // Network server dependant
            if (!NetworkServer.active)
                return;

            if (self == null)
                return;

            CharacterBody body = self.body;

            // Mandatory checks
            if (body == null || body.inventory == null)
                return;

            // Check if character has Weakness Policy item
            int itemCount = body.inventory.GetItemCountEffective(WeaknessPolicy.itemDef);

            if (itemCount <= 0)
                return;

            // Ignore damage that isn't meaningful
            if (damageInfo.damage <= 0f)
                return;

            // Ignore attacks with no proc coefficient
            if (damageInfo.procCoefficient <= 0f)
                return;

            // Ignore damage if character is already dead
            if (!body.healthComponent.alive)
                return;

            // Get current stack count
            int currentStacks = body.GetBuffCount(weaknessPrivilege);

            // Get max stack count
            int maxTotalStacks = (int)MathUtility.GetLinearWithExtraStacking(
                WeaknessPolicy.maxBuffStacks, WeaknessPolicy.maxBuffStacksPerExtraStack, itemCount);

            // Add stack if not at max
            if (currentStacks < maxTotalStacks)
            {
                body.AddBuff(weaknessPrivilege);
            }

            // Refresh the timer (can still be done regardless of max)
            resetTimers[body] = duration;
        }

        // Counting down reset timer and removing buffs if needed
        private static void WeaknessPolicyFixedUpdateHook(
            On.RoR2.CharacterBody.orig_FixedUpdate orig,
            CharacterBody self)
        {
            orig(self);

            if (!NetworkServer.active)
                return;

            if (self == null)
                return;

            if (!resetTimers.TryGetValue(self, out float timer))
                return;

            // Countdown using fixed timerstep
            timer -= Time.fixedDeltaTime;

            // When timer expires, remove all weakness policy buffs and then its reset timer
            if (timer <= 0f)
            {
                resetTimers.Remove(self);

                // Remove all Weakness Policy stacks
                self.SetBuffCount(weaknessPrivilege.buffIndex, 0);
                return;
            }

            // Store updated timer
            resetTimers[self] = timer;
        }

        // Remove timer when body is destroyed
        private static void WeaknessPolicyBodyDestroyHook(
            On.RoR2.CharacterBody.orig_OnDestroy orig,
            CharacterBody self)
        {
            if (self != null)
                resetTimers.Remove(self);

            orig(self);
        }
    }
}
