﻿using CalamityMod.Items.Weapons.Ranged;
using CalamityOverhaul.Content.Items.Ranged;
using CalamityOverhaul.Content.Projectiles.Weapons.Ranged.HeldProjs;
using CalamityOverhaul.Content.RemakeItems.Core;
using Terraria.ModLoader;
using Terraria;

namespace CalamityOverhaul.Content.RemakeItems.Ranged
{
    internal class RInfinity : BaseRItem
    {
        public override int TargetID => ModContent.ItemType<Infinity>();
        public override int ProtogenesisID => ModContent.ItemType<InfinityEcType>();
        public override string TargetToolTipItemName => "InfinityEcType";
        public override void SetDefaults(Item item) => item.SetCartridgeGun<InfinityHeldProj>(900);
    }
}
