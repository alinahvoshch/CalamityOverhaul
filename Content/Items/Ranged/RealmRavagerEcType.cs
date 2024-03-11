﻿using CalamityMod.Items.Weapons.Ranged;
using CalamityOverhaul.Common;
using CalamityOverhaul.Content.Projectiles.Weapons.Ranged.HeldProjs;

namespace CalamityOverhaul.Content.Items.Ranged
{
    internal class RealmRavagerEcType : EctypeItem
    {
        public override string Texture => CWRConstant.Cay_Wap_Ranged + "RealmRavager";
        public override void SetDefaults() {
            Item.SetCalamitySD<RealmRavager>();
            Item.SetCartridgeGun<RealmRavagerHeldProj>(180);
        }
    }
}
