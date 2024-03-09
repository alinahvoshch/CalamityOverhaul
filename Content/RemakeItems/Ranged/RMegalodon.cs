﻿using CalamityMod.Items.Weapons.Ranged;
using CalamityOverhaul.Content.Items.Ranged;
using CalamityOverhaul.Content.Projectiles.Weapons.Ranged.HeldProjs;
using CalamityOverhaul.Content.RemakeItems.Core;
using Terraria.ModLoader;
using Terraria;

namespace CalamityOverhaul.Content.RemakeItems.Ranged
{
    internal class RMegalodon : BaseRItem
    {
        public override int TargetID => ModContent.ItemType<Megalodon>();
        public override int ProtogenesisID => ModContent.ItemType<MegalodonEcType>();
        public override string TargetToolTipItemName => "MegalodonEcType";
        public override void SetDefaults(Item item) => item.SetCartridgeGun<MegalodonHeldProj>(220);
    }
}
