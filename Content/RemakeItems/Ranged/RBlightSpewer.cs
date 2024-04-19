﻿using CalamityOverhaul.Content.Items.Ranged;
using CalamityOverhaul.Content.Projectiles.Weapons.Ranged.HeldProjs;
using CalamityOverhaul.Content.RemakeItems.Core;
using Terraria.ModLoader;
using Terraria;
using CalamityMod.Items.Weapons.Ranged;

namespace CalamityOverhaul.Content.RemakeItems.Ranged
{
    internal class RBlightSpewer : BaseRItem
    {
        public override int TargetID => ModContent.ItemType<BlightSpewer>();
        public override int ProtogenesisID => ModContent.ItemType<BlightSpewerEcType>();
        public override string TargetToolTipItemName => "BlightSpewerEcType";
        public override void SetDefaults(Item item) {
            item.SetCartridgeGun<BlightSpewerHeldProj>(160);
            item.CWR().CartridgeEnum = CartridgeUIEnum.JAR;
        }
    }
}
