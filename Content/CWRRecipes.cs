﻿using CalamityMod.Items;
using CalamityMod.Items.DraedonMisc;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Placeables;
using CalamityMod.Items.Weapons.Magic;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Items.Weapons.Rogue;
using CalamityMod.Items.Weapons.Summon;
using CalamityMod.Tiles.Furniture.CraftingStations;
using CalamityOverhaul.Common;
using CalamityOverhaul.Content.Items.Materials;
using CalamityOverhaul.Content.Items.Melee.Extras;
using CalamityOverhaul.Content.RemakeItems.Core;
using CalamityOverhaul.Content.Tiles;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using static Terraria.ModLoader.ModContent;

namespace CalamityOverhaul.Content
{
    internal class CWRRecipes : ModSystem
    {
        public static string Any => Language.GetTextValue("LegacyMisc.37");
        public static RecipeGroup ARGroup;
        public static RecipeGroup GodDWGroup;
        public static RecipeGroup FishGroup;
        public static RecipeGroup AdamantiteBarGroup;
        public static void MS_Error_Set() {
            if (ModGanged.Has_MS_Config_recursionCraftingDepth(out ModConfig modConfig)) {
                string errorText = "检测到异常的合成任务调用，已经将RecursionCraftingDepth设置为0，该防御性改动是临时的，" +
                    "请自行前往magicStorage的模组设置中将RecursionCraftingDepth调整为0并保存!";
                string errorText2 = "Abnormal composition task call detected, RecursionCraftingDepth has been set to 0, " +
                    "this defensive change is temporary, please go to magicStorage's module Settings to adjust RecursionCraftingDepth to 0 and save!";
                CWRUtils.Text(VaultUtils.Translation(errorText, errorText2), Color.Red);
                ModGanged.MS_Config_recursionCraftingDepth_FieldInfo.SetValue(modConfig, 0);
            }
        }

        public static void SpawnAction(Recipe recipe, Item item, List<Item> consumedItems, Item destinationStack) {
            item.TurnToAir();
            Main.LocalPlayer.CWR().InspectOmigaTime = 120;
            CombatText.NewText(Main.LocalPlayer.Hitbox, Main.DiscoColor
                , Language.GetTextValue($"Mods.CalamityOverhaul.Tools.RecipesLoseText"));

            MS_Error_Set();
        }

        public static void MouldRecipeEvent(Recipe recipe, Item item, List<Item> consumedItems, Item destinationStack) {
            MurasamaMould murasamaMould = null;
            foreach (Item i in consumedItems) {
                if (i.type == ItemType<MurasamaMould>()) {
                    murasamaMould = (MurasamaMould)i.ModItem;
                }
            }
            if (murasamaMould != null) {
                murasamaMould.Durability--;
                if (murasamaMould.Durability <= 0) {
                    murasamaMould.Item.TurnToAir();
                }
                else {
                    Main.LocalPlayer.QuickSpawnItem(item.FromObjectGetParent(), murasamaMould.Item);
                }
            }

            MS_Error_Set();
        }

        public override void Unload() {
            ARGroup = null;
            GodDWGroup = null;
            FishGroup = null;
            AdamantiteBarGroup = null;
        }

        private void ModifyResultContent(Recipe recipe) {
            //修改凤凰爆破枪的合成
            {
                if (recipe.HasResult(ItemID.PhoenixBlaster)) {
                    recipe.AddIngredient(ItemType<PurifiedGel>(), 10);//添加纯净凝胶
                }
            }
            //修改肉后矿弩的合成：添加前置弓以及同级转化
            {
                //秘银弓合成需要钴蓝弓
                if (recipe.HasResult(ItemID.MythrilRepeater)) {
                    recipe.AddIngredient(ItemID.CobaltRepeater, 1);
                }
                //山铜弓合成需要钯金弓
                if (recipe.HasResult(ItemID.OrichalcumRepeater)) {
                    recipe.AddIngredient(ItemID.PalladiumRepeater, 1);
                }
                //精金弓合成需要秘银弓
                if (recipe.HasResult(ItemID.AdamantiteRepeater)) {
                    recipe.AddIngredient(ItemID.MythrilRepeater, 1);
                }
                //钛金弓合成需要山铜弓
                if (recipe.HasResult(ItemID.TitaniumRepeater)) {
                    recipe.AddIngredient(ItemID.OrichalcumRepeater, 1);
                }
                //神圣连弩合成需要精金弩和钛金弩
                if (recipe.HasResult(ItemID.HallowedRepeater)) {
                    recipe.AddIngredient(ItemID.AdamantiteRepeater, 1);
                    recipe.AddIngredient(ItemID.TitaniumRepeater, 1);
                    recipe.AddIngredient(ItemID.Ichor, 5);//添加灵液
                    recipe.AddIngredient(ItemID.CursedFlame, 5);//添加诅咒焰
                    recipe.AddIngredient(ItemID.UnicornHorn, 1);//添加独角兽角
                }
            }
            //修改禅心剑的合成
            {
                if (recipe.HasResult(ItemType<Ataraxia>())) {
                    recipe.RemoveIngredient(ItemType<AuricBar>());//移除圣金源锭的配方
                }
            }
            //修改天底的合成
            {
                if (recipe.HasResult(ItemType<Nadir>())) {
                    recipe.RemoveIngredient(ItemType<AuricBar>());//移除圣金源锭的配方
                    recipe.AddIngredient(ItemType<CosmiliteBar>(), 5);//添加宇宙锭
                }
            }
            //修改天顶剑的合成
            {
                if (recipe.HasResult(ItemID.Zenith)) {
                    recipe.RemoveIngredient(ItemType<AuricBar>());//移除圣金源锭的配方
                    recipe.RemoveTile(134);
                    recipe.AddTile(TileID.LunarCraftingStation);
                }
            }
            //修改大守卫者的合成
            {
                if (recipe.HasResult(ItemType<GrandGuardian>())) {
                    recipe.RemoveIngredient(ItemID.FragmentNebula);//移除星云碎片
                    recipe.AddIngredient(ItemID.LunarBar, 5);//添加夜明锭
                }
            }
            //修改月神P的合成
            {
                if (recipe.HasResult(ItemType<SomaPrime>())) {
                    recipe.AddIngredient(ItemType<Infinity>());//添加无穷
                }
            }
            //添加无尽催化剂的额外联动合成
            {
                if (CWRLoad.EternitySoul > ItemID.None) {
                    if (recipe.HasResult(ItemType<InfinityCatalyst>())) {
                        recipe.AddIngredient(CWRLoad.DeviatingEnergy, InfinityCatalyst.QFD(15));
                        recipe.AddIngredient(CWRLoad.AbomEnergy, InfinityCatalyst.QFD(15));
                        recipe.AddIngredient(CWRLoad.EternalEnergy, InfinityCatalyst.QFD(15));
                    }
                    //if (recipe.HasResult(CWRLoad.EternitySoul)) {//永恒魂额外需要5个无尽锭来合成
                    //    recipe.AddIngredient(ItemType<InfiniteIngot>(), 5);
                    //}
                }
                if (CWRLoad.MetanovaBar > ItemID.None) {
                    if (recipe.HasResult(ItemType<InfinityCatalyst>())) {
                        recipe.AddIngredient(CWRLoad.MetanovaBar, InfinityCatalyst.QFD(15));
                    }
                }
            }
            //修改暴政的合成
            {
                if (recipe.HasResult(ItemType<TheEnforcer>())) {
                    recipe.DisableRecipe();
                }
            }
            //瘟疫系列修改
            {
                //瘟疫大剑
                if (recipe.HasResult(ItemType<PlagueKeeper>())) {
                    recipe.RemoveIngredient(ItemID.LunarBar);//移除夜明锭的配方
                    recipe.AddIngredient(ItemType<Hellkite>());//添加地狱龙锋
                    recipe.AddIngredient(ItemType<PestilenceIngot>(), 5);//添加瘟疫锭
                }
                //瘟疫
                if (recipe.HasResult(ItemType<Contagion>())) {
                    recipe.RemoveIngredient(ItemType<PlagueCellCanister>());//移除瘟疫细胞罐的配方
                    recipe.AddIngredient(ItemType<PestilenceIngot>(), 15);//添加瘟疫锭
                }
                //瘟疫胸甲
                if (recipe.HasResult(ItemType<CalamityMod.Items.Armor.Plaguebringer.PlaguebringerCarapace>())) {
                    recipe.RemoveIngredient(ItemType<InfectedArmorPlating>());//移除瘟疫装甲镀层的配方
                    recipe.RemoveIngredient(ItemType<PlagueCellCanister>());//移除瘟疫细胞罐的配方
                    recipe.AddIngredient(ItemType<PestilenceIngot>(), 8);//添加瘟疫锭
                }
                //瘟疫头盔
                if (recipe.HasResult(ItemType<CalamityMod.Items.Armor.Plaguebringer.PlaguebringerVisor>())) {
                    recipe.RemoveIngredient(ItemType<InfectedArmorPlating>());//移除瘟疫装甲镀层的配方
                    recipe.RemoveIngredient(ItemType<PlagueCellCanister>());//移除瘟疫细胞罐的配方
                    recipe.AddIngredient(ItemType<PestilenceIngot>(), 5);//添加瘟疫锭
                }
                //瘟疫护腿
                if (recipe.HasResult(ItemType<CalamityMod.Items.Armor.Plaguebringer.PlaguebringerPistons>())) {
                    recipe.RemoveIngredient(ItemType<InfectedArmorPlating>());//移除瘟疫装甲镀层的配方
                    recipe.RemoveIngredient(ItemType<PlagueCellCanister>());//移除瘟疫细胞罐的配方
                    recipe.AddIngredient(ItemType<PestilenceIngot>(), 5);//添加瘟疫锭
                }
            }
            //修改拉扎尔射线
            {
                if (recipe.HasResult(ItemType<Lazhar>())) {
                    recipe.RemoveIngredient(ItemID.SpaceGun);//移除太空枪的配方
                }
            }
        }

        private void AddResultContent() {
            //添加热线枪的合成
            {
                Recipe.Create(ItemID.HeatRay)
                .AddIngredient(ItemID.SpaceGun)
                    .AddIngredient(ItemType<ScoriaBar>(), 5)
                    .AddTile(TileID.MythrilAnvil)
                    .Register();
            }

            //添加血泪的额外合成
            {
                Recipe.Create(ItemID.BloodMoonStarter)
                    .AddIngredient(ItemType<BloodSample>(), 50)
                    .AddIngredient(ItemType<BlightedGel>(), 75)
                    .AddTile(TileID.DemonAltar)
                    .Register();
                Recipe.Create(ItemID.BloodMoonStarter)
                    .AddIngredient(ItemType<RottenMatter>(), 50)
                    .AddIngredient(ItemType<BlightedGel>(), 75)
                    .AddTile(TileID.DemonAltar)
                    .Register();
            }

            //添加迈达斯统帅的合成
            {
                Recipe.Create(ItemType<MidasPrime>())
                .AddIngredient(ItemType<CrackshotColt>())
                .AddIngredient(ItemID.GoldRing)
                .AddTile(TileID.Anvils)
                .Register();
            }
            //添加钱币枪的合成
            {
                Recipe.Create(ItemID.CoinGun)
                .AddIngredient(ItemType<MidasPrime>())
                .AddRecipeGroup(AdamantiteBarGroup, 5)
                .AddIngredient(ItemID.PlatinumCoin, 5)
                .AddTile(TileID.MythrilAnvil)
                .Register();
            }
            //添加暴政的新合成
            {
                Recipe.Create(ItemType<TheEnforcer>())
                    .AddIngredient(ItemType<HolyCollider>())
                    .AddIngredient(ItemType<CosmiliteBar>(), 5)
                    .AddTile(TileType<CosmicAnvil>())
                    .Register();
            }
            //添加圣火之刃的合成
            {
                Recipe.Create(ItemType<HolyCollider>())
                .AddIngredient(ItemType<CelestialClaymore>())
                .AddIngredient(ItemType<DivineGeode>(), 16)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
            }
            //添加风暴长矛的合成
            {
                Recipe.Create(ItemID.ThunderSpear)
                .AddIngredient(ItemID.Spear)
                .AddIngredient<StormlionMandible>(5)
                .AddTile(TileID.Anvils)
                .Register();
                Recipe.Create(ItemID.ThunderSpear)
                    .AddIngredient(ItemID.Trident)
                    .AddIngredient<StormlionMandible>(5)
                    .AddTile(TileID.Anvils)
                    .Register();
            }
            //修改瘟疫系列的合成
            {
                //添加瘟疫长矛的合成
                Recipe.Create(ItemType<DiseasedPike>())
                    .AddIngredient(ItemType<HellionFlowerSpear>())//添加花刺长矛
                    .AddIngredient(ItemType<PestilenceIngot>(), 5)//添加瘟疫锭
                    .AddTile(TileType<PlagueInfuser>())
                    .Register();

                //添加瘟疫悠悠球的合成
                Recipe.Create(ItemType<Pandemic>())
                    .AddIngredient(ItemType<SulphurousGrabber>())//添加硫磺掠夺者
                    .AddIngredient(ItemType<PestilenceIngot>(), 5)//添加瘟疫锭
                    .AddTile(TileType<PlagueInfuser>())
                    .Register();

                //添加毒针的合成
                Recipe.Create(ItemType<TheSyringe>())
                    .AddIngredient(ItemType<PestilenceIngot>(), 5)//添加瘟疫锭
                    .AddTile(TileType<PlagueInfuser>())
                    .Register();

                //添加疫病污染者的合成
                Recipe.Create(ItemType<PestilentDefiler>())
                    .AddIngredient(ItemType<PestilenceIngot>(), 8)//添加瘟疫锭
                    .AddTile(TileType<PlagueInfuser>())
                    .Register();

                //添加蜂巢发射器的合成
                Recipe.Create(ItemType<TheHive>())
                    .AddIngredient(ItemType<PestilenceIngot>(), 8)//添加瘟疫锭
                    .AddTile(TileType<PlagueInfuser>())
                    .Register();

                //添加枯萎散播者的合成
                Recipe.Create(ItemType<BlightSpewer>())
                    .AddIngredient(ItemType<PestilenceIngot>(), 8)//添加瘟疫锭
                    .AddTile(TileType<PlagueInfuser>())
                    .Register();

                //添加蜂毒弓的合成
                Recipe.Create(ItemType<Malevolence>())
                    .AddIngredient(ItemType<PestilenceIngot>(), 8)//添加瘟疫锭
                    .AddTile(TileType<PlagueInfuser>())
                    .Register();

                //添加瘟疫法杖的合成
                Recipe.Create(ItemType<PlagueStaff>())
                    .AddIngredient(ItemType<PestilenceIngot>(), 8)//添加瘟疫锭
                    .AddTile(TileType<PlagueInfuser>())
                    .Register();

            }
            //添加闪光皇后鱼的配方
            {
                Recipe.Create(ItemType<CalamityMod.Items.Fishing.SunkenSeaCatches.SparklingEmpress>())
                    .AddRecipeGroup(FishGroup)
                    .AddIngredient(ItemType<SeaPrism>(), 15)
                    .AddIngredient(ItemType<PearlShard>(), 5)
                    .AddTile(TileID.Anvils)
                    .Register();
            }
            //添加硫火鱼的配方
            {
                Recipe.Create(ItemType<CalamityMod.Items.Fishing.BrimstoneCragCatches.DragoonDrizzlefish>())
                    .AddRecipeGroup(FishGroup)
                    .AddIngredient(ItemID.Hellstone, 15)
                    .AddIngredient(ItemType<PearlShard>(), 5)
                    .AddTile(TileID.Anvils)
                    .Register();
            }
            //添加鬼妖村正的合成
            {
                int _Mould = ItemType<MurasamaMould>();
                int murasamaItem = ItemType<Murasama>();

                Recipe.Create(_Mould)
                    .AddIngredient(murasamaItem, 1)
                    .AddIngredient(ItemType<PlasmaDriveCore>(), 1)
                    .AddIngredient(ItemType<MysteriousCircuitry>(), 5)
                    .AddIngredient(ItemID.CrimtaneBar, 15)
                    .AddConsumeItemCallback((Recipe recipe, int type, ref int amount) => {
                        if (type == murasamaItem)
                            amount = 0;
                    })
                    .AddTile(TileID.Anvils)
                    .DisableDecraft()
                    .Register();

                Recipe.Create(_Mould)
                    .AddIngredient(ItemType<EncryptedSchematicHell>(), 1)
                    .AddIngredient(ItemID.CrimtaneBar, 15)
                    .AddConsumeItemCallback((Recipe recipe, int type, ref int amount) => {
                        if (type == murasamaItem)
                            amount = 0;
                    })
                    .AddTile(TileID.Anvils)
                    .DisableDecraft()
                    .Register();

                Recipe.Create(ItemType<Murasama>())
                    .AddIngredient(ItemID.Muramasa, 1)
                    .AddIngredient(ItemID.MeteoriteBar, 1)
                    .AddIngredient(_Mould, 1)
                    .AddOnCraftCallback(MouldRecipeEvent)
                    .AddTile(TileID.Anvils)
                    .DisableDecraft()
                    .Register();

                Recipe.Create(ItemType<Murasama>())
                    .AddIngredient(ItemID.Katana, 1)
                    .AddIngredient(ItemID.MeteoriteBar, 1)
                    .AddIngredient(ItemID.CobaltBar, 1)
                    .AddIngredient(_Mould, 1)
                    .AddOnCraftCallback(MouldRecipeEvent)
                    .AddTile(TileID.Anvils)
                    .DisableDecraft()
                    .Register();
            }
            //添加卢克索礼物的合成
            {
                Recipe.Create(ItemType<CalamityMod.Items.Accessories.LuxorsGift>())
                    .AddIngredient(ItemID.FossilOre, 5)
                    .AddIngredient(ItemType<PearlShard>(), 12)
                    .AddTile(TileID.Anvils)
                    .Register();
            }
            //添加苍穹破晓合成
            {
                Recipe.Create(ItemType<DawnshatterAzure>())
                    .AddIngredient<Rock>(1)
                    .AddIngredient(ItemID.FragmentSolar, 16)
                    .AddIngredient(ItemID.DayBreak, 1)
                    .AddIngredient<RedSun>(1)
                    .AddIngredient<DraconicDestruction>(1)
                    .AddIngredient<DragonPow>(1)
                    .AddIngredient<DragonRage>(1)
                    .AddIngredient<ShadowspecBar>(3)
                    .AddBlockingSynthesisEvent()
                    .AddTile(TileType<TransmutationOfMatter>())
                    .Register();
            }
            //添加雪球炮的合成
            {
                Recipe.Create(ItemID.SnowballCannon)
                    .AddIngredient(ItemID.IllegalGunParts, 1)
                    .AddIngredient(ItemID.SnowBlock, 30)
                    .AddIngredient(ItemID.IceBlock, 50)
                    .AddTile(TileID.Anvils)
                    .Register();
            }
            //添加恒吹雪的合成
            {
                Recipe.Create(ItemType<EternalBlizzard>())
                    .AddIngredient(ItemType<Arbalest>())
                    .AddIngredient(ItemType<CoreofEleum>(), 6)
                    .AddIngredient(ItemID.IceBlock, 500)
                    .AddTile(TileID.IceMachine)
                    .Register();
            }
            //添加魔影系列的合成
            {
                //诘责
                Recipe.Create(ItemType<Condemnation>())
                    .AddIngredient(ItemID.HallowedRepeater)
                    .AddIngredient(ItemType<AshesofAnnihilation>(), 12)
                    .AddTile(TileType<DraedonsForge>())
                    .Register();
                //狞桀
                Recipe.Create(ItemType<Vehemence>())
                    .AddIngredient(ItemType<ValkyrieRay>())
                    .AddIngredient(ItemType<AshesofAnnihilation>(), 12)
                    .AddTile(TileType<DraedonsForge>())
                    .Register();
                //恣睢
                Recipe.Create(ItemType<Violence>())
                    .AddIngredient(ItemID.Gungnir)
                    .AddIngredient(ItemType<AshesofAnnihilation>(), 12)
                    .AddTile(TileType<DraedonsForge>())
                    .Register();
                //恂戒
                Recipe.Create(ItemType<Vigilance>())
                    .AddIngredient(ItemType<DeathstareRod>())
                    .AddIngredient(ItemType<AshesofAnnihilation>(), 12)
                    .AddTile(TileType<DraedonsForge>())
                    .Register();
            }
        }

        private void RItemLoadenRecipe() {
            foreach (BaseRItem baseRItem in CWRMod.RItemInstances) {
                if (baseRItem.FormulaSubstitution) {
                    baseRItem.LoadItemRecipe();
                }
            }
        }

        public override void PostAddRecipes() {
            {//遍历所有配方，执行对应的配方修改，这个应该执行在最前，放置覆盖后续的修改操作
                for (int i = 0; i < Recipe.numRecipes; i++) {
                    ModifyResultContent(Main.recipe[i]);
                }
            }
            {//添加配方的操作
                AddResultContent();
            }
            {//关于重制节点的配方加载
                if (!CWRServerConfig.Instance.WeaponOverhaul) {
                    RItemLoadenRecipe();
                }
            }
        }

        public override void AddRecipeGroups() {
            AdamantiteBarGroup = new RecipeGroup(() => $"{Any} {Lang.GetItemNameValue(ItemID.AdamantiteBar)}",
            [
                ItemID.AdamantiteBar,
                ItemID.TitaniumBar,
            ]);
            RecipeGroup.RegisterGroup("CWRMod:AdamantiteBarGroup", AdamantiteBarGroup);

            ARGroup = new RecipeGroup(() => $"{Any} {CWRLocText.GetTextValue("CWRRecipes_ApostolicRelics")}",
            [
                ItemType<ArmoredShell>(),
                ItemType<DarkPlasma>(),
                ItemType<TwistingNether>(),
            ]);
            RecipeGroup.RegisterGroup("CWRMod:ARGroup", ARGroup);

            GodDWGroup = new RecipeGroup(() => $"{Any} {CWRLocText.GetTextValue("CWRRecipes_GodEaterWeapon")}",
            [
                ItemType<Excelsus>(),
                ItemType<TheObliterator>(),
                ItemType<Deathwind>(),
                ItemType<DeathhailStaff>(),
                ItemType<StaffoftheMechworm>(),
                ItemType<Eradicator>(),
                ItemType<CosmicDischarge>(),
                ItemType<Norfleet>(),
            ]);
            RecipeGroup.RegisterGroup("CWRMod:AdamantiteBarGroup", GodDWGroup);

            FishGroup = new RecipeGroup(() => $"{Any} {CWRLocText.GetTextValue("CWRRecipes_FishGroup")}",
            [
                ItemID.Goldfish,
                ItemID.Bass,
                ItemID.Trout,
                ItemID.Salmon,
                ItemID.AtlanticCod,
                ItemID.Tuna,
                ItemID.RedSnapper,
                ItemID.NeonTetra,
                ItemID.ArmoredCavefish,
                ItemID.Damselfish,
                ItemID.CrimsonTigerfish,
                ItemID.FrostMinnow,
                ItemID.PrincessFish,
                ItemID.GoldenCarp,
                ItemID.SpecularFish,
                ItemID.Prismite,
                ItemID.VariegatedLardfish,
                ItemID.FlarefinKoi,
                ItemID.DoubleCod,
                ItemID.Honeyfin,
                ItemID.Obsidifish,
                ItemID.ChaosFish,
                ItemID.Stinkfish,
            ]);
            RecipeGroup.RegisterGroup("CWRMod:FishGroup", FishGroup);
        }
    }
}
