﻿using InnoVault.PRT;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;

namespace CalamityOverhaul.Content.PRTTypes
{
    internal class PRT_HeavenSmoke : BasePRT
    {
        public override string Texture => "CalamityMod/Particles/HeavySmoke";

        private Color[] rainbowColors = [Color.Red, Color.Orange, Color.Yellow, Color.Green, Color.Blue, Color.Indigo, Color.Violet];

        private Color rainColor => VaultUtils.MultiStepColorLerp(sengs % 30 / 30f, rainbowColors);
        private float Spin;
        private int MaxTime;
        private bool StrongVisual;
        private bool Glowing;
        private float HueShift;
        private float Scale2;
        private int sengs;
        private Player player;
        private static int FrameAmount = 6;

        public PRT_HeavenSmoke(Vector2 position, Vector2 velocity, Color color, int lifetime, float scale, float opacity
            , float rotationSpeed = 0f, bool glowing = false, float hueshift = 0f, bool required = false, Player player = null) {
            Position = position;
            Velocity = velocity;
            Color = color;
            Scale = Scale2 = scale;
            Lifetime = MaxTime = lifetime;
            Opacity = opacity;
            Spin = rotationSpeed;
            StrongVisual = required;
            Glowing = glowing;
            HueShift = hueshift;
            this.player = player;
            sengs = Main.rand.Next(30);
        }

        public override void SetProperty() {
            if (Glowing) {
                PRTDrawMode = PRTDrawModeEnum.AdditiveBlend;
            }
            else {
                PRTDrawMode = PRTDrawModeEnum.NonPremultiplied;
            }
            ai[0] = Main.rand.Next(7);
        }

        public override void AI() {
            if (Time / (float)Lifetime < 0.2f)
                Scale += 0.01f;
            else
                Scale *= 0.975f;

            Color = Color == Color.White ? rainColor : Color;
            Opacity *= 0.98f;
            Rotation += Spin * ((Velocity.X > 0) ? 1f : -1f);
            Velocity *= 0.85f;
            float opacity = Utils.GetLerpValue(1f, 0.85f, LifetimeCompletion, true);
            Color *= opacity;
            if (player != null)
                Position += player.velocity;
            sengs++;
        }

        public override bool PreDraw(SpriteBatch spriteBatch) {
            Texture2D tex = PRTLoader.PRT_IDToTexture[ID];
            int animationFrame = (int)Math.Floor(Time / ((float)(Lifetime / (float)FrameAmount)));
            Rectangle frame = new Rectangle((int)(80 * ai[0]), 80 * animationFrame, 80, 80);
            Vector2 pos = Position - Main.screenPosition;
            Vector2 org = frame.Size() / 2f;
            spriteBatch.Draw(tex, pos, frame, rainColor * Opacity, Rotation, org, Scale, SpriteEffects.None, 0);
            return false;
        }
    }
}
