﻿using CalamityMod.Items.Weapons.Melee;
using CalamityOverhaul.Common;
using CalamityOverhaul.Content.LegendWeapon.MurasamaLegend.MurasamaProj;
using CalamityOverhaul.Content.RemakeItems.Core;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalamityOverhaul.Content.LegendWeapon.MurasamaLegend
{
    internal class RMurasama : BaseRItem
    {
        public override int TargetID => ModContent.ItemType<Murasama>();
        public override int ProtogenesisID => ModContent.ItemType<MurasamaEcType>();
        public override void SetStaticDefaults() {
            Main.RegisterItemAnimation(TargetID, new DrawAnimationVertical(5, 13));
            ItemID.Sets.AnimatesAsSoul[TargetID] = true;
        }
        public override void SetDefaults(Item item) => MurasamaEcType.SetDefaultsFunc(item);

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
            // 创建一个新的集合以防修改 tooltips 集合时产生异常
            List<TooltipLine> newTooltips = new List<TooltipLine>(tooltips);
            List<TooltipLine> prefixTooltips = [];
            // 遍历 tooltips 集合并隐藏特定的提示行
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

            // 获取自定义的文本内容
            string textContent = Language.GetText("Mods.CalamityOverhaul.Items.MurasamaEcType.Tooltip").Value;
            // 拆分传奇提示行的文本内容
            string[] legendtopsList = textContent.Split("\n");
            // 遍历传奇提示行并添加新的提示行
            foreach (string legendtops in legendtopsList) {
                string text = legendtops;
                int index = InWorldBossPhase.Instance.Mura_Level();
                TooltipLine newLine = new TooltipLine(CWRMod.Instance, "CWRText", text);
                if (newLine.Text == "[Text]") {
                    text = index >= 0 && index <= 14 ? CWRLocText.GetTextValue($"Murasama_TextDictionary_Content_{index}") : "ERROR";

                    if (!CWRServerConfig.Instance.WeaponEnhancementSystem) {
                        text = InWorldBossPhase.Instance.level11 ? CWRLocText.GetTextValue("Murasama_No_legend_Content_2") : CWRLocText.GetTextValue("Murasama_No_legend_Content_1");
                    }
                    newLine.Text = text;
                    // 使用颜色渐变以提高可读性
                    newLine.OverrideColor = Color.Lerp(Color.IndianRed, Color.White, 0.5f + (float)Math.Sin(Main.GlobalTimeWrappedHourly) * 0.5f);
                }
                // 将新提示行添加到新集合中
                newTooltips.Add(newLine);
            }

            MurasamaEcType.SetTooltip(ref newTooltips, CWRMod.Instance.Name);
            // 清空原 tooltips 集合并添加修改后的新Tooltips集合
            tooltips.Clear();
            tooltips.AddRange(newTooltips);
            tooltips.AddRange(prefixTooltips);
        }

        public override void ModifyWeaponDamage(Item item, Player player, ref StatModifier damage)
            => CWRUtils.ModifyLegendWeaponDamageFunc(player, item, MurasamaEcType.GetOnDamage, MurasamaEcType.GetStartDamage, ref damage);
        //因为方法表现不稳定，所以重新使用回 ModifyWeaponDamage 而不是 On_ModifyWeaponDamage
        //public override bool On_ModifyWeaponDamage(Item item, Player player, ref StatModifier damage) {
        //    float oldMultiplicative = damage.Multiplicative;
        //    damage *= MurasamaEcType.GetOnDamage / (float)MurasamaEcType.GetStartDamage;
        //    damage /= oldMultiplicative;
        //    return false;
        //}
        public override void ModifyWeaponKnockback(Item item, Player player, ref StatModifier knockback)
            => CWRUtils.ModifyLegendWeaponKnockbackFunc(player, item, MurasamaEcType.GetOnKnockback, MurasamaEcType.GetStartKnockback, ref knockback);

        public override bool? On_ModifyWeaponCrit(Item item, Player player, ref float crit) {
            crit += MurasamaEcType.GetOnCrit;
            return false;
        }

        public override bool On_PreDrawInInventory(Item item, SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale) {
            Texture2D texture;
            if (Main.LocalPlayer.CWR().HeldMurasamaBool) {
                return true;
            }
            else {
                texture = ModContent.Request<Texture2D>("CalamityMod/Items/Weapons/Melee/MurasamaSheathed").Value;
                spriteBatch.Draw(texture, position, null, Color.White, 0f, origin, scale, SpriteEffects.None, 0);
            }
            return false;
        }

        public override bool? On_CanUseItem(Item item, Player player) {
            //在升龙斩或者爆发弹幕存在时不能使用武器
            return player.ownedProjectileCounts[ModContent.ProjectileType<MuraBreakerSlash>()] > 0
                || player.ownedProjectileCounts[ModContent.ProjectileType<MuraTriggerDash>()] > 0
                || player.PressKey(false)
                ? false
                : !CWRServerConfig.Instance.WeaponEnhancementSystem && !InWorldBossPhase.Instance.level11
                ? false
                : player.ownedProjectileCounts[item.shoot] == 0;
        }

        public override bool? Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
            Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<MuraSlashDefault>(), damage, knockback, player.whoAmI, 0f, 0f);
            return false;
        }
    }
}
