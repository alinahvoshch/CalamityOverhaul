﻿using CalamityMod;
using CalamityMod.Items.Armor.Bloodflare;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Projectiles.Melee;
using CalamityMod.Sounds;
using CalamityOverhaul.Content.Items.Melee.Extras;
using CalamityOverhaul.Content.Projectiles.Weapons;
using CalamityOverhaul.Content.Projectiles.Weapons.Melee;
using CalamityOverhaul.Content.Projectiles.Weapons.Melee.Core;
using CalamityOverhaul.Content.Projectiles.Weapons.Melee.HeldProjectiles;
using CalamityOverhaul.Content.RemakeItems.Core;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityOverhaul.Content.Items.Melee
{
    /// <summary>
    /// 女妖之爪
    /// </summary>
    internal class BansheeHookEcType : EctypeItem, ICWRLoader
    {
        public override string Texture => CWRConstant.Cay_Wap_Melee + "BansheeHook";
        private Asset<Texture2D> glow;
        internal static int index;
        void ICWRLoader.LoadAsset() => glow = CWRUtils.GetT2DAsset(CWRConstant.Cay_Wap_Melee + "BansheeHookGlow");
        public override void SetStaticDefaults() {
            ItemID.Sets.Spears[Type] = true;
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Type] = true;
        }

        public override void SetDefaults() {
            Item.SetItemCopySD<BansheeHook>();
            Item.SetKnifeHeld<BansheeHookHeld>();
            index = 0;
        }

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor
            , Color alphaColor, float rotation, float scale, int whoAmI) {
            Item.DrawItemGlowmaskSingleFrame(spriteBatch, rotation, glow.Value);
        }

        public override bool AltFunctionUse(Player player) => true;

        public override bool CanUseItem(Player player) => player.ownedProjectileCounts[ModContent.ProjectileType<BansheeHookHeldAlt>()] == 0;

        public static bool ShootFunc(Item item, Player player, EntitySource_ItemUse_WithAmmo source
            , Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
            if (player.altFunctionUse == 2) {
                type = ModContent.ProjectileType<BansheeHookHeldAlt>();
                Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
                SoundEngine.PlaySound(in CommonCalamitySounds.MeatySlashSound, player.Center);
                SoundEngine.PlaySound(in BloodflareHeadRanged.ActivationSound, player.Center);
                item.CWR().MeleeCharge = 0;
                return false;
            }
            if (++index > 3) {
                index = 0;
            }
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, index);
            return false;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source
            , Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
            return ShootFunc(Item, player, source, position, velocity, type, damage, knockback);
        }
    }

    internal class RBansheeHook : BaseRItem
    {
        public override int TargetID => ModContent.ItemType<BansheeHook>();
        public override int ProtogenesisID => ModContent.ItemType<BansheeHookEcType>();
        public override string TargetToolTipItemName => "BansheeHookEcType";
        public override void SetStaticDefaults() {
            ItemID.Sets.Spears[TargetID] = true;
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[TargetID] = true;
        }

        public override void SetDefaults(Item item) {
            item.SetKnifeHeld<BansheeHookHeld>();
            BansheeHookEcType.index = 0;
        }

        public override bool? AltFunctionUse(Item item, Player player) => true;

        public override bool? CanUseItem(Item item, Player player) => player.ownedProjectileCounts[ModContent.ProjectileType<BansheeHookHeldAlt>()] == 0;

        public override bool? Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source
            , Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
            return BansheeHookEcType.ShootFunc(item, player, source, position, velocity, type, damage, knockback);
        }
    }

    internal class BansheeHookHeld : BaseKnife
    {
        public override int TargetID => ModContent.ItemType<BansheeHook>();
        public override string trailTexturePath => CWRConstant.Masking + "MotionTrail3";
        public override string gradientTexturePath => CWRConstant.ColorBar + "WeaverGrievances_Bar";
        public override string GlowTexturePath => CWRConstant.Cay_Proj_Melee + "Spears/BansheeHookAltGlow";
        public override Texture2D TextureValue => CWRUtils.GetT2DValue(CWRConstant.Cay_Proj_Melee + "Spears/BansheeHookAlt");
        public override void SetKnifeProperty() {
            Projectile.width = Projectile.height = 64;
            canDrawSlashTrail = true;
            drawTrailHighlight = false;
            distanceToOwner = 20;
            drawTrailBtommWidth = 50;
            drawTrailTopWidth = 20;
            drawTrailCount = 16;
            Length = 52;
            SwingAIType = SwingAITypeEnum.UpAndDown;
            SwingDrawRotingOffset = MathHelper.PiOver2;
            ShootSpeed = 13;
        }

        public override bool PreSwingAI() {
            if (Projectile.ai[0] == 3) {
                if (Time == 0) {
                    OtherMeleeSize = 1.4f;
                }

                SwingData.baseSwingSpeed = 10;
                SwingAIType = SwingAITypeEnum.Down;

                if (Time < maxSwingTime / 3) {
                    OtherMeleeSize += 0.025f / SwingMultiplication;
                }
                else {
                    OtherMeleeSize -= 0.005f / SwingMultiplication;
                }
                return true;
            }

            if (Projectile.ai[0] == 0) {
                StabBehavior(scaleFactorDenominator: 520f, minLength: 20, maxLength: 120);
                return false;
            }

            return true;
        }

        public override void Shoot() {
            if (Projectile.ai[0] == 3) {
                return;
            }
            if (Projectile.ai[0] == 1 || Projectile.ai[0] == 2) {
                for (int i = 0; i < 3; i++) {
                    Projectile.NewProjectile(Source, ShootSpanPos, ShootVelocity.RotatedBy((-1 + i) * 0.1f)
                        , ModContent.ProjectileType<BansheeHookScythe>(), (int)(Projectile.damage * 0.75f)
                        , Projectile.knockBack * 0.85f, Projectile.owner);
                }
                return;
            }
            Projectile.NewProjectile(Source, ShootSpanPos, ShootVelocity * 1.2f
                , ModContent.ProjectileType<GiantBansheeScythe>(), (int)(Projectile.damage * 0.75f)
                , Projectile.knockBack * 0.85f, Projectile.owner);
        }

        public override void MeleeEffect() {
            Player player = Main.player[Projectile.owner];
            Vector2 vector = player.RotatedRelativePoint(player.MountedCenter);
            float num = player.itemAnimation / (float)player.itemAnimationMax;
            float num2 = (1f - num) * (MathF.PI * 2f);
            float num3 = Projectile.velocity.ToRotation();
            float num4 = Projectile.velocity.Length();
            Vector2 spinningPoint = Vector2.UnitX.RotatedBy(MathF.PI + num2) * new Vector2(num4, Projectile.ai[0]);
            Vector2 destination = vector + spinningPoint.RotatedBy(num3) + new Vector2(num4 + ShootSpeed + 40f, 0f).RotatedBy(num3);
            Vector2 directionToDestination = player.SafeDirectionTo(destination, Vector2.UnitX * player.direction);
            Vector2 velocityNormalized = Projectile.velocity.SafeNormalize(Vector2.UnitY);
            float num5 = 2f;
            float randomDustScaleMin = 0.6f;
            for (int i = 0; i < num5; i++) {
                Dust dust = Dust.NewDustDirect(Projectile.Center, 14, 14, DustID.RedTorch, 0f, 0f, 110);
                dust.velocity = player.SafeDirectionTo(dust.position) * 2f;
                dust.position = Projectile.Center + velocityNormalized.RotatedBy(num2 * 2f + i / num5 * (MathF.PI * 2f)) * 10f;
                dust.scale = 1f + Main.rand.NextFloat(randomDustScaleMin);
                dust.velocity += velocityNormalized * 3f;
                dust.noGravity = true;
            }
            if (Main.rand.NextBool(3)) {
                Dust dust2 = Dust.NewDustDirect(Projectile.Center, 20, 20, DustID.RedTorch, 0f, 0f, 110);
                dust2.velocity = player.SafeDirectionTo(dust2.position) * 2f;
                dust2.position = Projectile.Center + directionToDestination * -110f;
                dust2.scale = 0.45f + Main.rand.NextFloat(0.4f);
                dust2.fadeIn = 0.7f + Main.rand.NextFloat(0.4f);
                dust2.noGravity = true;
                dust2.noLight = true;
            }
        }

        public override void KnifeHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), target.Center, Vector2.Zero
                    , ModContent.ProjectileType<BansheeHookBoom>(), (int)(hit.Damage * 0.25), 10f
                    , Projectile.owner, 0f, 0.85f + Main.rand.NextFloat() * 1.15f);
        }
    }

    //yes yes yes yes 我知道你们在想什么，这是个失败的设计，是一个耻辱
    internal class BansheeHookHeldAlt : BaseHeldProj
    {
        public override string Texture => CWRConstant.Cay_Wap_Melee + "BansheeHook";
        private Item bansheeHook => Owner.GetItem();
        private int drawUIalp = 0;
        public override void SetDefaults() {
            Projectile.width = 40;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.timeLeft = 90;
            Projectile.height = 40;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.ownerHitCheck = true;
            Projectile.hide = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 5;
            Projectile.alpha = 255;
            Projectile.hide = true;
        }

        public override bool ShouldUpdatePosition() => false;

        public override void AI() {
            Projectile.velocity = Vector2.Zero;
            if (Owner == null || bansheeHook == null || (bansheeHook.type != ModContent.ItemType<BansheeHookEcType>() && bansheeHook.type != ModContent.ItemType<BansheeHook>())) {
                Projectile.Kill();
                return;
            }

            Projectile.localAI[1]++;
            Owner.heldProj = Projectile.whoAmI;

            if (Projectile.IsOwnedByLocalPlayer()) {
                int SafeGravDir = Math.Sign(Owner.gravDir);
                float rot = (MathHelper.PiOver2 * SafeGravDir - Owner.Center.To(Projectile.Center).ToRotation()) * Owner.direction * SafeGravDir * SafeGravDir;
                Owner.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, rot * -Owner.direction * SafeGravDir);
                Owner.direction = Owner.Center.To(Projectile.Center).X > 0 ? 1 : -1;
                Projectile.spriteDirection = Owner.direction;
                if (PlayerInput.Triggers.Current.MouseRight) Projectile.timeLeft = 2;
            }

            if (Projectile.ai[2] == 0) {
                Projectile.Center = Owner.GetPlayerStabilityCenter();
                Projectile.rotation += MathHelper.ToRadians(25);

                drawUIalp = Math.Min(drawUIalp + 5, 255);

                if (Projectile.IsOwnedByLocalPlayer()) {
                    bansheeHook.CWR().MeleeCharge += 8.333f;
                    if (Projectile.localAI[1] % 20 == 0) {
                        SoundEngine.PlaySound(SoundID.DD2_GhastlyGlaivePierce, Projectile.Center);
                        for (int i = 0; i < 7; i++) {
                            Vector2 vr = CWRUtils.GetRandomVevtor(0, 360, 25);
                            Projectile.NewProjectile(Owner.FromObjectGetParent(), Owner.Center, vr, ModContent.ProjectileType<BansheeHookScythe>(), Projectile.damage / 2, 0, Owner.whoAmI);
                        }
                    }
                    if (Projectile.localAI[1] % 10 == 0) {
                        for (int i = 0; i < 7; i++) {
                            Vector2 vr = (MathHelper.TwoPi / 7 * i).ToRotationVector2() * 10;
                            Projectile.NewProjectile(Owner.FromObjectGetParent(), Owner.Center, vr, ModContent.ProjectileType<SpiritFlame>(), Projectile.damage / 3, 0, Owner.whoAmI, 1);
                        }
                    }
                }
                if (Projectile.localAI[1] > 60) {
                    bansheeHook.CWR().MeleeCharge = 500;
                    Projectile.ai[2] = 1;
                    Projectile.localAI[1] = 0;
                }
            }

            if (Projectile.ai[2] == 1 && Projectile.IsOwnedByLocalPlayer()) {
                Vector2 toMous = Owner.GetPlayerStabilityCenter().To(Main.MouseWorld).UnitVector();
                Vector2 topos = toMous * 56 + Owner.GetPlayerStabilityCenter();
                Projectile.Center = Vector2.Lerp(topos, Projectile.Center, 0.01f);
                Projectile.rotation = toMous.ToRotation();
                Projectile.localAI[2]++;

                bansheeHook.CWR().MeleeCharge--;

                if (Projectile.localAI[1] > 10) {
                    if (Projectile.localAI[1] % 20 == 0) {
                        SoundEngine.PlaySound(SoundID.DD2_GhastlyGlaivePierce with { Pitch = 0.35f, Volume = 0.7f }, Projectile.Center);
                        int damages = (int)(Owner.GetWeaponDamage(Owner.GetItem()) * 0.5f);
                        for (int i = 0; i < 3; i++) {
                            Vector2 spanPos = Main.MouseWorld + CWRUtils.GetRandomVevtor(0, 360, 160);
                            Projectile.NewProjectile(Owner.FromObjectGetParent(), spanPos, spanPos.To(Main.MouseWorld).UnitVector() * 15f, ModContent.ProjectileType<AbominateHookScythe>(), damages, 0, Owner.whoAmI);
                        }
                    }
                    if (Projectile.localAI[1] % 15 == 0) {
                        for (int i = 0; i < 3; i++) {
                            Vector2 pos = Projectile.Center + Projectile.rotation.ToRotationVector2() * 45 * Projectile.scale + CWRUtils.GetRandomVevtor(0, 360, Main.rand.Next(2, 16));
                            Projectile.NewProjectile(Owner.FromObjectGetParent(), pos, Vector2.Zero, ModContent.ProjectileType<SpiritFlame>(), Projectile.damage / 2, 0, Owner.whoAmI);
                        }
                    }
                }

                if (bansheeHook.CWR().MeleeCharge <= 0) {
                    Projectile.ai[2] = 0;
                    Projectile.localAI[1] = 0;
                    Projectile.netUpdate = true;
                    bansheeHook.CWR().MeleeCharge = 0;
                    SoundEngine.PlaySound(in CommonCalamitySounds.MeatySlashSound, Projectile.Center);
                    SoundEngine.PlaySound(in BloodflareHeadRanged.ActivationSound, Projectile.Center);
                }
            }
        }

        public override bool PreDraw(ref Color lightColor) {
            Texture2D texture2D = ModContent.Request<Texture2D>(Texture).Value;
            Texture2D glow = ModContent.Request<Texture2D>("CalamityMod/Projectiles/Melee/Spears/BansheeHookGlow").Value;

            SpriteEffects spriteEffects = SpriteEffects.None;
            float drawRot = Projectile.rotation + MathHelper.PiOver4;
            Vector2 drawPos = Projectile.Center - Main.screenPosition;

            drawPos += Owner.CWR().SpecialDrawPositionOffset;

            if (Projectile.spriteDirection == -1) {
                spriteEffects = SpriteEffects.FlipVertically;
                drawRot = Projectile.rotation - MathHelper.PiOver4;
            }

            Main.EntitySpriteDraw(texture2D, drawPos, null, lightColor,
                    drawRot, CWRUtils.GetOrig(texture2D), Projectile.scale, spriteEffects);

            Main.EntitySpriteDraw(glow, drawPos, null, lightColor,
                    drawRot, CWRUtils.GetOrig(glow), Projectile.scale, spriteEffects);

            if (Projectile.ai[2] == 0) {
                Texture2D value = CWRAsset.SemiCircularSmear.Value;
                Main.spriteBatch.EnterShaderRegion(BlendState.Additive);
                Main.EntitySpriteDraw(color: WeaverBeam.sloudColor2 * 0.9f
                    , origin: value.Size() * 0.5f, texture: value, position: drawPos
                    , sourceRectangle: null, rotation: Projectile.rotation + MathHelper.PiOver2
                    , scale: Projectile.scale, effects: SpriteEffects.None);
                Main.spriteBatch.ExitShaderRegion();
            }

            TerrorBladeEcType.DrawRageEnergyChargeBar(
                Main.player[Projectile.owner], drawUIalp / 255f,
                bansheeHook.CWR().MeleeCharge / 500f);

            if (Projectile.localAI[2] != 0) {
                Texture2D mainValue = CWRUtils.GetT2DValue(CWRConstant.Masking + "StarTexture_White");
                Vector2 pos = drawPos + UnitToMouseV * 40;
                int Time = (int)Projectile.localAI[2];
                int slp = Time * 5;
                if (slp > 255) { slp = 255; }

                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, SamplerState.PointClamp
                    , DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
                for (int i = 0; i < 5; i++) {
                    Main.spriteBatch.Draw(mainValue, pos, null, Color.Red, MathHelper.ToRadians(Time * 5 + i * 17), CWRUtils.GetOrig(mainValue), slp / 1755f, SpriteEffects.None, 0);
                }
                for (int i = 0; i < 5; i++) {
                    Main.spriteBatch.Draw(mainValue, pos, null, Color.White, MathHelper.ToRadians(Time * 6 + i * 17), CWRUtils.GetOrig(mainValue), slp / 2055f, SpriteEffects.None, 0);
                }
                for (int i = 0; i < 5; i++) {
                    Main.spriteBatch.Draw(mainValue, pos, null, Color.Gold, MathHelper.ToRadians(Time * 9 + i * 17), CWRUtils.GetOrig(mainValue), slp / 2355f, SpriteEffects.None, 0);
                }
                Main.spriteBatch.ResetBlendState();
            }
            return false;
        }

        public override void PostDraw(Color lightColor) {

        }
    }
}
