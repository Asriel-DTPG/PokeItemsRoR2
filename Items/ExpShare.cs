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
        private static readonly Dictionary<CharacterMaster, float>  prevRunTimesDict = new();
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
                    float expBonus = ConfigManager.GetFloatValue(
                        ConfigManager.ExpShare_ExpPercentBonusPerStack,
                        expPercentBonusPerStack);
                    float expExtraBonus = ConfigManager.GetFloatValue(
                        ConfigManager.ExpShare_ExpPercentBonusPerExtraStack,
                        expPercentBonusPerExtraStack);

                    // Set multiplier of the EXP gain
                    float multiplier = 1f + (MathUtility.GetLinearWithExtraStacking(expBonus, expExtraBonus, itemCount)) / 100f;

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

            // Check if EXP Share exists in inventory
            bool hasEXPShare = itemCount > 0;

            Run run = Run.instance;

            // Check if this is a viable run
            if (run == null)
                return;

            // Check if the stage is a purely timed environment
            SceneDef sceneDef = SceneCatalog.GetSceneDefForCurrentScene();
            bool isTimedStage = sceneDef != null && sceneDef.sceneType == SceneType.Stage;

            CharacterMaster master = self.master;

            float currentRunTime = run.fixedTime;

            // If player does not have EXP Share or is in an untimed stage, reset timer (if dict is empty, reset now)
            if (!prevRunTimesDict.ContainsKey(master) || (!isTimedStage || !hasEXPShare))
            {
                ResetTimer(master, currentRunTime);
                return;
            }

            float previousRunTime = prevRunTimesDict[master];
            
            // Calculate how much run time has passed
            float deltaTime = currentRunTime - previousRunTime;

            // Update stored run time
            prevRunTimesDict[master] = currentRunTime;

            // Check if run time has not progressed
            if (deltaTime <= 0f)
                return;
            
            // Increment timer
            timersDict[master] += deltaTime;

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
            float expRate = ConfigManager.GetFloatValue(
                ConfigManager.ExpShare_ExpRateBonus,
                expRateBonus);

            // Get amount of EXP required
            uint level = (uint)Mathf.FloorToInt(body.level);
            ulong currentXP = TeamManager.GetExperienceForLevel(level);
            ulong nextXP = TeamManager.GetExperienceForLevel(level + 1);
            ulong requiredEXP = nextXP - currentXP;

            // Multiply required EXP into bonus percent (convert amount into per second)
            ulong experience = (ulong)(requiredEXP * (expRate / 100f / 60f));

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

        // Initialize or reset tracking for prev run time and personal timer
        private static void ResetTimer(CharacterMaster master, float currTime)
        {
            prevRunTimesDict[master] = currTime;
            timersDict[master] = 0f;
        }

        // Destroy reference from dictionary if it no longer exists
        private static void DictionaryDestroy(
            On.RoR2.CharacterMaster.orig_OnDestroy orig,
            CharacterMaster self)
        {
            timersDict.Remove(self);
            prevRunTimesDict.Remove(self);
            orig(self);
        }
    }
}
