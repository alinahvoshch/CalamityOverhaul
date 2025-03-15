﻿using System.Collections.Generic;
using Terraria.Audio;
using Terraria.ID;

namespace CalamityOverhaul.Content.Industrials.Generator
{
    internal class FuelItems
    {
        public readonly static Dictionary<int, int> FuelItemToCombustion = new Dictionary<int, int>() {
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
            { ItemID.SpookyWood, 100 },
            { ItemID.DynastyWood, 50 },
            { ItemID.BorealWood, 50 },
            { ItemID.PalmWood, 50 },
            { ItemID.AshWood, 50 },
            { ItemID.LavaBucket, 500 },
            { ItemID.Gel, 80 },
            { ItemID.BottomlessLavaBucket, 2000 },
            { ItemID.Acorn, 20 },
            { ItemID.WorkBench, 200 },
            { ItemID.WoodenBow, 200 },
            { ItemID.WoodenArrow, 10 },
            { ItemID.WoodWall, 10 },
            { ItemID.MeteoriteBar, 400 },
            { ItemID.HellstoneBar, 600 },
            { ItemID.GlowingMushroom, 20 },
            { ItemID.WoodenHammer, 200 },
            { ItemID.WoodenBoomerang, 200 },
            { ItemID.Loom, 200 },
            { ItemID.Book, 50 },
            { ItemID.Piano, 250 },
            { ItemID.Dresser, 250 },
            { ItemID.Vine, 100 },
            { ItemID.Bed, 400 },
            { ItemID.Silk, 50 },
            { ItemID.BlackThread, 10 },
            { ItemID.GreenThread, 10 },
            { ItemID.Leather, 10 },
            { ItemID.Robe, 200 },
            { ItemID.Feather, 20 },
            { ItemID.Bench, 200 },
            { ItemID.Bathtub, 200 },
            { ItemID.Keg, 200 },
            { ItemID.Bookcase, 200 },
            { ItemID.TatteredCloth, 20 },
            { ItemID.Sawmill, 200 },
            { ItemID.PlankedWall, 10 },
            { ItemID.WoodenBeam, 10 },
            { ItemID.Mannequin, 300 },
            { ItemID.CursedFlame, 250 },
            { ItemID.BlueTorch, 10 },
            { ItemID.RedTorch, 10 },
            { ItemID.GreenTorch, 10 },
            { ItemID.PurpleTorch, 10 },
            { ItemID.WhiteTorch, 10 },
            { ItemID.YellowTorch, 10 },
            { ItemID.DemonTorch, 10 },
            { ItemID.CursedTorch, 20 },
            { ItemID.EbonwoodWall, 10 },
            { ItemID.RichMahoganyWall, 10 },
            { ItemID.PearlwoodWall, 10 },
            { ItemID.EbonwoodChest, 10 },
            { ItemID.RichMahoganyChest, 200 },
            { ItemID.PearlwoodChest, 200 },
            { ItemID.EbonwoodChair, 120 },
            { ItemID.RichMahoganyChair, 120 },
            { ItemID.PearlwoodChair, 120 },
            { ItemID.EbonwoodPlatform, 5 },
            { ItemID.RichMahoganyPlatform, 5 },
            { ItemID.PearlwoodPlatform, 5 },
            { ItemID.EbonwoodWorkBench, 200 },
            { ItemID.RichMahoganyWorkBench, 200 },
            { ItemID.PearlwoodWorkBench, 200 },
            { ItemID.EbonwoodTable, 120 },
            { ItemID.RichMahoganyTable, 120 },
            { ItemID.PearlwoodTable, 120 },
            { ItemID.EbonwoodPiano, 250 },
            { ItemID.RichMahoganyPiano, 250 },
            { ItemID.PearlwoodPiano, 250 },
            { ItemID.EbonwoodBed, 400 },
            { ItemID.RichMahoganyBed, 400 },
            { ItemID.PearlwoodBed, 400 },
            { ItemID.EbonwoodDresser, 250 },
            { ItemID.RichMahoganyDresser, 250 },
            { ItemID.PearlwoodDresser, 250 },
            { ItemID.EbonwoodDoor, 150 },
            { ItemID.RichMahoganyDoor, 150 },
            { ItemID.PearlwoodDoor, 150 },
            { ItemID.EbonwoodSword, 150 },
            { ItemID.EbonwoodHammer, 200 },
            { ItemID.EbonwoodBow, 200 },
            { ItemID.RichMahoganySword, 150 },
            { ItemID.RichMahoganyHammer, 200 },
            { ItemID.RichMahoganyBow, 200 },
            { ItemID.PearlwoodSword, 150 },
            { ItemID.PearlwoodHammer, 200 },
            { ItemID.PearlwoodBow, 200 },
            { ItemID.BorealWoodWorkBench, 200 },
            { ItemID.BorealWoodTable, 120 },
            { ItemID.RedPotion, 1000 },
            { ItemID.WoodHelmet, 200 },
            { ItemID.WoodBreastplate, 200 },
            { ItemID.WoodGreaves, 200 },
            { ItemID.EbonwoodHelmet, 200 },
            { ItemID.EbonwoodBreastplate, 200 },
            { ItemID.EbonwoodGreaves, 200 },
            { ItemID.RichMahoganyHelmet, 200 },
            { ItemID.RichMahoganyBreastplate, 200 },
            { ItemID.RichMahoganyGreaves, 200 },
            { ItemID.PearlwoodHelmet, 200 },
            { ItemID.PearlwoodBreastplate, 200 },
            { ItemID.PearlwoodGreaves, 200 },
            { ItemID.GrassWall, 10 },
            { ItemID.JungleWall, 10 },
            { ItemID.FlowerWall, 10 },
            { ItemID.MushroomWall, 10 },
            { ItemID.SlimeBlockWall, 20 },
            { ItemID.LivingWoodChair, 120 },
            { ItemID.MushroomChair, 120 },
            { ItemID.MushroomWorkBench, 200 },
            { ItemID.SlimeWorkBench, 300 },
            { ItemID.MushroomDoor, 150 },
            { ItemID.LivingWoodDoor, 150 },
            { ItemID.LivingWoodTable, 120 },
            { ItemID.LivingWoodChest, 200 },
            { ItemID.ShadewoodDoor, 150 },
            { ItemID.ShadewoodPlatform, 5 },
            { ItemID.ShadewoodChest, 200 },
            { ItemID.ShadewoodChair, 120 },
            { ItemID.ShadewoodWorkBench, 200 },
            { ItemID.ShadewoodTable, 120 },
            { ItemID.ShadewoodDresser, 250 },
            { ItemID.ShadewoodPiano, 250 },
            { ItemID.ShadewoodBed, 400 },
            { ItemID.ShadewoodSword, 150 },
            { ItemID.ShadewoodHammer, 200 },
            { ItemID.ShadewoodBow, 200 },
            { ItemID.ShadewoodHelmet, 200 },
            { ItemID.ShadewoodBreastplate, 200 },
            { ItemID.ShadewoodGreaves, 200 },
            { ItemID.Rope, 5 },
            { ItemID.RopeCoil, 50 },
            { ItemID.RainHat, 100 },
            { ItemID.RainCoat, 100 },
            { ItemID.WoodenSpike, 20 },
            { ItemID.UmbrellaHat, 100 },
            { ItemID.SailorHat, 80 },
            { ItemID.EyePatch, 50 },
            { ItemID.SailorShirt, 100 },
            { ItemID.SailorPants, 100 },
            { ItemID.Confetti, 10 },
            { ItemID.ExplosivePowder, 50 },
            { ItemID.WoodShelf, 5 },
            { ItemID.HayWall, 20 },
            { ItemID.SpookyWoodWall, 25 },
            { ItemID.SpookyChair, 240 },
            { ItemID.SpookyDoor, 300 },
            { ItemID.SpookyTable, 240 },
            { ItemID.SpookyWorkBench, 600 },
            { ItemID.SpookyPlatform, 10 },
            { ItemID.CursedSapling, 1000 },
            { ItemID.BugNet, 100 },
            { ItemID.EbonwoodBookcase, 200 },
            { ItemID.RichMahoganyBookcase, 200 },
            { ItemID.PearlwoodBookcase, 200 },
            { ItemID.SpookyBookcase, 400 },
            { ItemID.RichMahoganyLantern, 200 },
            { ItemID.PearlwoodLantern, 200 },
            { ItemID.SpookyLantern, 400 },
            { ItemID.EbonwoodCandle, 70 },
            { ItemID.RichMahoganyCandle, 70 },
            { ItemID.PearlwoodCandle, 70 },
            { ItemID.EbonwoodChandelier, 140 },
            { ItemID.RichMahoganyChandelier, 140 },
            { ItemID.PearlwoodChandelier, 140 },
            { ItemID.SpookyChandelier, 280 },
            { ItemID.SpookyBed, 800 },
            { ItemID.RichMahoganyBathtub, 200 },
            { ItemID.PearlwoodBathtub, 200 },
            { ItemID.SpookyBathtub, 400 },
            { ItemID.EbonwoodLamp, 100 },
            { ItemID.RichMahoganyLamp, 100 },
            { ItemID.PearlwoodLamp, 100 },
            { ItemID.SpookyLamp, 200 },
            { ItemID.EbonwoodCandelabra, 50 },
            { ItemID.RichMahoganyCandelabra, 50 },
            { ItemID.PearlwoodCandelabra, 50 },
            { ItemID.SpookyCandelabra, 100 },
            { ItemID.CarpentryRack, 200 },
            { ItemID.SwordRack, 200 },
            { ItemID.ShadewoodBathtub, 200 },
            { ItemID.LivingWoodLamp, 100 },
            { ItemID.ShadewoodLamp, 100 },
            { ItemID.LivingWoodBookcase, 200 },
            { ItemID.ShadewoodBookcase, 200 },
            { ItemID.LivingWoodBed, 400 },
            { ItemID.LivingWoodChandelier, 140 },
            { ItemID.ShadewoodChandelier, 140 },
            { ItemID.LivingWoodLantern, 200 },
            { ItemID.ShadewoodLantern, 200 },
            { ItemID.ShadewoodCandelabra, 50 },
            { ItemID.LivingWoodCandelabra, 50 },
            { ItemID.LivingWoodCandle, 70 },
            { ItemID.ShadewoodCandle, 70 },
            { ItemID.LivingLoom, 250 },
            { ItemID.EbonwoodFence, 5 },
            { ItemID.RichMahoganyFence, 5 },
            { ItemID.PearlwoodFence, 5 },
            { ItemID.ShadewoodFence, 5 },
            { ItemID.DynastyChandelier, 140 },
            { ItemID.DynastyLamp, 100 },
            { ItemID.DynastyLantern, 200 },
            { ItemID.DynastyCandelabra, 50 },
            { ItemID.DynastyChair, 120 },
            { ItemID.DynastyWorkBench, 200 },
            { ItemID.DynastyChest, 200 },
            { ItemID.DynastyBed, 400 },
            { ItemID.DynastyBathtub, 200 },
            { ItemID.DynastyBookcase, 200 },
            { ItemID.DynastyCup, 20 },
            { ItemID.DynastyBowl, 20 },
            { ItemID.DynastyCandle, 70 },
            { ItemID.DynastyClock, 400 },
            { ItemID.LivingWoodPiano, 250 },
            { ItemID.DynastyTable, 120 },
            { ItemID.DynastyDoor, 150 },
            { ItemID.WoodenCrate, 200 },
            { ItemID.OldShoe, 5 },
            { ItemID.SpookyPiano, 500 },
            { ItemID.SpookyDresser, 500 },
            { ItemID.Sofa, 300 },
            { ItemID.EbonwoodSofa, 300 },
            { ItemID.RichMahoganySofa, 300 },
            { ItemID.PearlwoodSofa, 300 },
            { ItemID.ShadewoodSofa, 300 },
            { ItemID.SpookySofa, 600 },
            { ItemID.BorealWoodWall, 10 },
            { ItemID.PalmWoodWall, 10 },
            { ItemID.BorealWoodFence, 5 },
            { ItemID.PalmWoodFence, 5 },
            { ItemID.BorealWoodHelmet, 200 },
            { ItemID.BorealWoodBreastplate, 200 },
            { ItemID.BorealWoodGreaves, 200 },
            { ItemID.PalmWoodHelmet, 200 },
            { ItemID.PalmWoodBreastplate, 200 },
            { ItemID.PalmWoodGreaves, 200 },
            { ItemID.PalmWoodBow, 200 },
            { ItemID.PalmWoodHammer, 200 },
            { ItemID.PalmWoodSword, 150 },
            { ItemID.PalmWoodPlatform, 5 },
            { ItemID.PalmWoodBathtub, 200 },
            { ItemID.PalmWoodBed, 400 },
            { ItemID.PalmWoodBench, 120 },
            { ItemID.PalmWoodCandelabra, 50 },
            { ItemID.PalmWoodCandle, 70 },
            { ItemID.PalmWoodChair, 120 },
            { ItemID.PalmWoodChandelier, 140 },
            { ItemID.PalmWoodChest, 200 },
            { ItemID.PalmWoodSofa, 300 },
            { ItemID.PalmWoodDoor, 150 },
            { ItemID.PalmWoodDresser, 250 },
            { ItemID.PalmWoodLantern, 200 },
            { ItemID.PalmWoodPiano, 250 },
            { ItemID.PalmWoodTable, 120 },
            { ItemID.PalmWoodLamp, 100 },
            { ItemID.PalmWoodWorkBench, 200 },
            { ItemID.BorealWoodBathtub, 200 },
            { ItemID.BorealWoodBed, 400 },
            { ItemID.BorealWoodBookcase, 200 },
            { ItemID.BorealWoodCandelabra, 50 },
            { ItemID.BorealWoodCandle, 70 },
            { ItemID.BorealWoodChair, 120 },
            { ItemID.BorealWoodChandelier, 140 },
            { ItemID.BorealWoodChest, 200 },
            { ItemID.BorealWoodClock, 400 },
            { ItemID.BorealWoodDoor, 150 },
            { ItemID.BorealWoodDresser, 250 },
            { ItemID.BorealWoodLamp, 100 },
            { ItemID.BorealWoodLantern, 200 },
            { ItemID.BorealWoodPiano, 250 },
            { ItemID.BorealWoodPlatform, 5 },
            { ItemID.SlimeBathtub, 300 },
            { ItemID.SlimeBed, 500 },
            { ItemID.SlimeBookcase, 300 },
            { ItemID.SlimeCandelabra, 150 },
            { ItemID.SlimeCandle, 170 },
            { ItemID.SlimeChair, 220 },
            { ItemID.SlimeChandelier, 240 },
            { ItemID.SlimeChest, 300 },
            { ItemID.SlimeClock, 500 },
            { ItemID.SlimeDoor, 250 },
            { ItemID.SlimeDresser, 350 },
            { ItemID.SlimeLamp, 200 },
            { ItemID.SlimeLantern, 300 },
            { ItemID.SlimePiano, 350 },
            { ItemID.SlimePlatform, 15 },
            { ItemID.SlimeSofa, 400 },
            { ItemID.SlimeTable, 220 },
            { ItemID.EbonwoodClock, 400 },
            { ItemID.LivingWoodClock, 400 },
            { ItemID.RichMahoganyClock, 400 },
            { ItemID.PalmWoodClock, 400 },
            { ItemID.PearlwoodClock, 400 },
            { ItemID.ShadewoodClock, 400 },
            { ItemID.SpookyClock, 800 },
            { ItemID.LivingWoodPlatform, 5 },
            { ItemID.LivingWoodWorkBench, 200 },
            { ItemID.LivingWoodSofa, 300 },
            { ItemID.SpookyCandle, 260 },
            { ItemID.WeaponRack, 200 },
            { ItemID.LivingFireBlock, 100 },
            { ItemID.BorealWoodSword, 150 },
            { ItemID.BorealWoodHammer, 200 },
            { ItemID.BorealWoodBow, 200 },
            { ItemID.LivingCursedFireBlock, 250 },
            { ItemID.LivingDemonFireBlock, 100 },
            { ItemID.LivingFrostFireBlock, 100 },
            { ItemID.LivingIchorBlock, 100 },
            { ItemID.LivingUltrabrightFireBlock, 200 },
            { ItemID.WoodenSink, 100 },
            { ItemID.EbonwoodSink, 100 },
            { ItemID.RichMahoganySink, 100 },
            { ItemID.PearlwoodSink, 100 },
            { ItemID.LivingWoodSink, 100 },
            { ItemID.ShadewoodSink, 100 },
            { ItemID.SpookySink, 200 },
            { ItemID.DynastySink, 100 },
            { ItemID.PalmWoodSink, 100 },
            { ItemID.SlimeSink, 150 },
            { ItemID.VineRope, 50 },
            { ItemID.RainbowTorch, 99 },
            { ItemID.CursedCampfire, 200 },
            { ItemID.DemonCampfire, 200 },
            { ItemID.FrozenCampfire, 200 },
            { ItemID.IchorCampfire, 200 },
            { ItemID.RainbowCampfire, 999 },
            { ItemID.PaintingAcorns, 5 },
            { ItemID.PinkGel, 160 },
            { ItemID.PinkTorch, 40 },
            { ItemID.DayBloomPlanterBox, 20 },
            { ItemID.MoonglowPlanterBox, 20 },
            { ItemID.CorruptPlanterBox, 20 },
            { ItemID.CrimsonPlanterBox, 20 },
            { ItemID.BlinkrootPlanterBox, 20 },
            { ItemID.WaterleafPlanterBox, 20 },
            { ItemID.ShiverthornPlanterBox, 20 },
            { ItemID.FireBlossomPlanterBox, 20 },
            { ItemID.ItemFrame, 200 },
            { ItemID.UltraBrightCampfire, 350 },
            { ItemID.AncientCloth, 20 },
            { ItemID.LivingWoodDresser, 250 },
            { ItemID.DynastyPlatform, 5 },
            { ItemID.DynastyDresser, 250 },
            { ItemID.DynastyPiano, 250 },
            { ItemID.DynastySofa, 300 },
            { ItemID.HatRack, 100 },
            { ItemID.WoodenCrateHard, 200 },
            { ItemID.ToiletEbonyWood, 100 },
            { ItemID.ToiletRichMahogany, 100 },
            { ItemID.ToiletPearlwood, 100 },
            { ItemID.ToiletLivingWood, 100 },
            { ItemID.ToiletShadewood, 100 },
            { ItemID.ToiletSpooky, 200 },
            { ItemID.ToiletDynasty, 100 },
            { ItemID.ToiletPalm, 100 },
            { ItemID.ToiletBoreal, 100 },
            { ItemID.ToiletSlime, 150 },
            { ItemID.DesertCampfire, 200 },
            { ItemID.CoralCampfire, 200 },
            { ItemID.CorruptCampfire, 200 },
            { ItemID.CrimsonCampfire, 200 },
            { ItemID.HallowedCampfire, 200 },
            { ItemID.JungleCampfire, 200 },
            { ItemID.AshWoodBathtub, 300 },
            { ItemID.AshWoodBed, 400 },
            { ItemID.AshWoodBookcase, 300 },
            { ItemID.AshWoodDresser, 250 },
            { ItemID.AshWoodCandelabra, 50 },
            { ItemID.AshWoodCandle, 70 },
            { ItemID.AshWoodChair, 120 },
            { ItemID.AshWoodChandelier, 140 },
            { ItemID.AshWoodChest, 200 },
            { ItemID.AshWoodClock, 400 },
            { ItemID.AshWoodDoor, 150 },
            { ItemID.AshWoodLamp, 100 },
            { ItemID.AshWoodLantern, 200 },
            { ItemID.AshWoodPiano, 250 },
            { ItemID.AshWoodPlatform, 5 },
            { ItemID.AshWoodSink, 100 },
            { ItemID.AshWoodSofa, 300 },
            { ItemID.AshWoodTable, 120 },
            { ItemID.AshWoodWorkbench, 200 },
            { ItemID.Fake_AshWoodChest, 200 },
            { ItemID.AshWoodWall, 10 },
            { ItemID.AshWoodFence, 5 },
        };
        /// <summary>
        /// 燃料被消耗时会运行
        /// </summary>
        /// <param name="itemType"></param>
        /// <param name="generator"></param>
        public static void OnAfterFlaming(int itemType, BaseGeneratorTP generator) {
            if (itemType == ItemID.LavaBucket || itemType == ItemID.BottomlessLavaBucket) {
                if (!VaultUtils.isClient) {
                    generator.DropItem(ItemID.EmptyBucket);
                }
            }
        }
    }
}