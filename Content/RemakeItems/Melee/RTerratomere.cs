﻿using CalamityMod.Items;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Rarities;
using CalamityOverhaul.Content.Items.Melee;
using CalamityOverhaul.Content.RemakeItems.Core;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityOverhaul.Content.RemakeItems.Melee
{
    internal class RTerratomere : BaseRItem
    {
        public override int TargetID => ModContent.ItemType<Terratomere>();
        public override int ProtogenesisID => ModContent.ItemType<TerratomereEcType>();
        public override string TargetToolTipItemName => "TerratomereEcType";
        public override void SetDefaults(Item item) {
            item.width = 60;
            item.height = 66;
            item.damage = 230;
            item.DamageType = DamageClass.Melee;
            item.useAnimation = 21;
            item.useTime = 21;
            item.useStyle = ItemUseStyleID.Swing;
            item.useTurn = true;
            item.UseSound = null;
            item.knockBack = 7f;
            item.autoReuse = true;
            item.noUseGraphic = true;
            item.noMelee = true;
            item.value = CalamityGlobalItem.RarityTurquoiseBuyPrice;
            item.rare = ModContent.RarityType<Turquoise>();
            item.SetKnifeHeld<TerratomereHeld>();
        }
    }
}
