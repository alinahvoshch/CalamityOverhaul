﻿using CalamityMod.Items.Weapons.Ranged;
using CalamityOverhaul.Content.RangedModify.Core;
using Terraria;
using Terraria.ModLoader;

namespace CalamityOverhaul.Content.Projectiles.Weapons.Ranged.HeldProjs
{
    internal class HandheldTankHeldProj : BaseFeederGun
    {
        public override string Texture => CWRConstant.Cay_Wap_Ranged + "HandheldTank";
        public override int TargetID => ModContent.ItemType<HandheldTank>();
        public override void SetRangedProperty() {
            FireTime = 30;
            KreloadMaxTime = 60;
            ShootPosToMouLengValue = 0;
            ShootPosNorlLengValue = 0;
            HandIdleDistanceX = 60;
            HandIdleDistanceY = 4;
            HandFireDistanceX = 60;
            ShootPosNorlLengValue = -6;
            ShootPosToMouLengValue = 25;
            GunPressure = 0.1f;
            ControlForce = 0.03f;
            EjectCasingProjSize = 2;
            Recoil = 3.5f;
            RangeOfStress = 28;
            RepeatedCartridgeChange = true;
            LoadingAA_None.Roting = 30;
            LoadingAA_None.gunBodyX = 0;
            LoadingAA_None.gunBodyY = 13;
        }

        public override void PostInOwner() {
            if (!DownLeft && kreloadTimeValue == 0) {
                ArmRotSengsFront = 70 * CWRUtils.atoR;
                ArmRotSengsBack = 110 * CWRUtils.atoR;
            }
        }

        public override void FiringShoot() {
            Projectile.NewProjectile(Source, ShootPos, ShootVelocity
                , Item.shoot, WeaponDamage, WeaponKnockback, Owner.whoAmI, 0);
        }
    }
}
