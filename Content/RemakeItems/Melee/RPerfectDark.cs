﻿using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Projectiles.Melee;
using CalamityOverhaul.Content.Projectiles.Weapons.Melee.Core;
using CalamityOverhaul.Content.RemakeItems.Core;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace CalamityOverhaul.Content.RemakeItems.Melee
{
    internal class RPerfectDark : ItemOverride
    {
        public override int TargetID => ModContent.ItemType<PerfectDark>();
        public override void SetDefaults(Item item) => item.SetKnifeHeld<PerfectDarkHeld>();
        public override bool? Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source
            , Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            return false;
        }
    }

    internal class PerfectDarkHeld : BaseKnife
    {
        public override int TargetID => ModContent.ItemType<PerfectDark>();
        public override string gradientTexturePath => CWRConstant.ColorBar + "PerfectDark_Bar";
        public override void SetKnifeProperty() {
            Projectile.width = Projectile.height = 40;
            drawTrailHighlight = false;
            canDrawSlashTrail = true;
            SwingData.starArg = 54;
            SwingData.baseSwingSpeed = 4f;
            drawTrailBtommWidth = 30;
            distanceToOwner = 14;
            drawTrailTopWidth = 20;
            Length = 50;
        }

        public override void Shoot() {
            Projectile.NewProjectile(Source, Owner.Center, ShootVelocity
                , ModContent.ProjectileType<DarkBall>(), Projectile.damage, Projectile.knockBack, Owner.whoAmI);
        }

        public override void KnifeHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
            target.AddBuff(ModContent.BuffType<BrainRot>(), 300);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info) {
            target.AddBuff(ModContent.BuffType<BrainRot>(), 300);
        }
    }
}
