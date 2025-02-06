﻿using CalamityOverhaul.Content.RangedModify.Core;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;

namespace CalamityOverhaul.Content.Projectiles.Weapons.Ranged.HeldProjs.Vanilla
{
    internal class OrichalcumRepeaterHeldProj : BaseGun
    {
        public override string Texture => CWRConstant.Placeholder;
        public override Texture2D TextureValue => TextureAssets.Item[ItemID.OrichalcumRepeater].Value;
        public override int TargetID => ItemID.OrichalcumRepeater;
        public override void SetRangedProperty() {
            ShootPosToMouLengValue = 0;
            ShootPosNorlLengValue = 0;
            HandIdleDistanceX = 15;
            HandIdleDistanceY = 0;
            GunPressure = 0;
            ControlForce = 0;
            Recoil = 0;
            DrawCrossArrowToMode = -3;
            DrawCrossArrowNum = 2;
            IsCrossbow = true;
        }

        public override void FiringShoot() {
            float angle = Main.rand.NextFloat(0.05f, 0.09f);
            for (int i = 0; i < 2; i++) {
                int proj = Projectile.NewProjectile(Source, ShootPos, ShootVelocity.RotatedBy(MathHelper.Lerp(-angle, angle, i))
                    , AmmoTypes, WeaponDamage, WeaponKnockback, Owner.whoAmI, 0);
                _ = UpdateConsumeAmmo();
                CWRUtils.SetArrowRot(proj);
            }
        }
    }
}
