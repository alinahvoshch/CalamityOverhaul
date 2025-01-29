﻿using CalamityMod.Items.Weapons.Melee;
using CalamityOverhaul.Content.Items.Melee;
using CalamityOverhaul.Content.RemakeItems.Core;
using Terraria;
using Terraria.ModLoader;

namespace CalamityOverhaul.Content.RemakeItems.Melee
{
    internal class RGreatswordofJudgement : ItemOverride
    {
        public override int TargetID => ModContent.ItemType<GreatswordofJudgement>();
 
        public override void SetDefaults(Item item) => GreatswordofJudgementEcType.SetDefaultsFunc(item);
    }
}
