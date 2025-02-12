﻿using CalamityMod.Projectiles.Melee;
using CalamityOverhaul.Content.Items.Melee;
using CalamityOverhaul.Content.MeleeModify.Core;
using Terraria;
using Terraria.ModLoader;

namespace CalamityOverhaul.Content.Projectiles.Weapons.Melee.HeldProjectiles
{
    internal class DivineSourceBladeHeld : BaseKnife
    {
        public override int TargetID => ModContent.ItemType<DivineSourceBlade>();
        public override string trailTexturePath => CWRConstant.Masking + "MotionTrail3";
        public override string gradientTexturePath => CWRConstant.ColorBar + "DragonRage_Bar";
        public override void SetKnifeProperty() {
            Projectile.width = Projectile.height = 112;
            canDrawSlashTrail = true;
            drawTrailCount = 34;
            distanceToOwner = -20;
            drawTrailTopWidth = 86;
            ownerOrientationLock = true;
            SwingData.starArg = 42;
            SwingData.baseSwingSpeed = 4.65f;
            unitOffsetDrawZkMode = 20;
            Length = 124;
            ShootSpeed = 18;
        }

        public override void UpdateCaches() {
            if (Time < 2) {
                return;
            }

            for (int i = drawTrailCount - 1; i > 0; i--) {
                oldRotate[i] = oldRotate[i - 1];
                oldDistanceToOwner[i] = oldDistanceToOwner[i - 1];
                oldLength[i] = oldLength[i - 1];
            }

            oldRotate[0] = safeInSwingUnit.RotatedBy(MathHelper.ToRadians(-8 * Projectile.spriteDirection)).ToRotation();
            oldDistanceToOwner[0] = distanceToOwner;
            oldLength[0] = Projectile.height * Projectile.scale;
        }

        public override bool PreInOwnerUpdate() {
            ExecuteAdaptiveSwing(initialMeleeSize: 1, phase0SwingSpeed: 0.3f
                , phase1SwingSpeed: 8.2f, phase2SwingSpeed: 5f
                , phase0MeleeSizeIncrement: 0, phase2MeleeSizeIncrement: 0);
            return base.PreInOwnerUpdate();
        }

        public override void Shoot() {
            int types = ModContent.ProjectileType<DivineSourceBeam>();
            Vector2 vector2 = Owner.Center.To(Main.MouseWorld).UnitVector() * 3;
            Vector2 position = Owner.Center;
            Projectile.NewProjectile(
                Source, position, vector2, types
                , (int)(Item.damage * 1.25f)
                , Item.knockBack
                , Owner.whoAmI);
            int type = ModContent.ProjectileType<DivineSourceBladeProjectile>();
            Projectile proj = Projectile.NewProjectileDirect(Source, ShootSpanPos, ShootVelocity, type, Projectile.damage, 0, Owner.whoAmI);
            proj.SetArrowRot();
        }

        public override void KnifeHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
            if (Projectile.numHits == 0) {
                int proj = Projectile.NewProjectile(Source, Projectile.Center, Vector2.Zero
                    , ModContent.ProjectileType<TerratomereSlashCreator>(),
                Projectile.damage / 3, 0, Projectile.owner, target.whoAmI, Main.rand.NextFloat(MathHelper.TwoPi));
                Main.projectile[proj].timeLeft = 30;
            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info) {
            if (Projectile.numHits == 0) {
                int proj = Projectile.NewProjectile(Source, Projectile.Center, Vector2.Zero
                    , ModContent.ProjectileType<TerratomereSlashCreator>(),
                Projectile.damage / 3, 0, Projectile.owner, target.whoAmI, Main.rand.NextFloat(MathHelper.TwoPi));
                Main.projectile[proj].timeLeft = 30;
            }
        }
    }
}
