﻿using CalamityMod.Items.Weapons.Ranged;
using CalamityOverhaul.Content.Items.Ranged;
using CalamityOverhaul.Content.Projectiles.Weapons.Ranged.HeldProjs;
using CalamityOverhaul.Content.RemakeItems.Core;
using Terraria;
using Terraria.ModLoader;

namespace CalamityOverhaul.Content.RemakeItems.Ranged
{
    internal class RTheBallista : ItemOverride
    {
        public override int TargetID => ModContent.ItemType<TheBallista>();
        public override int ProtogenesisID => ModContent.ItemType<TheBallistaEcType>();
        public override string TargetToolTipItemName => "TheBallistaEcType";
        public override void SetDefaults(Item item) => item.SetHeldProj<TheBallistaHeldProj>();
    }
}
