﻿using CalamityOverhaul.Content.RangedModify.Core;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.ID;

namespace CalamityOverhaul.Content.Projectiles.Weapons.Ranged.HeldProjs.Vanilla
{
    internal class BloodRainBowHeldProj : BaseBow
    {
        public override string Texture => CWRConstant.Placeholder;
        public override Texture2D TextureValue => TextureAssets.Item[ItemID.BloodRainBow].Value;
        public override int TargetID => ItemID.BloodRainBow;
        public override void SetRangedProperty() => BowstringData.DeductRectangle = new Rectangle(6, 12, 2, 20);
        public override void PostInOwner() {
            if (onFire) {
                LimitingAngle();
            }
        }
        public override void BowShoot() => OrigItemShoot();
    }
}
