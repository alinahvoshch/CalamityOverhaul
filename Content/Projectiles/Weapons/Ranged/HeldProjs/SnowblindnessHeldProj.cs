﻿using CalamityMod;
using CalamityOverhaul.Common;
using CalamityOverhaul.Content.Items.Ranged.Extras;
using CalamityOverhaul.Content.Projectiles.Weapons.Ranged.Core;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityOverhaul.Content.Projectiles.Weapons.Ranged.HeldProjs
{
    internal class SnowblindnessHeldProj : BaseFeederGun
    {
        public override string Texture => CWRConstant.Item_Ranged + "Snowblindness";
        public override int targetCayItem => ModContent.ItemType<Snowblindness>();
        public override int targetCWRItem => ModContent.ItemType<Snowblindness>();
        public override void SetRangedProperty() {
            Recoil = 0.45f;
            FireTime = 4;
            kreloadMaxTime = 60;
            HandDistance = 40;
            HandDistanceY = 10;
            HandFireDistance = 40;
            HandFireDistanceY = 2;
            RecoilRetroForceMagnitude = 6;
            RecoilOffsetRecoverValue = 0.6f;
            RangeOfStress = 50;
            ShootPosNorlLengValue = -10;
            ShootPosToMouLengValue = 20;
            EnableRecoilRetroEffect = true;
            LoadingAmmoAnimation = LoadingAmmoAnimationEnum.Handgun;
            LoadingAA_Handgun.clipLocked = CWRSound.Gun_Snowblindness_Clipin;
            LoadingAA_Handgun.clipOut = CWRSound.Gun_Snowblindness_Clipout;
            LoadingAA_Handgun.Roting = -30;
            LoadingAA_Handgun.gunBodyX = -8;
            LoadingAA_Handgun.gunBodyY = -16;
            CanCreateCaseEjection = false;
            SpwanGunDustMngsData.dustID1 = 76;
            SpwanGunDustMngsData.dustID2 = 149;
            SpwanGunDustMngsData.dustID3 = 76;
        }
        public override void FiringShoot() {
            int proj = Projectile.NewProjectile(Source, GunShootPos, ShootVelocity.RotatedByRandom(0.1f), AmmoTypes, WeaponDamage, WeaponKnockback, Owner.whoAmI, 0, 1);
            Main.projectile[proj].Calamity().allProjectilesHome = true;
            Main.projectile[proj].CWR().GetHitAttribute.SuperAttack = true;
            Main.projectile[proj].extraUpdates = 1;
            Main.projectile[proj].usesLocalNPCImmunity = true;
            Main.projectile[proj].localNPCHitCooldown = -1;
            int bolt = ProjectileID.IceBolt;
            bool isbeam = false;
            if (Main.rand.NextBool(3)) {
                bolt = ProjectileID.FrostBeam;
                isbeam = true;
            }
            int proj2 = Projectile.NewProjectile(Source, GunShootPos, ShootVelocity, bolt, WeaponDamage, WeaponKnockback, Owner.whoAmI, 0, 1);
            Main.projectile[proj2].extraUpdates = 1;
            Main.projectile[proj2].friendly = true;
            Main.projectile[proj2].hostile = false;
            Main.projectile[proj2].DamageType = DamageClass.Ranged;
            if (isbeam) {
                Main.projectile[proj2].damage *= 2;
                Main.projectile[proj2].usesLocalNPCImmunity = true;
                Main.projectile[proj2].localNPCHitCooldown = -1;
                Main.projectile[proj2].ArmorPenetration = 50;
            }
        }
    }
}
