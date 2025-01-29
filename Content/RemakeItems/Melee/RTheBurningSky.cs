﻿using CalamityMod.Items.Weapons.Melee;
using CalamityOverhaul.Content.Items.Melee;
using CalamityOverhaul.Content.RemakeItems.Core;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace CalamityOverhaul.Content.RemakeItems.Melee
{
    internal class RTheBurningSky : ItemOverride
    {
        public override int TargetID => ModContent.ItemType<TheBurningSky>();
        public override void SetDefaults(Item item) => TheBurningSkyEcType.SetDefaultsFunc(item);
        public override bool? Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source
            , Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
            return TheBurningSkyEcType.ShootFunc(item, player, source, position, velocity, type, damage, knockback);
        }
    }
}
