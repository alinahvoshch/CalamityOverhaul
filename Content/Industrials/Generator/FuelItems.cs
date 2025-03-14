﻿using System.Collections.Generic;
using Terraria.ID;

namespace CalamityOverhaul.Content.Industrials.Generator
{
    internal class FuelItems
    {
        public static readonly Dictionary<int, int> FuelItemToCombustion = new Dictionary<int, int>() {
            { ItemID.Wood, 50 },
            { ItemID.Coal, 250 },
            { ItemID.Hay, 50 },
            { ItemID.WoodenSword, 150 },
            { ItemID.WoodenDoor, 150 },
            { ItemID.WoodenTable, 120 },
            { ItemID.WoodenChair, 120 },
            { ItemID.WoodPlatform, 20 },
            { ItemID.Ebonwood, 50 },
            { ItemID.RichMahogany, 50 },
            { ItemID.Pearlwood, 50 },
            { ItemID.Shadewood, 50 },
            { ItemID.SpookyWood, 50 },
            { ItemID.DynastyWood, 50 },
            { ItemID.BorealWood, 50 },
            { ItemID.PalmWood, 50 },
            { ItemID.AshWood, 50 },
            { ItemID.LavaBucket, 500 },
            { ItemID.Gel, 80 },
            { ItemID.BottomlessLavaBucket, 2000 },
            { ItemID.Acorn, 20 },
        };
        /// <summary>
        /// 燃料被消耗时会运行
        /// </summary>
        /// <param name="itemType"></param>
        /// <param name="generator"></param>
        public static void OnAfterFlaming(int itemType, BaseGeneratorTP generator) {
            if (itemType == ItemID.LavaBucket && !VaultUtils.isClient) {
                generator.DropItem(ItemID.EmptyBucket);
            }
        }
    }
}