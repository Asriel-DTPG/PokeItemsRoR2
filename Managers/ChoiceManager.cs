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
        public static float cooldownPenaltyPerStack = 25f;

        private static readonly Dictionary<CharacterBody, GenericSkill> chosenSkills = new();

        public static void Init()
        {
            On.RoR2.GenericSkill.OnExecute += SkillUsed;
            On.RoR2.GenericSkill.CalculateFinalRechargeInterval += ModifyCooldown;
            On.RoR2.Stage.Start += StageStart;
            On.RoR2.CharacterMaster.OnBodyStart += BodyStart;
        }

        private static void SkillUsed(
            On.RoR2.GenericSkill.orig_OnExecute orig,
            GenericSkill self)
        {
            // Run vanilla logic
            orig(self);

            CharacterBody body = self.characterBody;

            if (body == null || body.inventory == null)
                return;

            if (GetTotalChoiceStacks(body) <= 0)
                return;

            if (ChoiceBuffs.HasChoiceLock(body))
                return;

            if (body.skillLocator.primary == self)
                body.AddBuff(ChoiceBuffs.ChoicePrimaryLock);
            else if (body.skillLocator.secondary == self)
                body.AddBuff(ChoiceBuffs.ChoiceSecondaryLock);
            else if (body.skillLocator.utility == self)
                body.AddBuff(ChoiceBuffs.ChoiceUtilityLock);
            else if (body.skillLocator.special == self)
                body.AddBuff(ChoiceBuffs.ChoiceSpecialLock);
        }

        private static float ModifyCooldown(
            On.RoR2.GenericSkill.orig_CalculateFinalRechargeInterval orig,
            GenericSkill self)
        {
            // Run vanilla logic
            float cooldown = orig(self);

            // Check if this has no meaningful recharge interval
            if (!self.skillDef.mustKeyPress && cooldown <= 0f)
                return cooldown;

            CharacterBody body = self.characterBody;

            if (body == null || body.inventory == null)
                return cooldown;

            if (!ChoiceBuffs.HasChoiceLock(body))
                return cooldown;

            if (IsChosenSkill(body, self))
                return cooldown;

            int stacks = GetTotalChoiceStacks(body);

            float multiplier = 1f;

            if (stacks <= 0)
                multiplier += cooldownPenaltyPerStack / 100f;

            multiplier += (MathUtility.GetLinearStacking(cooldownPenaltyPerStack, stacks) / 100f);

            return cooldown * multiplier;
        }

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
