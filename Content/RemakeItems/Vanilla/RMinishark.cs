﻿using CalamityOverhaul.Common;
using CalamityOverhaul.Content.Projectiles.Weapons.Ranged.HeldProjs;
using CalamityOverhaul.Content.RemakeItems.Core;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityOverhaul.Content.RemakeItems.Vanilla
{
    internal class RMinishark : ItemOverride
    {
        public override int TargetID => ItemID.Minishark;
        public override bool FormulaSubstitution => false;
        public override void SetDefaults(Item item) => item.SetCartridgeGun<MinisharkHeldProj>(160);
    }
}
