﻿using CalamityMod.Items.Weapons.Ranged;
using CalamityOverhaul.Content.Projectiles.Weapons.Ranged.HeldProjs;
using CalamityOverhaul.Content.RemakeItems.Core;
using Terraria;
using Terraria.ModLoader;

namespace CalamityOverhaul.Content.RemakeItems.Ranged
{
    internal class RDrataliornus : ItemOverride
    {
        public override int TargetID => ModContent.ItemType<Drataliornus>();
        public override void SetDefaults(Item item) {
            item.damage = 136;
            item.SetHeldProj<DrataliornusHeldProj>();
        }
    }
}
