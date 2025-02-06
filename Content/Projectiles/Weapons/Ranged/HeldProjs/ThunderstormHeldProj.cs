﻿using CalamityMod.Items.Weapons.Magic;
using CalamityMod.Particles;
using CalamityOverhaul.Content.RangedModify.Core;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;

namespace CalamityOverhaul.Content.Projectiles.Weapons.Ranged.HeldProjs
{
    internal class ThunderstormHeldProj : BaseGun
    {
        public override string Texture => CWRConstant.Cay_Wap_Magic + "Thunderstorm";
        public override int TargetID => ModContent.ItemType<Thunderstorm>();
        public override void SetRangedProperty() {
            Projectile.DamageType = DamageClass.Magic;
            ControlForce = 0.03f;
            GunPressure = 0.15f;
            Recoil = 1.2f;
            HandIdleDistanceX = 20;
            HandIdleDistanceY = 5;
            ShootPosNorlLengValue = -6;
            ShootPosToMouLengValue = 25;
            FiringDefaultSound = false;
            CanCreateCaseEjection = false;
            CanCreateSpawnGunDust = false;
        }

        public override void FiringShoot() {
            if (Owner.CheckMana(Item)) {
                SoundEngine.PlaySound(Item.UseSound, Projectile.Center);
                for (int i = 0; i < 12; i++) {
                    int sparkLifetime = Main.rand.Next(22, 36);
                    float sparkScale = Main.rand.NextFloat(1f, 1.5f);
                    Color sparkColor = Color.LightGoldenrodYellow;
                    Vector2 sparkVelocity = ShootVelocity.RotatedBy(Main.rand.NextFloat(-0.1f, 0.1f)) * Main.rand.NextFloat(0.3f, 1.2f);
                    SparkParticle spark = new SparkParticle(ShootPos, sparkVelocity, false, sparkLifetime, sparkScale, sparkColor);
                    GeneralParticleHandler.SpawnParticle(spark);
                }

                Projectile.NewProjectile(Source, ShootPos, ShootVelocity, Item.shoot, WeaponDamage, WeaponKnockback, Owner.whoAmI, 0);

                Owner.statMana -= Item.mana;
                if (Owner.statMana < 0) {
                    Owner.statMana = 0;
                }
            }
        }
    }
}
