﻿using CalamityMod.Items.Weapons.Ranged;
using CalamityOverhaul.Content.Projectiles.Weapons.Ranged.HeldProjs;

namespace CalamityOverhaul.Content.Items.Ranged
{
    internal class SpykerEcType : EctypeItem
    {
        public override string Texture => CWRConstant.Cay_Wap_Ranged + "Spyker";
        public override void SetDefaults() {
            Item.SetCalamitySD<Spyker>();
            Item.SetCartridgeGun<SpykerHeldProj>(60);
        }
    }
}
