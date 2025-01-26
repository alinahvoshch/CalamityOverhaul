﻿using CalamityOverhaul.Content.Projectiles.Weapons.Ranged.Core;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.ID;

namespace CalamityOverhaul.Content.Projectiles.Weapons.Ranged.HeldProjs.Vanilla
{
    internal class DemonBowHeldProj : BaseBow
    {
        public override string Texture => CWRConstant.Placeholder;
        public override Texture2D TextureValue => TextureAssets.Item[ItemID.DemonBow].Value;
        public override int targetCayItem => ItemID.DemonBow;
        public override int targetCWRItem => ItemID.DemonBow;
        public override void SetRangedProperty() {
            InOwner_HandState_AlwaysSetInFireRoding = true;
            BowstringData.DeductRectangle = new Rectangle(2, 10, 2, 20);
            ShootSpanTypeValue = SpanTypesEnum.DemonBow;
        }
    }
}
