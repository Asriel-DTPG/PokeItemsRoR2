using R2API;
using RoR2;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace PokeItems.Items
{
    internal class ItemManager
    {
        public static ItemDef itemDef;
        public static readonly Dictionary<string, object[]> tokenMap = new();

        public static ItemDef CreateItemDef(string name, ItemTier tier, bool canRemove, bool hidden, ItemTag[] tags, params object[] itemTokens)
        {
            itemDef = ScriptableObject.CreateInstance<ItemDef>();

            // Tokens
            itemDef.name = name; // Item Name
            itemDef.nameToken = "Item_" + itemDef.name + "_Name"; // Name Token
            itemDef.pickupToken = "Item_" + itemDef.name + "_Pickup"; // Pickup Token
            itemDef.descriptionToken = "Item_" + itemDef.name + "_Desc"; // Description Token
            itemDef.loreToken = "Item_" + itemDef.name + "_Lore"; // Lore Token
            tokenMap[itemDef.descriptionToken] = itemTokens; // Parse Tokens

            // Grab item model from assetbundle (replace with mystery if it doesn't exist. No-tier items not having a prefab is normal)
            GameObject prefab = AssetManager.bundle.LoadAsset<GameObject>(name + ".prefab");
            if (prefab == null)
            {
                if (tier != ItemTier.NoTier)
                    Log.Warning("Missing prefab file for item " + itemDef.name + ". Substituting default...");

                prefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Mystery/PickupMystery.prefab").WaitForCompletion();
            }

            // Grab item sprite from assetbundle (replace with mystery if it doesn't exist)
            Sprite sprite = AssetManager.bundle.LoadAsset<Sprite>(name + ".png");
            if (sprite == null)
            {
                Log.Warning("Missing sprite file for item " + itemDef.name + ". Substituting default...");
                sprite = itemDef.pickupIconSprite = Addressables.LoadAssetAsync<Sprite>("RoR2/Base/Common/MiscIcons/texMysteryIcon.png").WaitForCompletion();
            }

            // Give ModePanelParameters for item display.
            ModelPanelParameters modelPanelParameters = prefab.AddComponent<ModelPanelParameters>();
            modelPanelParameters.focusPointTransform = prefab.transform;
            modelPanelParameters.cameraPositionTransform = prefab.transform;
            modelPanelParameters.maxDistance = 10f;
            modelPanelParameters.minDistance = 5f;

            // Grab item sprite from assetbundle (replace with mystery if it doesn't exist)
            itemDef.pickupIconSprite = sprite;
            itemDef.pickupModelPrefab = prefab;

            // Item Tier (If item is a DLC tier, make it a DLC requirement)
            itemDef.deprecatedTier = tier;
            if (itemDef.tier == ItemTier.VoidBoss || itemDef.tier == ItemTier.VoidTier1 ||
                itemDef.tier == ItemTier.VoidTier2 || itemDef.tier == ItemTier.VoidTier3)
            {
                itemDef.requiredExpansion = PokeItems.sotvDLC;
            }

            // If this item can be removed by any means in-game (typically for no-tier items)
            itemDef.canRemove = canRemove;

            // If this item cannot be claimed by normal means
            itemDef.hidden = hidden;

            // Tags to describe the item
            itemDef.tags = tags;

            // Display where the item will be kept around the survivor
            var displayRules = new ItemDisplayRuleDict(null);

            // Wait until .language file has been initialized or when switching languages
            Language.onCurrentLanguageChanged += FormatDescription;

            // Add it to R2API
            ItemAPI.Add(new CustomItem(itemDef, displayRules));

            // Log about the completion of ItemDef
            string tagString = "";
            foreach (ItemTag tag in tags)
            {
                tagString += tag + ", ";
            }
            Log.Info(
                "Item created: " + name + "; Tier: " + tier + "; Tags: " + tagString.Trim());

            // Return the ItemDef
            return itemDef;
        }

        // Set the description of the item based on .language file and parse in values.
        private static void FormatDescription()
        {
            foreach (var mapPair in tokenMap)
            {
                string desc = Language.GetStringFormatted(mapPair.Key, mapPair.Value);
                LanguageAPI.AddOverlay(mapPair.Key, desc);
            }
        }
    }
}
