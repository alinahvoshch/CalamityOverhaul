﻿using CalamityMod.Items.Weapons.Ranged;
using CalamityOverhaul.Content.Items.Ranged;
using CalamityOverhaul.Content.Projectiles.Weapons.Ranged.Core;
using Terraria.ModLoader;

namespace CalamityOverhaul.Content.Projectiles.Weapons.Ranged.HeldProjs
{
    internal class ShellshooterHeldProj : BaseBow
    {
        public override string Texture => CWRConstant.Cay_Wap_Ranged + "Shellshooter";
        public override int targetCayItem => ModContent.ItemType<Shellshooter>();
        public override int targetCWRItem => ModContent.ItemType<ShellshooterEcType>();
        public override void SetRangedProperty() => BowstringData.DeductRectangle = new Rectangle(4, 14, 2, 18);
        public override void BowShoot() => OrigItemShoot();
    }
}
