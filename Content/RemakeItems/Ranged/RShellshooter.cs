﻿using CalamityMod.Items.Weapons.Ranged;
using CalamityOverhaul.Content.Items.Ranged;
using CalamityOverhaul.Content.Projectiles.Weapons.Ranged.HeldProjs;
using CalamityOverhaul.Content.RemakeItems.Core;
using Terraria;
using Terraria.ModLoader;

namespace CalamityOverhaul.Content.RemakeItems.Ranged
{
    internal class RShellshooter : BaseRItem
    {
        public override int TargetID => ModContent.ItemType<Shellshooter>();
        public override int ProtogenesisID => ModContent.ItemType<ShellshooterEcType>();
        public override string TargetToolTipItemName => "ShellshooterEcType";
        public override void SetDefaults(Item item) => item.SetHeldProj<ShellshooterHeldProj>();
    }
}
