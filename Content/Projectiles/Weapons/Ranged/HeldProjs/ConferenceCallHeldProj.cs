﻿using CalamityMod.Items.Weapons.Ranged;
using CalamityOverhaul.Content.RangedModify.Core;
using Terraria;
using Terraria.ModLoader;

namespace CalamityOverhaul.Content.Projectiles.Weapons.Ranged.HeldProjs
{
    internal class ConferenceCallHeldProj : BaseFeederGun
    {
        public override string Texture => CWRConstant.Cay_Wap_Ranged + "ConferenceCall";
        public override int TargetID => ModContent.ItemType<ConferenceCall>();
        public override void SetRangedProperty() {
            KreloadMaxTime = 90;
            FireTime = 25;
            HandIdleDistanceX = 20;
            HandIdleDistanceY = 5;
            HandFireDistanceX = 20;
            HandFireDistanceY = -6;
            ShootPosNorlLengValue = -0;
            ShootPosToMouLengValue = 10;
            RepeatedCartridgeChange = true;
            GunPressure = 0.2f;
            ControlForce = 0.05f;
            Recoil = 1.2f;
            RangeOfStress = 25;
            EnableRecoilRetroEffect = true;
            RecoilRetroForceMagnitude = 6;
        }

        public override void FiringShoot() {
            for (int index = 0; index < 5; ++index) {
                Vector2 velocity = ShootVelocity;
                velocity.X += Main.rand.Next(-20, 21) * 0.05f;
                velocity.Y += Main.rand.Next(-20, 21) * 0.05f;
                Projectile.NewProjectile(Source, ShootPos, velocity, AmmoTypes, WeaponDamage, WeaponKnockback, Owner.whoAmI, 0);
            }
            NPC target = Projectile.Center.FindClosestNPC(1200, false, true);
            if (target != null) {
                for (int index = 0; index < 3; ++index) {
                    Vector2 spanPos = new Vector2(Main.rand.Next(500) * (ShootVelocity.X < 0 ? 1 : -1), -900) + Projectile.Center;
                    Vector2 velocity = spanPos.To(target.Center).UnitVector() * ShootSpeedModeFactor;
                    Projectile.NewProjectile(Source, spanPos, velocity, AmmoTypes, WeaponDamage, WeaponKnockback, Owner.whoAmI, 0);
                }
            }
        }
    }
}
