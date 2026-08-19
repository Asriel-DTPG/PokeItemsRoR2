using PokeItems.Managers;
using R2API;
using RoR2;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;

namespace PokeItems.Items
{
    internal class ExpShare
    {
        public static ItemDef itemDef;

        // Item Settings
        private static ItemTier tier = ItemTier.Tier3; // 1 = WHITE; 2 = GREEN; 3 = RED
        public static float expPercentBonusPerStack = 200f; // EXP Bonus Percent
        public static float expPercentBonusPerExtraStack = 100f; // EXP Bonus Percent per extra stack
        public static float expRateBonus = 30f; // Passive EXP percent per minute (scaled by requirement)

        private static readonly Dictionary<CharacterMaster, float> timersDict = new();
        private static bool isPassiveEXP = false;

        public static void Init()
        {
            // Create the itemDef via ItemManager
            itemDef = ItemManager.CreateItemDef("ExpShare", tier, true, false,
                [ItemTag.Utility, ItemTag.CanBeTemporary],
                expPercentBonusPerStack, expPercentBonusPerExtraStack, expRateBonus);

            // Add the functionality
            On.RoR2.ExperienceManager.AwardExperience += ExpShareHook;
            On.RoR2.CharacterBody.FixedUpdate += ExpShareRateHook;
            On.RoR2.CharacterMaster.OnDestroy += DictionaryDestroy;
        }

        // Apply EXP bonus
        private static void ExpShareHook(
            On.RoR2.ExperienceManager.orig_AwardExperience orig,
            ExperienceManager self,
            Vector3 origin,
            CharacterBody body,
            ulong experience)
        {
            // Mandatory check
            if (body != null && body.inventory != null && !isPassiveEXP)
            {
                // Get the count of how many of this item is in the inventory
                int itemCount = body.inventory.GetItemCountEffective(itemDef);

                // Ignore this if this item is not in inventory
                if (itemCount > 0)
                {
                    // Check if custom values are enabled
                    if (ConfigManager.CustomValuesEnabled.Value)
                    {
                        expPercentBonusPerStack = ConfigManager.ExpShare_ExpPercentBonusPerStack.Value;
                        expPercentBonusPerExtraStack = ConfigManager.ExpShare_ExpPercentBonusPerExtraStack.Value;
                    }

                    // Set multiplier of the EXP gain
                    float multiplier = 1f + (MathUtility.GetLinearWithExtraStacking(expPercentBonusPerStack, expPercentBonusPerExtraStack, itemCount)) / 100f;

                    // Multiply the EXP gain
                    experience = (ulong)(experience * multiplier);
                }
            }

            orig(self, origin, body, experience);
        }

        // Apply EXP passive gain
        private static void ExpShareRateHook(
            On.RoR2.CharacterBody.orig_FixedUpdate orig,
            CharacterBody self)
        {
            orig(self);

            // Handled on server
            if (!NetworkServer.active)
                return;

            // Mandatory check
            if (self == null || self.inventory == null || self.master == null)
                return;

            // Get the count of how many of this item is in the inventory
            int itemCount = self.inventory.GetItemCountEffective(itemDef);

            // Ignore this if this item is not in inventory
            if (itemCount <= 0)
                return;

            CharacterMaster master = self.master;

            // Save personal timer to dictionary list
            if (!timersDict.ContainsKey(master))
                timersDict[master] = 0f;

            // Increment timer
            timersDict[master] += Time.fixedDeltaTime;

            // Check if timer has reached a second. If so, decrement by a second and initiate passive gain
            if (timersDict[master] >= 1f)
            {
                timersDict[master] -= 1f;
                GainPassiveEXP(self, itemCount);
            }
        }

        // Helper function to handle passive EXP gain
        private static void GainPassiveEXP(CharacterBody body, int itemCount)
        {
            // Handled on server
            if (!NetworkServer.active)
                return;

            // Check if custom values are enabled
            if (ConfigManager.CustomValuesEnabled.Value)
            {
                expRateBonus = ConfigManager.ExpShare_ExpRateBonus.Value;
            }

            // Get amount of EXP required
            uint level = (uint)Mathf.FloorToInt(body.level);
            ulong currentXP = TeamManager.GetExperienceForLevel(level);
            ulong nextXP = TeamManager.GetExperienceForLevel(level + 1);
            ulong requiredEXP = nextXP - currentXP;

            // Multiply required EXP into bonus percent (convert amount into per second)
            ulong experience = (ulong)(requiredEXP * (expRateBonus / 100f / 60f));

            // If experience is too low, compensate for ceiling round
            if (experience <= 0)
                experience = 1;

            // Award EXP via the ExperienceManager. Ignore bonus gain
            isPassiveEXP = true;

            ExperienceManager.instance.AwardExperience(
                body.corePosition,
                body,
                experience);

            isPassiveEXP = false;
        }

        // Destroy reference from dictionary if it no longer exists
        private static void DictionaryDestroy(
            On.RoR2.CharacterMaster.orig_OnDestroy orig,
            CharacterMaster self)
        {
            timersDict.Remove(self);
            orig(self);
        }
    }
}
