using R2API;
using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace PokeItems.Managers
{
    internal class BuffManager
    {
        public static BuffDef buffDef;

        public static BuffDef CreateBuffDef(string name, Color color, bool canStack, bool isDebuff, bool isHidden)
        {
            buffDef = ScriptableObject.CreateInstance<BuffDef>();
            buffDef.name = name;
            buffDef.iconSprite = TryGetSprite(name);
            buffDef.buffColor = Color.white;

            buffDef.canStack = false;

            buffDef.isDebuff = true;

            buffDef.isHidden = false;

            ContentAddition.AddBuffDef(buffDef);

            return buffDef;
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
