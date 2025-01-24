﻿using CalamityMod.Items.Weapons.Ranged;
using CalamityOverhaul.Common;
using CalamityOverhaul.Content.Items.Ranged;
using CalamityOverhaul.Content.RemakeItems.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalamityOverhaul.Content.RemakeItems.Ranged
{
    internal class RHalibutCannon : BaseRItem
    {
        public override int TargetID => ModContent.ItemType<HalibutCannon>();
        public override int ProtogenesisID => ModContent.ItemType<HalibutCannonEcType>();
        public override void SetDefaults(Item item) => HalibutCannonEcType.SetDefaultsFunc(item);
        public override bool? On_ModifyWeaponCrit(Item item, Player player, ref float crit) {
            crit += HalibutCannonEcType.GetOnCrit;
            return false;
        }

        public override bool On_ModifyWeaponDamage(Item item, Player player, ref StatModifier damage) {
            CWRUtils.ModifyLegendWeaponDamageFunc(player, item, HalibutCannonEcType.GetOnDamage, HalibutCannonEcType.GetStartDamage, ref damage);
            return false;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
            List<TooltipLine> newTooltips = new List<TooltipLine>(tooltips);
            List<TooltipLine> prefixTooltips = [];
            foreach (TooltipLine line in newTooltips.ToList()) {
                for (int i = 0; i < 9; i++) {
                    if (line.Name == "Tooltip" + i) {
                        line.Hide();
                    }
                }
                if (line.Name.Contains("Prefix")) {
                    prefixTooltips.Add(line.Clone());
                    line.Hide();
                }
            }

            string textContent = Language.GetText("Mods.CalamityOverhaul.Items.HalibutCannonEcType.Tooltip").Value;
            string[] legendtopsList = textContent.Split("\n");
            foreach (string legendtops in legendtopsList) {
                string text = legendtops;
                int index = InWorldBossPhase.Instance.Halibut_Level();
                TooltipLine newLine = new TooltipLine(CWRMod.Instance, "CWRText", text);
                if (newLine.Text == "[Text]") {
                    text = index >= 0 && index <= 14 ? CWRLocText.GetTextValue($"Halibut_TextDictionary_Content_{index}") : "ERROR";

                    if (!CWRServerConfig.Instance.WeaponEnhancementSystem) {
                        text = InWorldBossPhase.Level11 ? CWRLocText.GetTextValue("Halibut_No_legend_Content_2") : CWRLocText.GetTextValue("Halibut_No_legend_Content_1");
                    }
                    newLine.Text = text;
                    // 使用颜色渐变以提高可读性
                    newLine.OverrideColor = Color.Lerp(Color.BlueViolet, Color.White, 0.5f + (float)Math.Sin(Main.GlobalTimeWrappedHourly) * 0.5f);
                }
                // 将新提示行添加到新集合中
                newTooltips.Add(newLine);
            }
            HalibutCannonEcType.SetTooltip(ref newTooltips, CWRMod.Instance.Name);
            // 清空原 tooltips 集合并添加修改后的新Tooltips集合
            tooltips.Clear();
            tooltips.AddRange(newTooltips);
            tooltips.AddRange(prefixTooltips);
        }
    }
}
