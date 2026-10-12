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

            buffDef.name = name; // Buff Name
            buffDef.iconSprite = TryGetSprite(name); // Get sprite icon
            buffDef.buffColor = color; // Buff color

            // Does this buff show stack count?
            buffDef.canStack = canStack;

            // Is this a debuff? This will be affected via items and equipment like Blast Showers
            buffDef.isDebuff = isDebuff;

            // Will this buff not display in-game?
            buffDef.isHidden = isHidden;

            // Add to R2API
            ContentAddition.AddBuffDef(buffDef);

            // Log about the completion of BuffDef
            Log.Info("Buff created: " + name + "; CanStack: " + canStack + "; isDebuff: " + isDebuff + "; isHidden: " + isHidden);

            // Return the BuffDef
            return buffDef;
        }

        // Helper function to ensure validity of sprite icon
        private static Sprite TryGetSprite(string spriteName)
        {
            // Grab item sprite from assetbundle (replace with mystery if it doesn't exist)
            Sprite sprite = AssetManager.bundle.LoadAsset<Sprite>(spriteName + ".png");
            if (sprite == null)
            {
                Log.Warning("Missing debuff sprite file for" + buffDef.name + ". Substituting default...");
                sprite = Addressables.LoadAssetAsync<Sprite>("RoR2/Base/Common/MiscIcons/texMysteryIcon.png").WaitForCompletion();
            }

            return sprite;
        }
    }
}
