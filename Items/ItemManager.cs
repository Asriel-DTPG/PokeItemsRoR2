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
            itemDef.name = name;
            itemDef.nameToken = "ITEM_" + itemDef.name + "_NAME";
            itemDef.pickupToken = "ITEM_" + itemDef.name + "_PICKUP";
            itemDef.descriptionToken = "ITEM_" + itemDef.name + "_DESC";
            itemDef.loreToken = "ITEM_" + itemDef.name + "_LORE";
            tokenMap[itemDef.descriptionToken] = itemTokens;

            // Item Tier
            itemDef.deprecatedTier = tier;

            // You can create your own icons and prefabs through assetbundles, but to keep this example plugin brief, we'll be using question marks.
            itemDef.pickupIconSprite = Addressables.LoadAssetAsync<Sprite>("RoR2/Base/Common/MiscIcons/texMysteryIcon.png").WaitForCompletion();
            itemDef.pickupModelPrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Mystery/PickupMystery.prefab").WaitForCompletion();

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
