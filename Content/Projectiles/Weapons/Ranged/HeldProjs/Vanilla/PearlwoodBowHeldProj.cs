﻿using CalamityOverhaul.Content.Projectiles.Weapons.Ranged.Core;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.ID;

namespace CalamityOverhaul.Content.Projectiles.Weapons.Ranged.HeldProjs.Vanilla
{
    internal class PearlwoodBowHeldProj : BaseBow
    {
        public override string Texture => CWRConstant.Placeholder;
        public override Texture2D TextureValue => TextureAssets.Item[ItemID.PearlwoodBow].Value;
        public override int TargetID => ItemID.PearlwoodBow;
        public override void SetRangedProperty() {
            ArmRotSengsBackBaseValue = 70;
            ShootSpanTypeValue = SpanTypesEnum.WoodenBow;
        }
    }
}
