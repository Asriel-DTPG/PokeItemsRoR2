using System.IO;
using UnityEngine;

namespace PokeItems.Managers
{
    // This code is based on shirograhm's TooManyItems mod (slightly altered to reference the assetbundle file)
    // https://github.com/shirograhm/TooManyItemsRoR2/blob/master/TooManyItems/Managers/AssetManager.cs
    public static class AssetManager
    {
        // Variables for AssetBundle
        public static AssetBundle bundle;
        public const string bundleName = "pokeassets";

        // Get the path of the plugin and then refer to the name of the AssetBundle
        public static string AssetBundlePath
        {
            get
            {
                return Path.Combine(Path.GetDirectoryName(PokeItems.PInfo.Location), bundleName);
            }
        }

        public static void Init()
        {
            // Load the AssetBundle
            bundle = AssetBundle.LoadFromFile(AssetBundlePath);
        }
    }
}