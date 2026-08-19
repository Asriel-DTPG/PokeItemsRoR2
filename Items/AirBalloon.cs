using PokeItems.Managers;
using R2API;
using RoR2;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace PokeItems.Items
{
    internal class AirBalloon
    {
        public static ItemDef itemDef;

        // Item Settings
        private static ItemTier tier = ItemTier.Tier1; // 1 = WHITE; 2 = GREEN; 3 = RED
        public static float fallSpeedLimit = 50f; // Falling Speed Cap
        public static float fallPercentReductionPerExtraStack = 10f; // Cap Reduction per extra stack to reduce falling speed even further
        public static float hpThresholdPercent = 35f;

        public static void Init()
        {
            // Create the itemDef via ItemManager
            itemDef = ItemManager.CreateItemDef("AirBalloon", tier, true, false,
                [ItemTag.Utility, ItemTag.CanBeTemporary],
                fallSpeedLimit, fallPercentReductionPerExtraStack, hpThresholdPercent);

            // Add the functionality
            On.RoR2.HealthComponent.TakeDamage += AirBalloonDamagedHook;
            On.RoR2.CharacterMotor.FixedUpdate += AirBalloonHook;
        }

        // Check for HP threshold when taking damage, and if under, pop the balloons
        private static void AirBalloonDamagedHook(On.RoR2.HealthComponent.orig_TakeDamage orig, HealthComponent self, DamageInfo damageInfo)
        {
            orig(self, damageInfo);

            CharacterBody body = self.body;

            // Mandatory check for player body with inventory
            if (body == null || body.inventory == null || body.healthComponent == null)
                return;

            // Get the count of how many of this item is in the inventory
            int itemCount = body.inventory.GetItemCountEffective(itemDef);

            if (itemCount <= 0)
                return;

            // Check if custom values are enabled
            float hpThreshold = ConfigManager.GetFloatValue(
                ConfigManager.AirBalloon_HpThresholdPercent,
                hpThresholdPercent);

            // Grab the HP fraction
            float hpFraction = self.combinedHealth / self.fullCombinedHealth;

            // Check against the HP threshold of the item
            if (hpFraction <= (hpThreshold / 100f))
            {
                // Pop the balloons
                PopAllBalloons(body.inventory);
            }
        }

        // Pop all active Air Balloons into unusable state
        private static void PopAllBalloons(Inventory inventory)
        {
            // Get the count of how many of this item is in the inventory
            int itemCount = inventory.GetItemCountEffective(itemDef);

            if (itemCount <= 0)
                return;

            // Replace all Air Balloons with Popped Air Balloons
            inventory.RemoveItem(itemDef, itemCount);
            inventory.GiveItem(AirBalloonBroken.itemDef, itemCount);

            // Begin setting up the pickup notification for the transformation
            CharacterMaster master = inventory.GetComponent<CharacterMaster>();
            if (master)
            {
                CharacterBody body = master.GetBody();

                if (body && body.isPlayerControlled)
                {
                    CharacterMasterNotificationQueue.PushItemTransformNotification(
                        master,
                        itemDef.itemIndex,
                        AirBalloonBroken.itemDef.itemIndex,
                        CharacterMasterNotificationQueue.TransformationType.Default);
                }
            }
        }

        // Enable falling speed cap
        private static void AirBalloonHook(On.RoR2.CharacterMotor.orig_FixedUpdate orig, CharacterMotor self)
        {
            orig(self);

            CharacterBody body = self.body;

            // Mandatory checks
            if (body == null || body.inventory == null)
                return;

            // Get the count of how many of this item is in the inventory
            int itemCount = body.inventory.GetItemCountEffective(itemDef);

            if (itemCount <= 0)
                return;

            // Affect falling speed with a set cap
            if (self.velocity.y < 0f)
            {
                // Check if custom values are enabled
                float fallLimit = ConfigManager.GetFloatValue(
                    ConfigManager.AirBalloon_FallSpeedLimit,
                    fallSpeedLimit);
                float fallLimitReduction = ConfigManager.GetFloatValue(
                    ConfigManager.AirBalloon_FallPercentReductionPerExtraStack,
                    fallPercentReductionPerExtraStack);

                // Grab the percentage reduction from extra stacks
                float reductionMultiplier = MathUtility.GetExponentialPercentReductionStacking(fallLimitReduction, itemCount - 1);
                
                // Get the total terminalvelocity
                float terminalVelocity = -fallLimit * reductionMultiplier;

                // Check if velocity goes beyond terminal velocity, and if so, set it to terminal velocity to prevent it from going any further
                if (self.velocity.y < terminalVelocity)
                {
                    Vector3 velocity = self.velocity;
                    velocity.y = terminalVelocity;
                    self.velocity = velocity;
                }
            }
        }
    }
}
