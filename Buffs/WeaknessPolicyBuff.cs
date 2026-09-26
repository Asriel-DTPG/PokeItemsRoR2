using PokeItems.Managers;
using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace PokeItems.Buffs
{
    internal static class WeaknessPolicyBuff
    {
        public static BuffDef WeaknessPBuff;

        public static void Init()
        {
            //ChoicePrimaryLock = CreateChoiceLock(
            //    "PrimaryLock",
            //    TryGetSprite("PrimaryLock")
            //);
            WeaknessPBuff.name = "WeaknessPolicy";
            WeaknessPBuff.iconSprite = TryGetSprite("WeaknessPolicy");
            WeaknessPBuff.buffColor = Color.white;

            WeaknessPBuff.canStack = true;

            WeaknessPBuff.isDebuff = false;

            WeaknessPBuff.isHidden = false;

            ContentAddition.AddBuffDef(WeaknessPBuff);
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
    }
}
