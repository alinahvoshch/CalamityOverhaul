﻿using CalamityMod.Items.Materials;
using CalamityOverhaul.Common;
using InnoVault.TileProcessors;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace CalamityOverhaul.Content.Industrials.Generator.WindGriven
{
    internal class WindGrivenGeneratorMK2 : ModItem
    {
        public override string Texture => CWRConstant.Asset + "Generator/WindGrivenGeneratorMK2Item";
        public static LocalizedText UnderstandWindGrivenMK2 { get; private set; }
        public override void SetStaticDefaults() {
            UnderstandWindGrivenMK2 = this.GetLocalization(nameof(UnderstandWindGrivenMK2),
                () => "A deep understanding of wind power is required");
        }
        public override void SetDefaults() {
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 9999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.value = Item.buyPrice(0, 2, 0, 0);
            Item.rare = ItemRarityID.Pink;
            Item.createTile = ModContent.TileType<WindGrivenGeneratorMK2Tile>();
            Item.CWR().StorageUE = true;
            Item.CWR().ConsumeUseUE = 2200;
        }

        public static LocalizedText WindGrivenRecipeCondition(out Func<bool> condition) {
            condition = new Func<bool>(() => Main.LocalPlayer.CWR().UnderstandWindGrivenMK2);
            return UnderstandWindGrivenMK2;
        }

        public override void AddRecipes() {
            CreateRecipe().
                AddIngredient<WindGrivenGenerator>().
                AddIngredient<DubiousPlating>(20).
                AddIngredient<MysteriousCircuitry>(20).
                AddRecipeGroup(CWRRecipes.MythrilBarGroup, 5).
                AddIngredient(ItemID.GoldBar, 15).
                AddCondition(WindGrivenRecipeCondition(out var condition), condition).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }

    internal class WindGrivenGeneratorMK2Tile : BaseGeneratorTile
    {
        public override string Texture => CWRConstant.Asset + "Generator/WindGrivenGeneratorMK2Tile";
        public override int GeneratorTP => TileProcessorLoader.GetModuleID<WindGrivenGeneratorMK2TP>();
        public override int GeneratorUI => 0;
        public override int TargetItem => ModContent.ItemType<WindGrivenGeneratorMK2>();
        public override void SetStaticDefaults() {
            Main.tileLighted[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = false;
            Main.tileWaterDeath[Type] = false;
            Main.tileSolidTop[Type] = true;
            AddMapEntry(new Color(67, 72, 81), CWRUtils.SafeGetItemName<WindGrivenGeneratorMK2>());

            TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
            TileObjectData.newTile.Width = 3;
            TileObjectData.newTile.Height = 27;
            TileObjectData.newTile.Origin = new Point16(1, 25);
            TileObjectData.newTile.CoordinateHeights = [
                16, 16, 16, 16, 16, 16, 16, 16, 16
                , 16, 16, 16, 16, 16, 16, 16, 16, 16
                , 16, 16, 16, 16, 16, 16, 16, 16, 16];
            TileObjectData.newTile.StyleWrapLimit = 36;
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.CoordinatePadding = 2;
            TileObjectData.addTile(Type);
        }
        public override bool CanDrop(int i, int j) => false;
    }

    internal class WindGrivenGeneratorMK2TP : BaseGeneratorTP, ICWRLoader
    {
        public override int TargetTileID => ModContent.TileType<WindGrivenGeneratorMK2Tile>();
        private float rotition;
        private float rotSpeed;
        private int soundCount;
        public override float MaxUEValue => 2200;
        public override int TargetItem => ModContent.ItemType<WindGrivenGeneratorMK2>();
        internal static Asset<Texture2D> MK2Blade { get; private set; }
        void ICWRLoader.LoadAsset() => MK2Blade = CWRUtils.GetT2DAsset(CWRConstant.Asset + "Generator/MK2Blade");
        void ICWRLoader.UnLoadData() => MK2Blade = null;
        public override void GeneratorUpdate() {
            rotSpeed = 0.012f;
            rotition += rotSpeed;
            if (MachineData.UEvalue < MaxUEValue) {
                MachineData.UEvalue += rotSpeed * 120;
            }

            if (++soundCount > 160 && Main.LocalPlayer.Distance(CenterInWorld) < 600) {
                SoundEngine.PlaySound(CWRSound.Windmill with { Volume = 0.65f, MaxInstances = 12 }, CenterInWorld);
                soundCount = 0;
            }
        }

        public override void FrontDraw(SpriteBatch spriteBatch) {
            Texture2D blade = MK2Blade.Value;
            Vector2 drawPos = PosInWorld - Main.screenPosition + new Vector2(24, 28);
            Vector2 drawOrig = new Vector2(blade.Width / 2, blade.Height);
            for (int i = 0; i < 3; i++) {
                float drawRot = (MathHelper.TwoPi) / 3f * i + rotition;
                Color color = Lighting.GetColor(Position.ToPoint() + drawRot.ToRotationVector2().ToPoint());
                spriteBatch.Draw(blade, drawPos, null, color, drawRot, drawOrig, 1, SpriteEffects.None, 0);
            }
        }
    }
}
