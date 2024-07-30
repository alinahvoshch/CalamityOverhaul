﻿using CalamityMod.Items.Weapons.Melee;
using CalamityOverhaul.Content.Items.Melee;
using CalamityOverhaul.Content.Projectiles.Weapons.Melee.HeldProjectiles;
using CalamityOverhaul.Content.RemakeItems.Core;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;

namespace CalamityOverhaul.Content.RemakeItems.Melee
{
    internal class RHellkite : BaseRItem
    {
        public override int TargetID => ModContent.ItemType<Hellkite>();
        public override int ProtogenesisID => ModContent.ItemType<HellkiteEcType>();
        public override void SetDefaults(Item item) => item.SetKnifeHeld<HellkiteHeld>();
        public override bool? Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source
            , Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            return false;
        }
    }
}
