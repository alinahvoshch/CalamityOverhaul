﻿using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityOverhaul.Content.Projectiles.Boss.SkeletronPrime
{
    internal class PrimeCannonOnSpan : ModProjectile
    {
        public override string Texture => CWRConstant.Projectile + "DeathLaser";
        private int scaleTimer = 0;
        private int scaleIndex = 0;
        private bool formeNPC;
        public override void SetStaticDefaults() => ProjectileID.Sets.DrawScreenCheckFluff[Type] = 2000;
        public override void SetDefaults() {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.hostile = true;
            Projectile.scale = 1f;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 60;
            Projectile.alpha = 0;
        }

        public override void AI() {
            NPC npc = CWRUtils.GetNPCInstance((int)Projectile.ai[0]);
            if (Projectile.localAI[0] == 0) {
                if (npc.Alives()) {
                    formeNPC = true;
                }
                Projectile.rotation = Projectile.velocity.ToRotation();
            }

            if (npc.Alives()) {
                Projectile.Center = npc.Center;
            }
            else if (formeNPC) {
                Projectile.Kill();
            }

            Player player = CWRUtils.GetPlayerInstance((int)Projectile.ai[1]);
            if (player.Alives()) {
                Vector2 toSet = Projectile.Center.To(player.Center);
                Projectile.EntityToRot(toSet.ToRotation() + Projectile.ai[2], 0.03f);
            }

            Projectile.scale += 0.05f;
            if (Projectile.alpha < 255) {
                Projectile.alpha += 15;
            }

            if (scaleTimer < 8 && scaleIndex == 0) {
                scaleTimer++;
            }
            if (Projectile.timeLeft < 30) {
                scaleIndex = 1;
            }
            if (scaleIndex > 0) {
                scaleTimer--;
            }

            Projectile.localAI[0]++;
        }

        public override void OnKill(int timeLeft) {
            SoundEngine.PlaySound(SoundID.Item62, Projectile.Center);
            if (!VaultUtils.isClient) {
                Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Projectile.rotation.ToRotationVector2() * 13
                        , ModContent.ProjectileType<RocketSkeleton>(), Projectile.damage, 0f, Main.myPlayer, Projectile.ai[1], 2);
            }
        }

        public override bool PreDraw(ref Color lightColor) {
            if (scaleTimer >= 0) {
                Texture2D tex = CWRUtils.GetT2DValue(CWRConstant.Masking + "MaskLaserLine");
                Color drawColor = Color.White;
                drawColor.A = 0;
                Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition, null, drawColor
                    , Projectile.rotation, new Vector2(0, tex.Height / 2f), new Vector2(1000, scaleTimer * 0.04f), SpriteEffects.None, 0);
            }
            return false;
        }
    }
}
