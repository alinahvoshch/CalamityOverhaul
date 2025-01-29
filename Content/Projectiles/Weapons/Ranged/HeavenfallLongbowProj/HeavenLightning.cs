﻿using CalamityMod;
using CalamityMod.Graphics.Primitives;
using CalamityOverhaul.Content.CWRDamageTypes;
using CalamityOverhaul.Content.Items.Ranged;
using CalamityOverhaul.Content.Particles;
using InnoVault.PRT;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityOverhaul.Content.Projectiles.Weapons.Ranged.HeavenfallLongbowProj
{
    internal class HeavenLightning : ModProjectile
    {
        public const int Lifetime = 45;
        private Color chromaColor => VaultUtils.MultiStepColorLerp(Projectile.timeLeft % 15 / 15f, HeavenfallLongbow.rainbowColors);
        public override string Texture => "CalamityMod/Projectiles/LightningProj";
        public override void SetStaticDefaults() {
            ProjectileID.Sets.DrawScreenCheckFluff[Type] = 10000;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 50;
        }

        public override void SetDefaults() {
            Projectile.width = 22;
            Projectile.height = 22;
            Projectile.alpha = 255;
            Projectile.penetrate = -1;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
            Projectile.DamageType = EndlessDamageClass.Instance;
            Projectile.MaxUpdates = 5;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 13 * Projectile.MaxUpdates;
            Projectile.timeLeft = Projectile.MaxUpdates * Lifetime;
        }

        public override void AI() {
            Projectile.frameCounter++;
            Projectile.oldPos[1] = Projectile.oldPos[0];

            float adjustedTimeLife = Projectile.timeLeft / Projectile.MaxUpdates;
            Projectile.Opacity = Utils.GetLerpValue(0f, 6f, adjustedTimeLife, true) * Utils.GetLerpValue(Lifetime, Lifetime - 3f, adjustedTimeLife, true);
            Projectile.scale = Projectile.Opacity;

            Lighting.AddLight(Projectile.Center, Color.White.ToVector3());
            if (Projectile.frameCounter >= Projectile.extraUpdates * 2) {
                Projectile.frameCounter = 0;
            }

            if (!VaultUtils.isServer) {
                Color outerSparkColor = chromaColor;
                float scaleBoost = MathHelper.Clamp(Projectile.ai[1] * 0.005f, 0f, 2f);
                float outerSparkScale = 1.3f + scaleBoost;
                PRT_HeavenfallStar spark = new PRT_HeavenfallStar(Projectile.Center, Projectile.velocity, false, 7, outerSparkScale, outerSparkColor);
                PRTLoader.AddParticle(spark);

                Color innerSparkColor = VaultUtils.MultiStepColorLerp(Projectile.ai[1] % 30 / 30f, HeavenfallLongbow.rainbowColors);
                float innerSparkScale = 0.6f + scaleBoost;
                PRT_HeavenfallStar spark2 = new PRT_HeavenfallStar(Projectile.Center, Projectile.velocity, false, 7, innerSparkScale, innerSparkColor);
                PRTLoader.AddParticle(spark2);
            }

            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
        }

        public float PrimitiveWidthFunction(float completionRatio) => CalamityUtils.Convert01To010(completionRatio) * Projectile.scale * Projectile.width;

        public Color PrimitiveColorFunction(float completionRatio) {
            float colorInterpolant = (float)Math.Sin(Projectile.identity / 3f + completionRatio * 20f + Main.GlobalTimeWrappedHourly * 1.1f) * 0.5f + 0.5f;
            Color color = CalamityUtils.MulticolorLerp(colorInterpolant, Color.Red, Color.Orange, Color.Yellow, Color.Green, Color.Blue, Color.Indigo, Color.Violet);
            return color;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) {
            List<Vector2> checkPoints = Projectile.oldPos.Where(oldPos => oldPos != Vector2.Zero).ToList();
            if (checkPoints.Count <= 2)
                return false;

            for (int i = 0; i < checkPoints.Count - 1; i++) {
                float _ = 0f;
                float width = PrimitiveWidthFunction(i / (float)checkPoints.Count);
                if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), checkPoints[i], checkPoints[i + 1], width * 0.8f, ref _))
                    return true;
            }
            return false;
        }

        public override bool PreDraw(ref Color lightColor) {
            GameShaders.Misc["CalamityMod:HeavenlyGaleLightningArc"].UseImage1("Images/Misc/Perlin");
            GameShaders.Misc["CalamityMod:HeavenlyGaleLightningArc"].Apply();

            PrimitiveRenderer.RenderTrail(Projectile.oldPos, new PrimitiveSettings(PrimitiveWidthFunction, PrimitiveColorFunction
                , (float _) => Projectile.Size * 0.5f, smoothen: true, pixelate: false, GameShaders.Misc["CalamityMod:HeavenlyGaleLightningArc"]), 50);
            return false;
        }
    }
}
