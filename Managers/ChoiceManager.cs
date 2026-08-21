using PokeItems.Buffs;
using System;
using System.Collections.Generic;
using System.Text;
using RoR2;
using UnityEngine;
using System.Collections;
using PokeItems.Items;

namespace PokeItems.Managers
{
    internal static class ChoiceManager
    {
        public static float cooldownPenaltyPerStack = 20f;
        public static float cooldownPenaltyPercentLimit = 1f;

        private static readonly Dictionary<CharacterBody, GenericSkill> chosenSkills = new();

        public static void Init()
        {
            // Add functionality
            On.RoR2.GenericSkill.OnExecute += SkillUsed;
            On.RoR2.GenericSkill.RunRecharge += ModifyCooldown;
            On.RoR2.Stage.Start += StageStart;
            On.RoR2.CharacterMaster.OnBodyStart += BodyStart;
        }

        // Record the chosen skill into a choice lock if it doesn't exist
        private static void SkillUsed(
            On.RoR2.GenericSkill.orig_OnExecute orig,
            GenericSkill self)
        {
            // Run vanilla logic
            orig(self);

            CharacterBody body = self.characterBody;

            // Mandatory check
            if (body == null || body.inventory == null)
                return;

            // Get the count of how many of choice items is in the inventory
            if (GetTotalChoiceStacks(body) <= 0)
                return;

            // Proceed if they do not have a choice lock
            if (ChoiceBuffs.HasChoiceLock(body))
                return;

            // Add choice lock based on chosen skill
            if (body.skillLocator.primary == self)
                body.AddBuff(ChoiceBuffs.ChoicePrimaryLock);
            else if (body.skillLocator.secondary == self)
                body.AddBuff(ChoiceBuffs.ChoiceSecondaryLock);
            else if (body.skillLocator.utility == self)
                body.AddBuff(ChoiceBuffs.ChoiceUtilityLock);
            else if (body.skillLocator.special == self)
                body.AddBuff(ChoiceBuffs.ChoiceSpecialLock);
        }

        // Modify the recharge time based on non-chosen skill and number of choice items
        private static void ModifyCooldown(
            On.RoR2.GenericSkill.orig_RunRecharge orig,
            GenericSkill self,
            float rechargeTime)
        {
            // Check if this skill has no meaningful recharge
            if (rechargeTime <= 0f)
            {
                orig(self, rechargeTime);
                return;
            }

            CharacterBody body = self.characterBody;

            // Mandatory check
            if (body == null || body.inventory == null)
            {
                orig(self, rechargeTime);
                return;
            }

            // Get total Choice item stacks
            int choiceStacks = GetTotalChoiceStacks(body);

            if (!ChoiceBuffs.HasChoiceLock(body) || IsChosenSkill(body, self))
            {
                orig(self, rechargeTime);
                return;
            }

            // Check if custom values are enabled
            float cooldownPenalty = ConfigManager.GetFloatValue(
                ConfigManager.ChoiceItems_CooldownPenaltyPerStack,
                cooldownPenaltyPerStack);

            // Calculate percentage reduction exponentially (Choice locks themselves can still cause effects without choice items)
            float multiplier = MathUtility.GetExponentialPercentReductionStacking(cooldownPenalty, Math.Max(1, choiceStacks));

            // Set limit to multiplier
            multiplier = Mathf.Max(multiplier, cooldownPenaltyPercentLimit / 100f);

            // Set new recharge time
            float newRechargeTime = rechargeTime * multiplier;

            // Continue vanilla logic
            orig(self, newRechargeTime);
        }

        // Reset choice lock on new stage
        private static IEnumerator StageStart(
            On.RoR2.Stage.orig_Start orig,
            Stage self)
        {
            // Run vanilla logic
            yield return orig(self);

            foreach (CharacterMaster master in CharacterMaster.readOnlyInstancesList)
            {
                CharacterBody body = master.GetBody();

                if (body != null)
                    ChoiceBuffs.RemoveAllChoiceLocks(body);
            }
        }

        // Reset choice lock on respawn
        private static void BodyStart(
            On.RoR2.CharacterMaster.orig_OnBodyStart orig,
            CharacterMaster self,
            CharacterBody body)
        {
            // Run vanilla logic
            orig(self, body);

            if (body != null)
                ChoiceBuffs.RemoveAllChoiceLocks(body);
        }

        // Check if this skill has a choice lock associated with it
        private static bool IsChosenSkill(
            CharacterBody body,
            GenericSkill skill)
        {
            if (body.skillLocator.primary == skill)
                return body.HasBuff(ChoiceBuffs.ChoicePrimaryLock);

            if (body.skillLocator.secondary == skill)
                return body.HasBuff(ChoiceBuffs.ChoiceSecondaryLock);

            if (body.skillLocator.utility == skill)
                return body.HasBuff(ChoiceBuffs.ChoiceUtilityLock);

            if (body.skillLocator.special == skill)
                return body.HasBuff(ChoiceBuffs.ChoiceSpecialLock);

            return false;
        }

        // Get the total number of choice item stacks
        public static int GetTotalChoiceStacks(CharacterBody body)
        {
            Inventory inventory = body.inventory;

            if (inventory == null)
                return 0;

            return
                inventory.GetItemCountEffective(ChoiceBand.itemDef) +
                inventory.GetItemCountEffective(ChoiceSpecs.itemDef) +
                inventory.GetItemCountEffective(ChoiceScarf.itemDef);
        }
    }
}
