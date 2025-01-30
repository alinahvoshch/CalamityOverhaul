﻿using CalamityMod.Items.Weapons.Ranged;
using CalamityOverhaul.Content.Projectiles.Weapons.Ranged.HeldProjs;
using CalamityOverhaul.Content.RemakeItems.Core;
using Terraria;
using Terraria.ModLoader;

namespace CalamityOverhaul.Content.RemakeItems.Ranged
{
    internal class RMineralMortar : ItemOverride
    {
        public override int TargetID => ModContent.ItemType<MineralMortar>();
        public override void SetDefaults(Item item) => item.SetCartridgeGun<MineralMortarHeldProj>(8);
    }
}
