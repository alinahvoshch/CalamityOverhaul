﻿using CalamityMod.Items.Weapons.Ranged;
using CalamityOverhaul.Content.Items.Ranged;
using CalamityOverhaul.Content.Projectiles.Weapons.Ranged.HeldProjs;
using CalamityOverhaul.Content.RemakeItems.Core;
using Terraria;
using Terraria.ModLoader;

namespace CalamityOverhaul.Content.RemakeItems.Ranged
{
    internal class RMolecularManipulator : ItemOverride
    {
        public override int TargetID => ModContent.ItemType<MolecularManipulator>();
        public override int ProtogenesisID => ModContent.ItemType<MolecularManipulatorEcType>();
        public override string TargetToolTipItemName => "MolecularManipulatorEcType";
        public override void SetDefaults(Item item) => item.SetCartridgeGun<MolecularManipulatorHeldProj>(480);
    }
}
