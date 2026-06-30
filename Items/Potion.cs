//using R2API;
//using RoR2;

//namespace PokeItems.Items
//{
//    internal class Potion
//    {
//        public static ItemDef itemDef;

//        // Crit more and crit harder.
//        public static ConfigurableValue<bool> isEnabled = new(
//            "Item: Potion",
//            "Enabled",
//            true,
//            "Whether or not the item is enabled.",
//            ["ITEM_POTION_DESC"]
//        );
//        public static ConfigurableValue<float> healRegenPerStack = new(
//            "Item: Potion",
//            "Heal Regeneration",
//            6f,
//            "Amount of heal regeneration gained per stack.",
//            ["ITEM_POTION_DESC"]
//        );
//        public static ConfigurableValue<float> healRegenPerExtraStack = new(
//            "Item: Potion",
//            "Heal Regeneration Extra Stacks",
//            6f,
//            "Amount of heal regeneration gained for extra stacks.",
//            ["ITEM_POTION_DESC"]
//        );
//        public static float healRegen = healRegenPerStack.Value;
//        public static float healRegenExtraStacks = healRegenPerExtraStack.Value;

//        internal static void Init()
//        {
//            itemDef = ItemManager.GenerateItem("Potion", [ItemTag.Healing, ItemTag.CanBeTemporary], ItemTier.Tier1);

//            Hooks();
//        }

//        public static void Hooks()
//        {
//            RecalculateStatsAPI.GetStatCoefficients += (sender, args) =>
//            {
//                if (sender && sender.inventory)
//                {
//                    int count = sender.inventory.GetItemCountEffective(itemDef);
//                    if (count > 0)
//                    {
//                        //args.critAdd += Utilities.GetLinearStacking(critChancePerStack.Value, critChancePerExtraStack.Value, count);
//                        //args.critDamageMultAdd += Utilities.GetLinearStacking(percentCritDamage, percentCritDamageExtraStacks, count);
//                    }
//                }
//            };
//        }
//    }
//}