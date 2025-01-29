﻿using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Projectiles.Melee;
using CalamityOverhaul.Content.Projectiles.Weapons.Melee.Core;
using CalamityOverhaul.Content.RemakeItems.Core;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityOverhaul.Content.RemakeItems.Melee
{
    internal class RDraconicDestruction : ItemOverride
    {
        public override int TargetID => ModContent.ItemType<DraconicDestruction>();
        public override void SetDefaults(Item item) => item.SetKnifeHeld<DraconicDestructionHeld>();
        public override bool? Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source
            , Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            return false;
        }
    }

    internal class DraconicDestructionHeld : BaseKnife
    {
        public override int TargetID => ModContent.ItemType<DraconicDestruction>();
        public override string trailTexturePath => CWRConstant.Masking + "MotionTrail3";
        public override string gradientTexturePath => CWRConstant.ColorBar + "HolyCollider_Bar";
        public override void SetKnifeProperty() {
            Projectile.width = Projectile.height = 60;
            canDrawSlashTrail = true;
            SwingData.starArg = 54;
            SwingData.baseSwingSpeed = 4f;
            distanceToOwner = 40;
            drawTrailTopWidth = 30;
            Length = 86;
            ShootSpeed = 14;
        }

        public override void Shoot() {
            Projectile.NewProjectile(Source, ShootSpanPos, ShootVelocity
                , ModContent.ProjectileType<DracoBeam>(), Projectile.damage
                , Projectile.knockBack, Owner.whoAmI, 0f, 0);
        }

        public override bool PreInOwnerUpdate() {
            if (Main.rand.NextBool(5 * UpdateRate)) {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Lava);
            }
            return base.PreInOwnerUpdate();
        }
    }
}
