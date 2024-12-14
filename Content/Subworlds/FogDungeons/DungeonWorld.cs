using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using SubworldLibrary;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.WorldBuilding;


namespace CalamityOverhaul.Content.Subworlds.FogDungeons
{
    internal class DungeonWorld : Subworld
    {
        public override int Width => 2000;

        public override int Height => 2000;

        public static bool Active => SubworldSystem.IsActive<DungeonWorld>();

        public override List<GenPass> Tasks => [new FogDungeonGen()];

        public static void Enter() => SubworldSystem.Enter<DungeonWorld>();

        public override void OnEnter() {
            SubworldSystem.noReturn = true;
            SubworldSystem.hideUnderworld = false;
        }

        public override void OnLoad() {
            Main.dayTime = true;
            Main.time = 27000;
        }

        public override void Update() {

        }

        public override bool ChangeAudio() {
            return base.ChangeAudio();
        }

        public override void DrawMenu(GameTime gameTime) {
            Main.spriteBatch.Draw(CWRUtils.GetT2DValue(CWRConstant.Asset + "Sky/FogDungeonBack"), Vector2.Zero, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0); ;
            DynamicSpriteFontExtensionMethods.DrawString(Main.spriteBatch, FontAssets.DeathText.Value, Main.statusText
                , new Vector2(Main.screenWidth, Main.screenHeight) / 2 - FontAssets.DeathText.Value.MeasureString(Main.statusText) / 2, Color.White);
        }

        public override void DrawSetup(GameTime gameTime) {
            base.DrawSetup(gameTime);
        }

        public override float GetGravity(Entity entity) {
            return base.GetGravity(entity);
        }
    }
}
