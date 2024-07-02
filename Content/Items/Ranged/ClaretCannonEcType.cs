﻿using CalamityMod.Items.Weapons.Ranged;
using CalamityOverhaul.Content.Projectiles.Weapons.Ranged.HeldProjs;

namespace CalamityOverhaul.Content.Items.Ranged
{
    internal class ClaretCannonEcType : EctypeItem
    {
        public override string Texture => CWRConstant.Cay_Wap_Ranged + "ClaretCannon";
        public override void SetDefaults() {
            Item.SetCalamitySD<ClaretCannon>();
            Item.SetCartridgeGun<ClaretCannonHeldProj>(48);
        }
    }
}
