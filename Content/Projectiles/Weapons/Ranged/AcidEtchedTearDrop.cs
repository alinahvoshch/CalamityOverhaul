﻿using CalamityMod;
using CalamityMod.Buffs.StatDebuffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityOverhaul.Content.Projectiles.Weapons.Ranged
{
    internal class AcidEtchedTearDrop : ModProjectile
    {
        public override string Texture => CWRConstant.Projectile + "AcidEtchedTearDrop";

        public override void SetStaticDefaults() {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults() {
            Projectile.width = 14;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = false;
            Projectile.timeLeft = 300;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 17;
        }

        public override void AI() {
            NPC potentialTarget = Projectile.Center.ClosestNPCAt(600f, !Projectile.tileCollide);
            if (potentialTarget is not null) {
                float flySpeed = Projectile.velocity.Length();
                if (flySpeed < 5f)
                    flySpeed = 5f;

                Projectile.velocity = Vector2.Lerp(Projectile.velocity, Projectile.SafeDirectionTo(potentialTarget.Center) * flySpeed, 0.085f);
            }

            Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.PiOver2;
            Projectile.tileCollide = Projectile.timeLeft <= 300;
        }

        public override bool OnTileCollide(Vector2 oldVelocity) {
            for (int i = 0; i < 4; i++) {
                int idx = Dust.NewDust(Projectile.position - Projectile.velocity, 2, 2, DustID.Rain, 0f, 0f, 0, new Color(112, 150, 42, 127), 1f);
                Dust dust = Main.dust[idx];
                dust.position.X -= 2f;
                dust.alpha = 38;
                dust.velocity *= 0.1f;
                dust.velocity -= Projectile.velocity * 0.025f;
                dust.scale = 2f;
            }
            return true;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) => target.AddBuff(ModContent.BuffType<Irradiated>(), 180);

        public override bool PreDraw(ref Color lightColor) {
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], new Color(255, 255, 255, 127), 2);
            return false;
        }
    }
}
