﻿using CalamityOverhaul.Common;
using CalamityOverhaul.Content.RemakeItems.Vanilla;
using CalamityMod.Sounds;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using CalamityMod.Projectiles.Melee;

namespace CalamityOverhaul.Content.Projectiles.Weapons.Ranged.HeldProjs.Vanilla
{
    internal class SniperRifleHeldProj : BaseFeederGun
    {
        public override string Texture => CWRConstant.Placeholder;
        public override Texture2D TextureValue => TextureAssets.Item[ItemID.SniperRifle].Value;
        public override int targetCayItem => ItemID.SniperRifle;
        public override int targetCWRItem => ItemID.SniperRifle;
        public static SoundStyle ShootSound = new("CalamityMod/Sounds/Item/TankCannon") { PitchVariance = 0.5f };
        public override void SetRangedProperty() {
            FireTime = 60;
            ShootPosToMouLengValue = 0;
            ShootPosNorlLengValue = -2;
            HandDistance = 15;
            HandDistanceY = 0;
            GunPressure = 0.2f;
            ControlForce = 0.05f;
            Recoil = 1f;
            RangeOfStress = 48;
            kreloadMaxTime = 120;
            ShootSound = new("CalamityMod/Sounds/Item/TankCannon") { Pitch = 0.5f };
        }

        public override void PreInOwnerUpdate() {
            LoadingAnimation(30, 0, 13);
        }

        public override void FiringShoot() {
            SoundEngine.PlaySound(ShootSound, Projectile.Center);
            if (AmmoTypes == ProjectileID.Bullet) {
                AmmoTypes = ProjectileID.BulletHighVelocity;
            }
            SpawnGunFireDust(GunShootPos, ShootVelocity);
            if (BulletNum == 3 | BulletNum == 2) {
                int proj = Projectile.NewProjectile(Source, GunShootPos, ShootVelocity, AmmoTypes, WeaponDamage, WeaponKnockback, Owner.whoAmI, 0);
                if (Main.projectile[proj].penetrate > 1) {
                    Main.projectile[proj].penetrate = 1;
                }
            }
            if (BulletNum == 1) {
                int proj = Projectile.NewProjectile(Source, GunShootPos, ShootVelocity, AmmoTypes, WeaponDamage, WeaponKnockback, Owner.whoAmI, 0);
                if (Main.projectile[proj].penetrate > 1) {
                    Main.projectile[proj].penetrate = 1;
                }
                FireTime = 90;
                Item.crit = 100;
            }
            if (BulletNum == 0) {
                int proj1 = Projectile.NewProjectile(Source, GunShootPos, ShootVelocity, AmmoTypes, WeaponDamage * 2, WeaponKnockback * 2, Owner.whoAmI, 0);
                Main.projectile[proj1].extraUpdates += 3;
                if (Main.projectile[proj1].penetrate > 1) {
                    Main.projectile[proj1].penetrate = 1;
                    Item.crit = -1000;
                    FireTime = 60;
                }
            }
        }
    }
}
