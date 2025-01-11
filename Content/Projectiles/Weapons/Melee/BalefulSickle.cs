﻿using CalamityMod;
using CalamityOverhaul.Content.Items.Melee;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityOverhaul.Content.Projectiles.Weapons.Melee
{
    internal class BalefulSickle : ModProjectile
    {
        public override string Texture => CWRConstant.Projectile_Melee + "BalefulSickle";
        int oldRotDir;
        public override void SetStaticDefaults() {
            ProjectileID.Sets.TrailCacheLength[Type] = 6;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }
        public override void SetDefaults() {
            Projectile.DamageType = DamageClass.Melee;
            Projectile.width = Projectile.height = 52;
            Projectile.friendly = true;
            Projectile.penetrate = 6;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.MaxUpdates = 2;
            Projectile.timeLeft = 120 * Projectile.MaxUpdates;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 15 * Projectile.MaxUpdates;
        }

        public override void PostAI() {
            int rotDir = Math.Sign(Projectile.velocity.X);
            if (rotDir == 0) {
                rotDir = oldRotDir;
            }
            else {
                oldRotDir = rotDir;
            }
            Projectile.rotation += rotDir * (Projectile.ai[0] + 0.1f);
            Projectile.ai[0] += 0.01f;
            if (Projectile.ai[0] > 0.5f) {
                Projectile.ai[0] = 0.5f;
            }
            Projectile.velocity *= 0.97f;

            if (Projectile.timeLeft < 40) {
                NPC target = Projectile.Center.FindClosestNPC(650);
                if (target != null) {
                    Projectile.ChasingBehavior(target.Center, 30f);
                }
            }

            Lighting.AddLight(Projectile.Center, Color.DarkGray.ToVector3());
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
            target.AddBuff(BuffID.OnFire3, 240);
            BalefulHarvesterEcType.SpanDust(Projectile.Center, 13, 0.7f, 1.2f);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info) {
            target.AddBuff(BuffID.OnFire3, 240);
        }

        public override void OnKill(int timeLeft) {
            SoundEngine.PlaySound(SoundID.Dig, Projectile.position);
            for (int j = 0; j < 5; j++) {
                int deathDust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Blood, 0f, 0f, 100, default, 2f);
                Main.dust[deathDust].velocity *= 3f;
                if (Main.rand.NextBool()) {
                    Main.dust[deathDust].scale = 0.5f;
                    Main.dust[deathDust].fadeIn = 1f + Main.rand.Next(10) * 0.1f;
                }
            }
            for (int k = 0; k < 10; k++) {
                int deathDust2 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Torch, 0f, 0f, 100, default, 3f);
                Main.dust[deathDust2].noGravity = true;
                Main.dust[deathDust2].velocity *= 5f;
                deathDust2 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Torch, 0f, 0f, 100, default, 2f);
                Main.dust[deathDust2].velocity *= 2f;
            }
        }

        public override bool PreDraw(ref Color lightColor) {
            Texture2D value = CWRUtils.GetT2DValue(Texture);

            Main.spriteBatch.Draw(value, Projectile.Center - Main.screenPosition, null, Color.DarkRed
                , Projectile.rotation + (Projectile.ai[2] == 0 ? MathHelper.PiOver2 : 0), value.Size() / 2
                , Projectile.scale, Projectile.velocity.X > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);

            Texture2D value2 = ModContent.Request<Texture2D>("CalamityMod/Particles/SemiCircularSmear").Value;
            Main.spriteBatch.EnterShaderRegion(BlendState.Additive);
            Main.EntitySpriteDraw(color: Color.IndianRed
                , origin: value2.Size() * 0.5f, texture: value2, position: Projectile.Center - Main.screenPosition
                , sourceRectangle: null, rotation: Projectile.rotation - CWRUtils.PiOver5
                , scale: Projectile.scale * 0.5f, effects: SpriteEffects.None);
            Main.spriteBatch.ExitShaderRegion();

            return false;
        }
    }
}
