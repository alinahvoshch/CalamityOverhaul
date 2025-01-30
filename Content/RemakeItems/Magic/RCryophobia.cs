﻿using CalamityMod.Items.Weapons.Magic;
using CalamityOverhaul.Content.Projectiles.Weapons.Magic.HeldProjs;
using CalamityOverhaul.Content.RemakeItems.Core;
using Terraria;
using Terraria.ModLoader;

namespace CalamityOverhaul.Content.RemakeItems.Magic
{
    internal class RCryophobia : ItemOverride
    {
        public override int TargetID => ModContent.ItemType<Cryophobia>();
        public override void SetDefaults(Item item) => item.SetHeldProj<CryophobiaHeldProj>();
    }
}
