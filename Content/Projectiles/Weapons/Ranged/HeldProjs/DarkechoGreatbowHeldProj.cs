﻿using CalamityMod.Items.Weapons.Ranged;
using CalamityOverhaul.Content.Items.Ranged;
using CalamityOverhaul.Content.Projectiles.Weapons.Ranged.Core;
using Terraria.ModLoader;

namespace CalamityOverhaul.Content.Projectiles.Weapons.Ranged.HeldProjs
{
    internal class DarkechoGreatbowHeldProj : BaseBow
    {
        public override string Texture => CWRConstant.Cay_Wap_Ranged + "DarkechoGreatbow";
        public override int targetCayItem => ModContent.ItemType<DarkechoGreatbow>();
        public override int targetCWRItem => ModContent.ItemType<DarkechoGreatbowEcType>();
        public override void SetRangedProperty() {
            BowArrowDrawNum = 2;
            HandDistance = 16;
            HandFireDistance = 16;
            DrawArrowMode = -24;
            BowstringData.DeductRectangle = new Rectangle(2, 6, 2, 54);
        }
        public override void BowShoot() {
            OrigItemShoot();
        }
    }
}
