﻿using InnoVault.PRT;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;

namespace CalamityOverhaul.Content.Particles
{
    internal class PRT_SparkAlpha : BasePRT
    {
        public Color InitialColor;
        public bool AffectedByGravity;
        public Entity entity;
        public override string Texture => "CalamityMod/Projectiles/StarProj";
        public PRT_SparkAlpha(Vector2 relativePosition, Vector2 velocity, bool affectedByGravity, int lifetime, float scale, Color color) {
            Position = relativePosition;
            Velocity = velocity;
            AffectedByGravity = affectedByGravity;
            Scale = scale;
            Lifetime = lifetime;
            Color = InitialColor = color;
        }
        public override void SetProperty() => SetLifetime = true;
        public override void AI() {
            Scale *= 0.95f;
            Color = Color.Lerp(InitialColor, Color.Transparent, (float)Math.Pow(LifetimeCompletion, 3D));
            Velocity *= 0.95f;
            if (Velocity.Length() < 12f && AffectedByGravity) {
                Velocity.X *= 0.94f;
                Velocity.Y += 0.25f;
            }
            Rotation = Velocity.ToRotation() + MathHelper.PiOver2;
            if (entity != null) {
                if (entity.active) {
                    Position += entity.velocity;
                }
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch) {
            Vector2 scale = new Vector2(0.5f, 1.6f) * Scale;
            Texture2D texture = PRTLoader.PRT_IDToTexture[ID];

            spriteBatch.Draw(texture, Position - Main.screenPosition, null
                , Color, Rotation, texture.Size() * 0.5f, scale, 0, 0f);
            spriteBatch.Draw(texture, Position - Main.screenPosition, null
                , Color, Rotation, texture.Size() * 0.5f, scale * new Vector2(0.45f, 1f), 0, 0f);

            return false;
        }
    }
}
