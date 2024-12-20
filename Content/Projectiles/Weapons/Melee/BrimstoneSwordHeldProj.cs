﻿using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Projectiles.BaseProjectiles;
using CalamityMod.Projectiles.Melee;
using CalamityOverhaul.Content.Items.Melee;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalamityOverhaul.Content.Projectiles.Weapons.Melee
{
    internal class BrimstoneSwordHeldProj : BaseShortswordProjectile
    {
        public override LocalizedText DisplayName => CWRUtils.SafeGetItemName<BrimstoneSwordEcType>();
        public override string Texture => "CalamityMod/Items/Weapons/Melee/BrimstoneSword";
        private bool trueMelee;
        public override void SetDefaults() {
            Projectile.Size = new Vector2(36);
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.scale = 1f;
            Projectile.DamageType = ModContent.GetInstance<TrueMeleeDamageClass>();
            Projectile.ownerHitCheck = true;
            Projectile.timeLeft = 360;
            Projectile.hide = true;
            Projectile.ownerHitCheck = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }

        public override void SetVisualOffsets() {
            const int HalfSpriteWidth = 32 / 2;
            const int HalfSpriteHeight = 32 / 2;

            int HalfProjWidth = Projectile.width / 2;
            int HalfProjHeight = Projectile.height / 2;

            DrawOriginOffsetX = 0;
            DrawOffsetX = -(HalfSpriteWidth - HalfProjWidth);
            DrawOriginOffsetY = -(HalfSpriteHeight - HalfProjHeight);
        }

        public override void ExtraBehavior() {
            if (!Owner.Alives()) {
                Projectile.Kill();
                return;
            }
            Item item = Owner.GetItem();
            if (item == null || item.type == ItemID.None) {
                return;
            }
            item.initialize();
            if (Projectile.IsOwnedByLocalPlayer() && Timer == TotalDuration / 2 && !trueMelee && ++item.CWR().ai[0] > 1) {
                Projectile.NewProjectile(Projectile.FromObjectGetParent(), Projectile.Center, Projectile.velocity * 5.5f
                    , ModContent.ProjectileType<BrimstoneSwordBall>(), Projectile.damage / 2, Projectile.knockBack, Main.myPlayer);
                item.CWR().ai[0] = 0;
            }
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers) => modifiers.CritDamage *= 0.5f;

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
            trueMelee = true;
            if (CWRLoad.WormBodys.Contains(target.type) && !Main.rand.NextBool(5)) {
                return;
            }
            var source = Projectile.GetSource_FromThis();
            target.AddBuff(ModContent.BuffType<BrimstoneFlames>(), 300);
            Vector2 spanPos = new(target.Center.X + Main.rand.Next(-260, 260), target.Center.Y);
            Projectile.NewProjectile(source, spanPos, Vector2.Zero
                , ModContent.ProjectileType<Brimblast>(), Projectile.damage, Projectile.knockBack, Main.myPlayer);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info) {
            trueMelee = true;
            var source = Projectile.GetSource_FromThis();
            target.AddBuff(ModContent.BuffType<BrimstoneFlames>(), 300);
            Projectile.NewProjectile(source, target.Center, Vector2.Zero
                , ModContent.ProjectileType<Brimblast>(), Projectile.damage, Projectile.knockBack, Main.myPlayer);
        }

        public override bool PreDraw(ref Color lightColor) {
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            float rot = Projectile.rotation;
            Vector2 drawPosition = Projectile.Center - Main.screenPosition;
            Vector2 origin = texture.Size() / 2;
            Main.EntitySpriteDraw(texture, drawPosition, null, Projectile.GetAlpha(lightColor)
                , rot, origin, Projectile.scale, Owner.direction > 0 ? 0 : SpriteEffects.FlipHorizontally, 0);
            return false;
        }
    }
}
