﻿using CalamityMod.Items.Weapons.Magic;
using CalamityOverhaul.Content.Projectiles.Weapons.Magic;
using CalamityOverhaul.Content.RemakeItems.Core;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;

namespace CalamityOverhaul.Content.Items.Magic
{
    /// <summary>
    /// 冰坠法杖
    /// </summary>
    internal class RIcicleStaff : BaseRItem
    {
        public override bool DrawingInfo => false;
        public override int TargetID => ModContent.ItemType<IcicleStaff>();
        public override bool FormulaSubstitution => true;
        public override void SetDefaults(Item item) => item.SetHeldProj<IcicleStaffHeld>();
    }

    internal class IcicleStaffHeld : BaseMagicStaff<IcicleStaff>
    {
        private int fireIndex;
        public override void FiringShoot() {
            int leftorright = (InMousePos - Owner.Center).X > 0 ? 1 : -1;
            Vector2 starpos = Owner.Center + new Vector2(Main.rand.NextFloat(-120, 240) * leftorright, Main.rand.NextFloat(-600, -800));
            Vector2 vel = (InMousePos - starpos).UnitVector() * 12f;
            Projectile.NewProjectile(Source, starpos, vel
                , AmmoTypes, WeaponDamage, WeaponKnockback, Owner.whoAmI, 0);
        }

        public override void HanderPlaySound() {
            if (++fireIndex > 1) {
                SoundEngine.PlaySound(Item.UseSound, Projectile.Center);
                fireIndex = 0;
            }
        }
    }
}
