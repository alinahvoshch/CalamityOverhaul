﻿using CalamityMod.Items.Weapons.Ranged;
using CalamityOverhaul.Content.Items.Ranged;
using CalamityOverhaul.Content.Projectiles.Weapons.Ranged.Core;
using Terraria.ModLoader;

namespace CalamityOverhaul.Content.Projectiles.Weapons.Ranged.HeldProjs
{
    internal class HoarfrostBowHeldProj : BaseBow
    {
        public override string Texture => CWRConstant.Cay_Wap_Ranged + "HoarfrostBow";
        public override int targetCayItem => ModContent.ItemType<HoarfrostBow>();
        public override int targetCWRItem => ModContent.ItemType<HoarfrostBowEcType>();
        public override void SetRangedProperty() {
            BowArrowDrawNum = 2;
            BowstringData.DeductRectangle = new Rectangle(4, 14, 2, 34);
        }
        public override void BowShoot() => OrigItemShoot();
    }
}
