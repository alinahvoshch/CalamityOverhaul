﻿using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Projectiles.Ranged;
using CalamityOverhaul.Common;
using CalamityOverhaul.Content.Items.Ranged;
using CalamityOverhaul.Content.Projectiles.Weapons.Ranged.Core;
using Terraria.ModLoader;

namespace CalamityOverhaul.Content.Projectiles.Weapons.Ranged.HeldProjs
{
    internal class BlightSpewerHeldProj : BaseFeederGun
    {
        public override string Texture => CWRConstant.Cay_Wap_Ranged + "BlightSpewer";
        public override int targetCayItem => ModContent.ItemType<BlightSpewer>();
        public override int targetCWRItem => ModContent.ItemType<BlightSpewerEcType>();
        public override void SetRangedProperty() {
            FireTime = 15;
            HandDistance = 25;
            HandDistanceY = 4;
            HandFireDistance = 25;
            HandFireDistanceY = -5;
            ShootPosNorlLengValue = -3;
            ShootPosToMouLengValue = 0;
            GunPressure = 0;
            ControlForce = 0;
            Recoil = 0;
            RangeOfStress = 28;
            RepeatedCartridgeChange = true;
            CanCreateCaseEjection = CanCreateSpawnGunDust = false;
            kreloadMaxTime = 90;
            loadTheRounds = CWRSound.Liquids_Fill_0 with { Pitch = -0.8f };
            ForcedConversionTargetAmmoFunc = () => true;
            ToTargetAmmo = ModContent.ProjectileType<BlightFlames>();
            LoadingAA_None.Roting = 30;
            LoadingAA_None.gunBodyX = 0;
            LoadingAA_None.gunBodyY = 12;
        }
    }
}
