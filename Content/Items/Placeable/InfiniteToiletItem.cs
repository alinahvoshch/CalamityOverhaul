﻿using CalamityOverhaul.Common;
using CalamityOverhaul.Content.Items.Materials;
using CalamityOverhaul.Content.Tiles;
using CalamityOverhaul.Content.UIs.SupertableUIs;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityOverhaul.Content.Items.Placeable
{
    internal class InfiniteToiletItem : ModItem
    {
        public static string[] FullItems = ["CalamityOverhaul/InfiniteIngot", "CalamityOverhaul/InfiniteIngot", "0", "0", "0", "0", "0", "0", "0",
            "CalamityOverhaul/InfiniteIngot", "CalamityOverhaul/InfiniteIngot", "0", "0", "0", "0", "0", "0", "0",
            "CalamityOverhaul/InfiniteIngot", "CalamityOverhaul/InfiniteIngot", "0", "0", "0", "0", "0", "0", "0",
            "CalamityOverhaul/InfiniteIngot", "CalamityOverhaul/InfiniteIngot", "CalamityOverhaul/InfiniteIngot", "CalamityOverhaul/InfiniteIngot", "CalamityOverhaul/InfiniteIngot", "CalamityOverhaul/InfiniteIngot", "CalamityOverhaul/InfiniteIngot", "CalamityOverhaul/InfiniteIngot", "CalamityOverhaul/InfiniteIngot",
            "0", "CalamityOverhaul/InfiniteIngot", "CalamityOverhaul/InfinityCatalyst", "CalamityOverhaul/InfinityCatalyst", "CalamityOverhaul/InfinityCatalyst", "CalamityOverhaul/InfinityCatalyst", "CalamityOverhaul/InfinityCatalyst", "CalamityOverhaul/InfiniteIngot", "0",
            "0", "0", "CalamityOverhaul/InfiniteIngot", "CalamityOverhaul/InfinityCatalyst", "CalamityOverhaul/InfinityCatalyst", "CalamityOverhaul/InfinityCatalyst", "CalamityOverhaul/InfiniteIngot", "0", "0",
            "0", "0", "0", "CalamityOverhaul/InfiniteIngot", "CalamityOverhaul/InfinityCatalyst", "CalamityOverhaul/InfiniteIngot", "0", "0", "0",
            "0", "0", "0", "CalamityOverhaul/InfiniteIngot", "CalamityOverhaul/InfiniteIngot", "CalamityOverhaul/InfiniteIngot", "0", "0", "0",
            "0", "0", "CalamityOverhaul/InfiniteIngot", "CalamityOverhaul/InfiniteIngot", "CalamityOverhaul/InfiniteIngot", "CalamityOverhaul/InfiniteIngot", "CalamityOverhaul/InfiniteIngot", "0", "0",
            "CalamityOverhaul/InfiniteToiletItem"
        ];
        public override string Texture => CWRConstant.Item + "Placeable/" + "InfiniteToiletItem";
        public override void SetStaticDefaults() => SupertableUI.OtherRpsData_ZenithWorld_StringList.Add(FullItems);
        public override void SetDefaults() {
            Item.width = 28;
            Item.height = 20;
            Item.maxStack = 9999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<InfiniteToiletTile>();
            Item.CWR().OmigaSnyContent = FullItems;
            Item.CWR().AutoloadingOmigaSnyRecipe = false;//这个物品的配方有些特殊，所以禁用掉自动装填转而使用更自由的手动装填
        }

        public override bool PreDrawTooltipLine(DrawableTooltipLine line, ref int yOffset) {
            if (line.Name == "ItemName" && line.Mod == "Terraria") {
                InfiniteIngot.DrawColorText(Main.spriteBatch, line);
                return false;
            }
            return true;
        }

        public static void DrawItemIcon(SpriteBatch spriteBatch, Vector2 position, int Type, float alp = 1) {
            spriteBatch.Draw(TextureAssets.Item[Type].Value, position, null, Color.White
                , 0, TextureAssets.Item[Type].Value.Size() / 2, 0.8f, SpriteEffects.None, 0);
        }

        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame
            , Color drawColor, Color itemColor, Vector2 origin, float scale) {
            return true;
        }

        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor
            , Color alphaColor, ref float rotation, ref float scale, int whoAmI) {
            spriteBatch.Draw(TextureAssets.Item[Type].Value, Item.Center - Main.screenPosition, null
                , Main.DiscoColor, 0, TextureAssets.Item[Type].Value.Size() / 2, 1, SpriteEffects.None, 0);
            return false;
        }

        public override void AddRecipes() {
            Condition condition = new Condition(CWRLocText.GetTextKey("OnlyZenith"), () => Main.zenithWorld);
            CreateRecipe()
                .AddIngredient<InfiniteIngot>(29)
                .AddIngredient<InfinityCatalyst>(9)
                .AddBlockingSynthesisEvent()
                .AddTile(ModContent.TileType<TransmutationOfMatter>())
                .AddCondition(condition)
                .Register();
        }
    }
}
