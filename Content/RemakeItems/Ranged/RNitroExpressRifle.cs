﻿using CalamityMod.Items.Weapons.Ranged;
using CalamityOverhaul.Content.Items.Ranged;
using CalamityOverhaul.Content.Projectiles.Weapons.Ranged.HeldProjs;
using CalamityOverhaul.Content.RemakeItems.Core;
using Terraria;
using Terraria.ModLoader;

namespace CalamityOverhaul.Content.RemakeItems.Ranged
{
    internal class RNitroExpressRifle : ItemOverride
    {
        public override int TargetID => ModContent.ItemType<NitroExpressRifle>();
        public override int ProtogenesisID => ModContent.ItemType<NitroExpressRifleEcType>();
        public override string TargetToolTipItemName => "NitroExpressRifleEcType";
        public override void SetDefaults(Item item) => item.SetCartridgeGun<NitroExpressRifleHeldProj>(8);
    }
}
