﻿using CalamityMod.Items.Weapons.Ranged;
using CalamityOverhaul.Content.Projectiles.Weapons.Ranged.HeldProjs;
using CalamityOverhaul.Content.RemakeItems.Core;
using Terraria;
using Terraria.ModLoader;

namespace CalamityOverhaul.Content.RemakeItems.Ranged
{
    internal class RTelluricGlare : ItemOverride
    {
        public override int TargetID => ModContent.ItemType<TelluricGlare>();
        public override void SetDefaults(Item item) {
            item.damage = 180;
            item.SetHeldProj<TelluricGlareHeldProj>();
        }
    }
}
