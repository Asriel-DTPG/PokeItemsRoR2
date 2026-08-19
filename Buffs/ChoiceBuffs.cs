using PokeItems.Managers;
using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace PokeItems.Buffs
{
    internal static class ChoiceBuffs
    {
        public static readonly Dictionary<string, object[]> tokenMap = new();

        public static BuffDef ChoicePrimaryLock;
        public static BuffDef ChoiceSecondaryLock;
        public static BuffDef ChoiceUtilityLock;
        public static BuffDef ChoiceSpecialLock;

        public static BuffDef[] ChoiceLocks;

        public static void Init()
        {
            ChoicePrimaryLock = CreateChoiceLock(
                "PrimaryLock",
                TryGetSprite("PrimaryLock")
            );
            ChoiceSecondaryLock = CreateChoiceLock(
                "SecondaryLock",
                TryGetSprite("SecondaryLock")
            );
            ChoiceUtilityLock = CreateChoiceLock(
                "UtilityLock",
                TryGetSprite("UtilityLock")
            );
            ChoiceSpecialLock = CreateChoiceLock(
                "SpecialLock",
                TryGetSprite("SpecialLock")
            );

            ChoiceLocks =
            [
                ChoicePrimaryLock,
                ChoiceSecondaryLock,
                ChoiceUtilityLock,
                ChoiceSpecialLock
            ];
        }

        private static Sprite TryGetSprite(string spriteName)
        {
            // Grab item sprite from assetbundle (replace with mystery if it doesn't exist)
            Sprite sprite = AssetManager.bundle.LoadAsset<Sprite>(spriteName + ".png");
            if (sprite == null)
            {
                Log.Warning("Missing debuff sprite file for choice items. Substituting default...");
                sprite = Addressables.LoadAssetAsync<Sprite>("RoR2/Base/Common/MiscIcons/texMysteryIcon.png").WaitForCompletion();
            }

            return sprite;
        }

        // Create buff/debuff based on choice lock
        private static BuffDef CreateChoiceLock(string name, Sprite icon)
        {
            BuffDef buff = ScriptableObject.CreateInstance<BuffDef>();
            buff.name = name;
            buff.iconSprite = icon;
            buff.buffColor = Color.white;

            buff.canStack = false;

            buff.isDebuff = true;

            buff.isHidden = false;

            ContentAddition.AddBuffDef(buff);

            return buff;
        }

        // Remove any and all choice locks
        public static void RemoveAllChoiceLocks(CharacterBody body)
        {
            foreach (BuffDef buff in ChoiceLocks)
            {
                if (body.HasBuff(buff))
                    body.RemoveBuff(buff);
            }
        }

        // Checks if player has any choice lock
        public static bool HasChoiceLock(CharacterBody body)
        {
            return
                body.HasBuff(ChoicePrimaryLock) ||
                body.HasBuff(ChoiceSecondaryLock) ||
                body.HasBuff(ChoiceUtilityLock) ||
                body.HasBuff(ChoiceSpecialLock);
        }
    }
}
