﻿using CalamityMod;
using CalamityMod.Graphics.Primitives;
using CalamityMod.Projectiles.Magic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityOverhaul.Content.Projectiles.Weapons.Melee.Rapiers
{
    internal class FadingGloryBeam : ModProjectile
    {
        public override string Texture => CWRConstant.Item_Melee + "FadingGloryGlow";

        private NPC hitnpc;
        private Vector2 oldnpcPos;
        public override void SetStaticDefaults() {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 30;
        }
        public override void SetDefaults() {
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.tileCollide = false;
            Projectile.Size = new Vector2(36);
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.timeLeft = 380;
            Projectile.extraUpdates = 3;
            Projectile.alpha = 0;
        }

        public override void AI() {
            Projectile.localAI[1] = (float)Math.Abs(Math.Sin(Projectile.timeLeft * 0.05f));
            Projectile.rotation = Projectile.velocity.ToRotation();
            if (Projectile.ai[1] <= 0) {
                if (Projectile.ai[2] > 0) {
                    Projectile.penetrate = 1;
                    Projectile.extraUpdates = 5;
                    Projectile.alpha += 15;
                    NPC target = Projectile.Center.FindClosestNPC(1800);
                    if (target != null) {
                        Projectile.ChasingBehavior2(target.Center, 1, 0.35f);
                    }
                }
                else {
                    Projectile.alpha = (int)(155 + Projectile.ai[1] * 100);
                    if (Projectile.ai[0] > 20) {
                        Projectile.velocity *= 0.98f;
                    }
                }
            }
            else {
                if (hitnpc == null || !hitnpc.active) {
                    Projectile.Kill();
                    return;
                }
                Projectile.extraUpdates = 0;
                Vector2 npcUpdateVer = oldnpcPos.To(hitnpc.position);
                oldnpcPos = hitnpc.position;
                Projectile.position += npcUpdateVer;
                float sengs = 0.9f;
                if (hitnpc.width > 180) {
                    sengs += 0.01f;
                }
                if (hitnpc.width > 280) {
                    sengs += 0.01f;
                }
                if (hitnpc.width > 320) {
                    sengs += 0.01f;
                }
                if (hitnpc.width > 340) {
                    sengs += 0.01f;
                }
                Projectile.velocity *= sengs;
            }
            Projectile.ai[0]++;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
            if (Projectile.ai[1] == 0 && Projectile.ai[0] <= 60) {
                hitnpc = target;
                oldnpcPos = target.position;
                Projectile.timeLeft = 90;
                if (!target.active) {
                    hitnpc = null;
                    Projectile.localAI[0] = 0;
                    return;
                }
                if (Projectile.ai[2] <= 0) {
                    for (int i = 0; i < 3; i++) {
                        Vector2 spanPos = Projectile.Center;
                        spanPos += Projectile.velocity.UnitVector() * -526;
                        spanPos = spanPos.RotatedByRandom(0.2f);
                        Vector2 ver = spanPos.To(target.Center).UnitVector() * 13;
                        int proj2 = Projectile.NewProjectile(Projectile.GetSource_FromAI(), spanPos, ver
                        , ModContent.ProjectileType<FadingGloryBeam>(), Projectile.damage, Projectile.knockBack, Projectile.owner, ai2: 1);
                        Main.projectile[proj2].rotation = Main.projectile[proj2].velocity.ToRotation();
                    }
                }

                Projectile.ai[1]++;
            }
        }

        public override void OnKill(int timeLeft) {
            if (Projectile.IsOwnedByLocalPlayer()) {
                if (Projectile.ai[2] <= 0 && Projectile.numHits > 0) {
                    Vector2 tentacleVelocity = Projectile.rotation.ToRotationVector2();
                    tentacleVelocity.Normalize();
                    Vector2 tentacleRandVelocity = new Vector2(Main.rand.Next(-100, 101), Main.rand.Next(-100, 101));
                    tentacleRandVelocity.Normalize();
                    tentacleVelocity = tentacleVelocity * 4f + tentacleRandVelocity;
                    tentacleVelocity.Normalize();
                    tentacleVelocity *= 23;
                    float tentacleYDirection = Main.rand.Next(10, 80) * 0.001f;
                    if (Main.rand.NextBool()) {
                        tentacleYDirection *= -1f;
                    }
                    float tentacleXDirection = Main.rand.Next(10, 80) * 0.001f;
                    if (Main.rand.NextBool()) {
                        tentacleXDirection *= -1f;
                    }
                    int proj = Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, tentacleVelocity
                        , ModContent.ProjectileType<EldritchTentacle>(), Projectile.damage
                        , Projectile.knockBack, Projectile.owner, tentacleXDirection, tentacleYDirection);
                    Main.projectile[proj].DamageType = DamageClass.Magic;
                }
                else {
                    CWRDust.SplashDust(Projectile, 21, DustID.FireworkFountain_Red, DustID.FireworkFountain_Red, 13, Color.DarkRed);
                }
            }
            Projectile.Explode(explosionSound: SoundID.Item14 with { Pitch = 0.6f });
        }

        public float PrimitiveWidthFunction(float completionRatio) => Projectile.scale * 20f;

        public Color PrimitiveColorFunction(float _) => Color.Red;

        public void DrawTrild() {
            float localIdentityOffset = Projectile.identity * 0.1372f;
            Color mainColor = CalamityUtils.MulticolorLerp((Main.GlobalTimeWrappedHourly * 2f + localIdentityOffset) % 1f, Color.Red, Color.DarkRed, Color.DarkRed, Color.OrangeRed, Color.IndianRed);
            Color secondaryColor = CalamityUtils.MulticolorLerp((Main.GlobalTimeWrappedHourly * 2f + localIdentityOffset + 0.2f) % 1f, Color.DarkRed, Color.IndianRed, Color.OrangeRed, Color.IndianRed, Color.MediumVioletRed);

            mainColor = Color.Lerp(Color.Red, mainColor, 0.85f);
            secondaryColor = Color.Lerp(Color.OrangeRed, secondaryColor, 0.85f);

            GameShaders.Misc["CalamityMod:HeavenlyGaleTrail"].SetMiscShaderAsset_1(ModContent.Request<Texture2D>(CWRConstant.Placeholder));
            GameShaders.Misc["CalamityMod:HeavenlyGaleTrail"].UseImage2("Images/Extra_189");
            GameShaders.Misc["CalamityMod:HeavenlyGaleTrail"].UseColor(mainColor);
            GameShaders.Misc["CalamityMod:HeavenlyGaleTrail"].UseSecondaryColor(secondaryColor);
            GameShaders.Misc["CalamityMod:HeavenlyGaleTrail"].Apply();
            PrimitiveRenderer.RenderTrail(Projectile.oldPos, new PrimitiveSettings(PrimitiveWidthFunction, PrimitiveColorFunction
                , (float _) => Projectile.Size * 0.5f, smoothen: true, pixelate: false, GameShaders.Misc["CalamityMod:HeavenlyGaleTrail"]), 53);
        }

        public override bool PreDraw(ref Color color) {
            if (Projectile.ai[1] <= 0) {
                DrawTrild();
            }

            Texture2D mainValue = Projectile.T2DValue();
            Main.spriteBatch.Draw(mainValue, Projectile.Center - Main.screenPosition
                , null, Color.White with { R = (byte)(Projectile.localAI[1] * 255) } * (Projectile.alpha / 255f)
                , Projectile.rotation + (Projectile.velocity.X > 0 ? MathHelper.ToRadians(60) : MathHelper.ToRadians(-240))
                , mainValue.Size() / 2, Projectile.scale
                , Projectile.velocity.X > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
            return false;
        }
    }
}
