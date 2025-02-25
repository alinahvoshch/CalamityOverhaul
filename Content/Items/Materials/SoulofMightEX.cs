﻿using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityOverhaul.Content.Items.Materials
{
    internal class SoulofMightEX : ModItem
    {
        public override string Texture => CWRConstant.Item_Material + "SoulofMightEX";
        public override void SetStaticDefaults() {
            Item.ResearchUnlockCount = 64;
            Main.RegisterItemAnimation(Type, new DrawAnimationVertical(5, 8));
            ItemID.Sets.AnimatesAsSoul[Type] = true;
        }
        public override void SetDefaults() {
            Item.width = Item.height = 30;
            Item.maxStack = 9999;
            Item.rare = ItemRarityID.Red;
            Item.value = Item.sellPrice(gold: 12);
            Item.useAnimation = Item.useTime = 15;
            Item.useStyle = ItemUseStyleID.HoldUp;
        }
    }
}
