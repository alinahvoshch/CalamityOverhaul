﻿using CalamityOverhaul.Content.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityOverhaul.Content.Projectiles.Weapons.Melee.RebelBladeProj
{
    public class RebelBladeOrb : ModProjectile
    {
        public override string Texture => CWRConstant.Placeholder;
        public override void SetDefaults() {
            Projectile.width = Projectile.height = 22;
            Projectile.friendly = true;
            Projectile.timeLeft = 60;
            Projectile.tileCollide = false;
            Projectile.penetrate = 6;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;
        }

        public override void AI() {
            Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width
                , Projectile.height, DustID.FireworkFountain_Blue, 0, 0, 55, Main.DiscoColor);
            dust.noGravity = true;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
            target.AddBuff(BuffID.OnFire, 30);
            target.AddBuff(BuffID.OnFire3, 30);

            if (target.IsWormBody()) {
                Projectile.timeLeft = 1;
            }
            else {
                target.AddBuff(ModContent.BuffType<HellburnBuff>(), 30);
            }
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers) {
            if (target.IsWormBody()) {
                modifiers.FinalDamage /= 10;
            }
        }

        public override void OnKill(int timeLeft) {
            Projectile.Explode(66, SoundID.Item60 with { Pitch = 0.6f });
        }
    }
}
