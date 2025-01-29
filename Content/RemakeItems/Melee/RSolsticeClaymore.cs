﻿using CalamityMod.Items.Weapons.Melee;
using CalamityOverhaul.Content.Items.Melee;
using CalamityOverhaul.Content.RemakeItems.Core;
using Terraria;


using Terraria.ModLoader;

namespace CalamityOverhaul.Content.RemakeItems.Melee
{
    internal class RSolsticeClaymore : ItemOverride
    {
        public override int TargetID => ModContent.ItemType<SolsticeClaymore>();
        public override void SetDefaults(Item item) => SolsticeClaymoreEcType.SetDefaultsFunc(item);
    }
}
