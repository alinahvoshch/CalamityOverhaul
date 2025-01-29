﻿using CalamityMod.Items.Weapons.Typeless;
using CalamityOverhaul.Content.Projectiles.Weapons.Typeless;
using CalamityOverhaul.Content.RemakeItems.Core;
using Terraria;
using Terraria.ModLoader;

namespace CalamityOverhaul.Content.RemakeItems.Typeless
{
    /// <summary>
    /// 星光之眼
    /// </summary>
    internal class RLunicEye : ItemOverride
    {
        public override bool DrawingInfo => false;
        public override int TargetID => ModContent.ItemType<LunicEye>();
        public override bool FormulaSubstitution => true;
        public override void SetDefaults(Item item) => item.SetHeldProj<LunicEyeHeldProj>();
    }

    /// <summary>
    /// 马格努斯之眼
    /// </summary>
    internal class REyeofMagnus : ItemOverride
    {
        public override bool DrawingInfo => false;
        public override int TargetID => ModContent.ItemType<EyeofMagnus>();
        public override bool FormulaSubstitution => true;
        public override void SetDefaults(Item item) => item.SetHeldProj<EyeofMagnusHeldProj>();
    }

    /// <summary>
    /// 美学魔杖
    /// </summary>
    internal class RAestheticus : ItemOverride
    {
        public override bool DrawingInfo => false;
        public override int TargetID => ModContent.ItemType<Aestheticus>();
        public override bool FormulaSubstitution => true;
        public override void SetDefaults(Item item) => item.SetHeldProj<AestheticusHeldProj>();
    }
}
