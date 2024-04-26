﻿using CalamityMod;
using CalamityOverhaul.Content.Projectiles.Weapons.Ranged;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;

namespace CalamityOverhaul.Content.Projectiles.Weapons.Magic
{
    internal abstract class BaseMagicGun : BaseGun
    {
        /// <summary>
        /// 法力恢复延迟，默认为<see cref="Player.maxRegenDelay"/>的对应值
        /// </summary>
        protected float SetRegenDelayValue;

        public override void SetRangedProperty() {
            Projectile.DamageType = DamageClass.Magic;
            SetMagicProperty();
        }

        public virtual void SetMagicProperty() {

        }

        public override void SpanProj() {
            if (SetRegenDelayValue == 0) {
                SetRegenDelayValue = Owner.maxRegenDelay;
            }
            if (GunShootCoolingValue <= 0 && (onFire || onFireR)) {
                if (ForcedConversionTargetAmmoFunc.Invoke()) {
                    AmmoTypes = ToTargetAmmo;
                }

                if (Owner.CheckMana(Item)) {
                    if (FiringDefaultSound) {
                        SoundEngine.PlaySound(Item.UseSound, Projectile.Center);
                    }
                    if (onFire) {
                        Shoot();
                    }
                    if (onFireR) {
                        ShootR();
                    }
                    CreateRecoil();
                    Owner.statMana -= Item.mana;
                    Owner.manaRegenDelay = SetRegenDelayValue;
                    if (Owner.statMana < 0) {
                        Owner.statMana = 0;
                    }
                }

                if (Owner.Calamity().luxorsGift || ModOwner.TheRelicLuxor > 0) {
                    LuxirEvent();
                }
                if (GlobalItemBehavior) {
                    ItemLoaderInFireSetBaver();
                }
                if (fireLight > 0) {
                    Lighting.AddLight(GunShootPos, CWRUtils.MultiStepColorLerp(Main.rand.NextFloat(0.3f, 0.65f), Color.Red, Color.Gold).ToVector3() * fireLight);
                }

                GunShootCoolingValue += Item.useTime;
                onFire = false;
            }
        }

        public virtual int Shoot() {
            return Projectile.NewProjectile(Source, GunShootPos, ShootVelocity, Item.shoot, WeaponDamage, WeaponKnockback, Owner.whoAmI, 0);
        }

        public virtual int ShootR() {
            return Projectile.NewProjectile(Source, GunShootPos, ShootVelocity, Item.shoot, WeaponDamage, WeaponKnockback, Owner.whoAmI, 0);
        }
    }
}
