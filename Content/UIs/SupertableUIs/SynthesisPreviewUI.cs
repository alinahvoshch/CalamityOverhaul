﻿using CalamityOverhaul.Common;
using CalamityOverhaul.Content.Items.Placeable;
using InnoVault.UIHandles;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace CalamityOverhaul.Content.UIs.SupertableUIs
{
    public class SynthesisPreviewUI : UIHandle
    {
        public static SynthesisPreviewUI Instance => UIHandleLoader.GetUIHandleOfType<SynthesisPreviewUI>();
        public override LayersModeEnum LayersMode => LayersModeEnum.None;
        public Texture2D mainBookPValue => CWRUtils.GetT2DValue("CalamityOverhaul/Assets/UIs/SupertableUIs/BookPans");
        public Texture2D mainCellValue => CWRUtils.GetT2DValue("CalamityOverhaul/Assets/UIs/SupertableUIs/MainValue3");
        public Texture2D TOMTex => CWRUtils.GetT2DValue(CWRConstant.Asset + "Items/Placeable/" + "TransmutationOfMatterItem");
        public Vector2 DrawPos;
        public bool DrawBool;
        public bool OnSupTale => SupertableUI.Instance.hoverInMainPage || SupertableUI.Instance.hoverInPutItemCellPage || SupertableUI.Instance.onInputP || SupertableUI.Instance.onCloseP;
        /// <summary>
        /// 在只利用一个数字索引的情况下反向计算出对应的格坐标
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Vector2 ArcCellPos(int index, Vector2 pos) {
            int y = index / 9;
            int x = index - (y * 9);
            return (new Vector2(x, y) * new Vector2(48, 46)) + pos;
        }

        public Vector2 Prevention(Vector2 pos) {
            float maxW = mainBookPValue.Width * 2.2f;
            float maxH = mainBookPValue.Height * 2.5f;
            if (pos.X < 0) {
                pos.X = 0;
            }
            if (pos.X + maxW > Main.screenWidth) {
                pos.X = Main.screenWidth - maxW;
            }
            if (pos.Y < 0) {
                pos.Y = 0;
            }
            if (pos.Y + maxH > Main.screenHeight) {
                pos.Y = Main.screenHeight - maxH;
            }
            return pos;
        }

        public void DrawHander(Item item, string[] OmigaSnyContent) {
            if (SupertableUI.Instance == null || OmigaSnyContent == null) {
                return;
            }
            Vector2 drawPos = new Vector2(580, 100);
            if (SupertableUI.Instance.Active) {
                drawPos = SupertableUI.Instance.DrawPosition - new Vector2(Instance.mainBookPValue.Width * 2.2f, 0);
            }
            if (!SupertableUI.Instance.hoverInMainPage) {
                Instance.Draw(Main.spriteBatch, drawPos, OmigaSnyContent);
                SynthesisPreviewStart.Instance.Update();
                SynthesisPreviewStart.Instance.Draw(Main.spriteBatch);
                SynthesisPreviewStart.doDraw = true;
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 drawPos, string[] names) {
            if (!DrawBool) {
                return;
            }

            if (!SupertableUI.Instance.Active) {
                SupertableUI.Instance.hoverInMainPage =
                SupertableUI.Instance.hoverInPutItemCellPage =
                SupertableUI.Instance.onInputP =
                SupertableUI.Instance.onCloseP = false;
            }
            DrawPos = Prevention(drawPos);
            Vector2 offset = new Vector2(100, 100);
            Item[] items = new Item[names.Length];
            Item targetItem = SupertableUI.InStrGetItem(names[names.Length - 1], true);
            for (int i = 0; i < names.Length - 1; i++) {
                string name = names[i];
                Item item = SupertableUI.InStrGetItem(name, true);
                items[i] = item;
            }

            Vector2 drawMainUISize = new Vector2(2.2f, 2.6f);

            VaultUtils.DrawBorderedRectangle(spriteBatch, CWRAsset.UI_JAR.Value, 4, DrawPos
                , (int)(mainBookPValue.Width * 2.2f), (int)(mainBookPValue.Height * 2.5f), Color.BlueViolet * 0.8f, Color.Azure * 0.2f, 1);
            VaultUtils.DrawBorderedRectangle(spriteBatch, CWRAsset.Placeholder_White.Value, 4, DrawPos
                , (int)(mainBookPValue.Width * 2.2f), (int)(mainBookPValue.Height * 2.5f), Color.BlueViolet * 0, Color.CadetBlue * 0.6f, 1);

            spriteBatch.Draw(mainCellValue, DrawPos + new Vector2(-25, -25) + offset, null, Color.White * 0.8f, 0, Vector2.Zero, 1, SpriteEffects.None, 0);

            Vector2 drawTOMItemIconPos = DrawPos + new Vector2(-20, mainCellValue.Height + 10) + offset;
            VaultUtils.SimpleDrawItem(spriteBatch, ModContent.ItemType<TransmutationOfMatterItem>(), drawTOMItemIconPos, 1, 0, Color.White);

            Vector2 drawText1 = new Vector2(DrawPos.X - 20, DrawPos.Y - 60) + offset;
            Utils.DrawBorderStringFourWay(spriteBatch, FontAssets.MouseText.Value,
                $"{CWRLocText.GetTextValue("SPU_Text0") + CWRUtils.SafeGetItemName<TransmutationOfMatterItem>() + CWRLocText.GetTextValue("SPU_Text1")}："
                , drawText1.X, drawText1.Y, Color.White, Color.Black, new Vector2(0.3f), 1f);

            if (targetItem != null) {
                Vector2 drawText2 = new Vector2(DrawPos.X + 16, DrawPos.Y + 420) + offset;
                string text = $"{CWRLocText.GetTextValue("SPU_Text2") + CWRUtils.SafeGetItemName(targetItem.type)}";
                Vector2 size = FontAssets.MouseText.Value.MeasureString(text);
                Utils.DrawBorderStringFourWay(spriteBatch, FontAssets.MouseText.Value, text, drawText2.X, drawText2.Y, Color.White, Color.Black, new Vector2(0.3f), 1f);
                Vector2 drawItemPos = drawText2 + new Vector2(size.X + 20, 8);
                SupertableUI.DrawItemIcons(spriteBatch, targetItem, drawItemPos, new Vector2(0.0001f, 0.0001f));

                if (targetItem.type == ModContent.ItemType<InfiniteToiletItem>()) {
                    text = CWRLocText.GetTextValue("OnlyZenith");
                    Utils.DrawBorderStringFourWay(spriteBatch, FontAssets.MouseText.Value, text, drawText2.X, drawText2.Y + size.Y, Color.Coral, Color.Black, new Vector2(0.3f), 1f);
                }
            }

            for (int i = 0; i < items.Length - 1; i++) {//遍历绘制出UI格中的所有预览物品
                if (items[i] != null) {
                    Item item = items[i];
                    if (item != null) {
                        SupertableUI.DrawItemIcons(spriteBatch, item, ArcCellPos(i, DrawPos + offset), new Vector2(0.0001f, 0.0001f), Color.White * 0.9f);
                    }
                }
            }
        }
    }
}
