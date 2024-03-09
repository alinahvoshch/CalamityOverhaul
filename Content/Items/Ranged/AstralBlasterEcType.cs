﻿using CalamityMod.Items.Weapons.Ranged;
using CalamityOverhaul.Common;
using CalamityOverhaul.Content.Projectiles.Weapons.Ranged.HeldProjs;

namespace CalamityOverhaul.Content.Items.Ranged
{
    internal class AstralBlasterEcType : EctypeItem
    {
        public override string Texture => CWRConstant.Cay_Wap_Ranged + "AstralBlaster";
        public override void SetDefaults() {
            Item.SetCalamityGunSD<AstralBlaster>();
            Item.SetCartridgeGun<AstralBlasterHeldProj>(30);
        }
    }
}
