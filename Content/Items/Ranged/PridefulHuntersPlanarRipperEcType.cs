﻿using CalamityMod.Items.Weapons.Ranged;
using CalamityOverhaul.Common;
using CalamityOverhaul.Content.Projectiles.Weapons.Ranged.HeldProjs;

namespace CalamityOverhaul.Content.Items.Ranged
{
    internal class PridefulHuntersPlanarRipperEcType : EctypeItem
    {
        public override string Texture => CWRConstant.Cay_Wap_Ranged + "PridefulHuntersPlanarRipper";
        public override void SetDefaults() {
            Item.SetCalamitySD<PridefulHuntersPlanarRipper>();
            Item.damage = 33;
            Item.SetCartridgeGun<PridefulHuntersPlanarRipperHeldProj>(280);
        }
    }
}
