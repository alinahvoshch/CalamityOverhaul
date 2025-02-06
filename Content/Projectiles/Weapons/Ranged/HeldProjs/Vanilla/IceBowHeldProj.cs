﻿using CalamityOverhaul.Content.RangedModify.Core;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;

namespace CalamityOverhaul.Content.Projectiles.Weapons.Ranged.HeldProjs.Vanilla
{
    internal class IceBowHeldProj : BaseBow
    {
        public override string Texture => CWRConstant.Placeholder;
        public override Texture2D TextureValue => TextureAssets.Item[ItemID.IceBow].Value;
        public override int TargetID => ItemID.IceBow;
        public override void SetRangedProperty() {
            ArmRotSengsBackBaseValue = 70;
            ShootSpanTypeValue = SpanTypesEnum.IceBow;
            ForcedConversionTargetAmmoFunc = () => Owner.IsWoodenAmmo(AmmoTypes);
            ISForcedConversionDrawAmmoInversion = true;
            ToTargetAmmo = ProjectileID.FrostArrow;
            BowstringData.DeductRectangle = new Rectangle(2, 6, 2, 24);
        }

        public override void SetShootAttribute() {
            Item.useTime = 5;
            fireIndex++;
            if (++fireIndex >= 3) {
                Item.useTime = 25;
                fireIndex = 0;
            }
        }
    }
}
