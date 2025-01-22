﻿using CalamityOverhaul.Content.Projectiles.AmmoBoxs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityOverhaul.Content.Items.Placeable
{
    internal class AmmoBoxFire : ModItem
    {
        public override string Texture => CWRConstant.Item + "Placeable/NapalmBombBox";
        public override void SetDefaults() {
            Item.width = Item.height = 32;
            Item.value = 890;
            Item.useTime = Item.useAnimation = 22;
            Item.consumable = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item1;
            Item.rare = ItemRarityID.LightRed;
            Item.maxStack = 64;
            Item.SetHeldProj<NapalmBombHeld>();
        }

        public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient(ItemID.AmmoBox)
                .AddIngredient(ItemID.EmptyBullet, 50)
                .AddIngredient(ItemID.LivingFireBlock, 5)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
