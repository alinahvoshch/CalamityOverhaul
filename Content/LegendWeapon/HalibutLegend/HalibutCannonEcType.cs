﻿using CalamityMod;
using CalamityMod.Items;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Rarities;
using CalamityOverhaul.Common;
using CalamityOverhaul.Content.Items;
using CalamityOverhaul.Content.RemakeItems.Core;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static CalamityOverhaul.Content.InWorldBossPhase;

namespace CalamityOverhaul.Content.LegendWeapon.HalibutLegend
{
    internal class HalibutCannonEcType : EctypeItem
    {
        #region Data
        public override string Texture => CWRConstant.Cay_Wap_Ranged + "HalibutCannon";
        /// <summary>
        /// 每个时期阶段对应的伤害，这个成员一般不需要直接访问，而是使用<see cref="GetOnDamage"/>
        /// </summary>
        private static Dictionary<int, int> DamageDictionary = new Dictionary<int, int>();
        /// <summary>
        /// 每个时期阶段对应的额外暴击振幅的字典，这个成员一般不需要直接访问，而是使用<see cref="GetOnCrit"/>
        /// </summary>
        private static Dictionary<int, int> SetLevelCritDictionary = new Dictionary<int, int>();
        public static int Level => InWorldBossPhase.Instance.Halibut_Level();
        /// <summary>
        /// 获取开局的伤害
        /// </summary>
        public static int GetStartDamage => DamageDictionary[0];
        /// <summary>
        /// 获取时期对应的伤害
        /// </summary>
        public static int GetOnDamage => DamageDictionary[Level];
        /// <summary>
        /// 计算伤害比例
        /// </summary>
        public static float GetSengsDamage => GetOnDamage / (float)GetStartDamage;
        /// <summary>
        /// 根据<see cref="GetOnDamage"/>获取一个与<see cref="RangedDamageClass"/>相关的乘算伤害
        /// </summary>
        public static int ActualTrueMeleeDamage => (int)(GetOnDamage * Main.LocalPlayer.GetDamage<RangedDamageClass>().Additive);
        /// <summary>
        /// 获取时期对应的额外暴击
        /// </summary>
        public static int GetOnCrit => SetLevelCritDictionary[Level];
        #endregion
        public static void LoadWeaponData() {
            DamageDictionary = new Dictionary<int, int>(){
                {0, 3 },
                {1, 4 },
                {2, 5 },
                {3, 7 },
                {4, 8 },
                {5, 10 },
                {6, 13 },
                {7, 15 },
                {8, 18 },
                {9, 22 },
                {10, 25 },
                {11, 28 },
                {12, 32 },
                {13, 36 },
                {14, 40 }
            };
            SetLevelCritDictionary = new Dictionary<int, int>(){
                {0, 0 },
                {1, 1 },
                {2, 1 },
                {3, 2 },
                {4, 2 },
                {5, 5 },
                {6, 5 },
                {7, 8 },
                {8, 9 },
                {9, 9 },
                {10, 10 },
                {11, 11 },
                {12, 13 },
                {13, 15 },
                {14, 20 }
            };
        }
        public override void SetStaticDefaults() => LoadWeaponData();
        public override void SetDefaults() => SetDefaultsFunc(Item);
        public static void SetDefaultsFunc(Item Item) {
            LoadWeaponData();
            Item.damage = GetStartDamage;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 118;
            Item.height = 56;
            Item.useTime = 10;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.rare = ModContent.RarityType<HotPink>();
            Item.noMelee = true;
            Item.knockBack = 1f;
            Item.value = CalamityGlobalItem.RarityHotPinkBuyPrice;
            Item.UseSound = SoundID.Item38;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.Bullet;
            Item.shootSpeed = 12f;
            Item.useAmmo = AmmoID.Bullet;
            Item.Calamity().canFirePointBlankShots = true;
            Item.CWR().hasHeldNoCanUseBool = true;
            Item.CWR().heldProjType = ModContent.ProjectileType<HalibutCannonHeld>();
        }

        public override void ModifyWeaponCrit(Player player, ref float crit) => crit += GetOnCrit;

        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
            => CWRUtils.ModifyLegendWeaponDamageFunc(player, Item, GetOnDamage, GetStartDamage, ref damage);

        public override void ModifyTooltips(List<TooltipLine> tooltips) => SetTooltip(ref tooltips);

        public static void SetTooltip(ref List<TooltipLine> tooltips) {
            int index = Instance.SHPC_Level();
            string newContent = index >= 0 && index <= 14 ? CWRLocText.GetTextValue($"Halibut_TextDictionary_Content_{index}") : "ERROR";
            if (CWRServerConfig.Instance.WeaponEnhancementSystem) {
                int level = Instance.Halibut_Level();
                string num = (level + 1).ToString();
                if (level == 14) {
                    num = CWRLocText.GetTextValue("Murasama_Text_Lang_End");
                }
                tooltips.ReplaceTooltip("[Lang4]", $"[c/00736d:{CWRLocText.GetTextValue("Murasama_Text_Lang_0") + " "}{num}]", "");
                tooltips.ReplaceTooltip("legend_Text", CWRLocText.GetTextValue("Halibut_No_legend_Content_3"), "");
            }
            else {
                tooltips.ReplaceTooltip("[Lang4]", "", "");
                tooltips.ReplaceTooltip("legend_Text", CWRLocText.GetTextValue("Halibut_No_legend_Content_4"), "");
                newContent = Level11 ? CWRLocText.GetTextValue("Halibut_No_legend_Content_2") : CWRLocText.GetTextValue("Halibut_No_legend_Content_1");
            }
            Color newColor = Color.Lerp(Color.IndianRed, Color.White, 0.5f + (float)Math.Sin(Main.GlobalTimeWrappedHourly) * 0.5f);
            tooltips.ReplaceTooltip("[Text]", CWRUtils.FormatColorTextMultiLine(newContent, newColor), "");
        }
    }

    internal class RHalibutCannon : ItemOverride
    {
        public override int TargetID => ModContent.ItemType<HalibutCannon>();
        public override int ProtogenesisID => ModContent.ItemType<HalibutCannonEcType>();
        public override string TargetToolTipItemName => "HalibutCannonEcType";
        public override void SetDefaults(Item item) => HalibutCannonEcType.SetDefaultsFunc(item);
        public override bool? On_ModifyWeaponCrit(Item item, Player player, ref float crit) {
            crit += HalibutCannonEcType.GetOnCrit;
            return false;
        }
        public override bool On_ModifyWeaponDamage(Item item, Player player, ref StatModifier damage) {
            CWRUtils.ModifyLegendWeaponDamageFunc(player, item, HalibutCannonEcType.GetOnDamage, HalibutCannonEcType.GetStartDamage, ref damage);
            return false;
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) => HalibutCannonEcType.SetTooltip(ref tooltips);
    }
}
