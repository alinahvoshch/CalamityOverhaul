﻿using CalamityOverhaul.Common;
using InnoVault.UIHandles;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;

namespace CalamityOverhaul.Content.UIs.SupertableUIs
{
    internal class MaterialOrganizer : UIHandle
    {
        protected SupertableUI mainUI => UIHandleLoader.GetUIHandleOfType<SupertableUI>();
        public override Texture2D Texture => CWRUtils.GetT2DValue("CalamityOverhaul/Assets/UIs/SupertableUIs/OneClick");
        public override float RenderPriority => 2;
        public override bool Active {
            get {
                if (SupertableUI.Instance == null) {
                    return false;
                }

                return SupertableUI.Instance.Active;
            }
        }
        protected virtual Vector2 offsetDraw => new Vector2(570, 330);
        private int useTimeCoolding;
        private int useMuse3AddCount;
        private bool checkSetO => GetType() != typeof(MaterialOrganizer);

        public override void Update() {
            // 更新当前绘制位置和矩形
            DrawPosition = mainUI.DrawPosition + offsetDraw;
            UIHitBox = new Rectangle((int)DrawPosition.X, (int)DrawPosition.Y, 30, 30);
            // 判断鼠标是否在主矩形内
            hoverInMainPage = UIHitBox.Intersects(new Rectangle((int)MousePosition.X, (int)MousePosition.Y, 1, 1));
            //int mouseState = DownStartL();
            int mouseState = (int)keyLeftPressState;
            // 重置鼠标点击计数器
            if (mouseState != 1 && mouseState != 3) {
                useMuse3AddCount = 30;
            }
            // 当鼠标在主区域内时，处理点击事件
            if (hoverInMainPage) {
                if (mouseState == 1 || mouseState == 3) {
                    HandleClickEvents(mouseState);
                }
                DragButton.DontDragTime = 2;
            }
            // 冷却时间递减
            if (useTimeCoolding > 0) {
                useTimeCoolding--;
            }
        }
        private void HandleClickEvents(int mouseState) {
            if (checkSetO) {
                // 如果有物品在使用，执行点击事件
                bool isItemInUse = mainUI.items.Any(item => item.type != ItemID.None);
                if (isItemInUse) {
                    ClickEvent();
                }
            }
            else if (useTimeCoolding <= 0 || mouseState == 1) {
                // 执行点击事件并处理冷却
                ClickEvent();
                useTimeCoolding = useMuse3AddCount;
                AdjustMouseClickSpeed();
            }
        }
        private void AdjustMouseClickSpeed() {
            // 调整鼠标点击速度
            if (useMuse3AddCount == 30) {
                useMuse3AddCount = 12;
            }
            else {
                useMuse3AddCount = Math.Max(useMuse3AddCount - 1, 1);
            }
        }
        protected virtual void ClickEvent() {
            mainUI.OneClickPFunc();
            mainUI.FinalizeCraftingResult();
        }
        public override void Draw(SpriteBatch spriteBatch) {
            Color color = Color.White;
            if (hoverInMainPage) {
                color = Color.Gold;
            }
            spriteBatch.Draw(Texture, DrawPosition, null, color, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            if (hoverInMainPage) {
                Utils.DrawBorderStringFourWay(spriteBatch, FontAssets.MouseText.Value
                , checkSetO ? CWRLocText.GetTextValue("SupMUI_OneClick_Text2") : CWRLocText.GetTextValue("SupMUI_OneClick_Text1")
                , DrawPosition.X - 30, DrawPosition.Y + 30, Color.White, Color.Black, new Vector2(0.3f), 0.8f);
            }
        }
    }
}
