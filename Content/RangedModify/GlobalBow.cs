﻿using CalamityOverhaul.Content.RangedModify.Core;
using Terraria;
using Terraria.ID;

namespace CalamityOverhaul.Content.RangedModify
{
    internal class GlobalBow : GlobalRanged
    {
        public static bool BowActive {
            get {
                Item handItem = Main.LocalPlayer.GetItem();
                return handItem != null && !CWRLoad.ItemHasCartridgeHolder[handItem.type]
                && (CWRLoad.ItemIsBow[handItem.type] || CWRLoad.ItemIsCrossBow[handItem.type] || IsArrow());
            }
        }

        public static bool IsArrow() {
            Item handItem = Main.LocalPlayer.GetItem();
            if (handItem == null || handItem.type == ItemID.None) {
                return false;
            }
            return handItem.ammo == AmmoID.Arrow;
        }

        public override void PostModifyBow(BaseBow bow) {
            if (bow.TargetID == ItemID.PlatinumBow
                || bow.TargetID == ItemID.RichMahoganyBow
                || bow.TargetID == ItemID.Shadewood
                || bow.TargetID == ItemID.SilverBow
                || bow.TargetID == ItemID.TinBow
                || bow.TargetID == ItemID.TungstenBow
                || bow.TargetID == ItemID.WoodenBow
                || bow.TargetID == ItemID.PalmWoodBow
                || bow.TargetID == ItemID.PearlwoodBow
                || bow.TargetID == ItemID.IronBow
                || bow.TargetID == ItemID.LeadBow
                || bow.TargetID == ItemID.GoldBow
                || bow.TargetID == ItemID.EbonwoodBow
                || bow.TargetID == ItemID.CopperBow
                || bow.TargetID == ItemID.BorealWoodBow) {
                bow.InOwner_HandState_AlwaysSetInFireRoding = true;
                bow.BowstringData.DeductRectangle = new Rectangle(2, 6, 2, 20);
            }
        }
    }
}
