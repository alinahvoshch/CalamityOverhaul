﻿using InnoVault.UIHandles;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;

namespace CalamityOverhaul.Content.UIs.SupertableUIs
{
    internal class DragButton : UIHandle, ICWRLoader
    {
        public override Texture2D Texture => CWRAsset.Placeholder_ERROR.Value;
        public static DragButton Instance;
        public override float RenderPriority => 1;
        public override bool Active {
            get {
                if (SupertableUI.Instance == null) {
                    return false;
                }

                return SupertableUI.Instance.Active;
            }
        }
        public Vector2 SupPos => SupertableUI.Instance.DrawPosition;
        public Vector2 InSupPosOffset => new Vector2(554, 380);
        public Vector2 InPosOffsetDragToPos;
        public Vector2 DragVelocity;
        public bool OnMain;
        public static int DontDragTime;
        public static bool OnDrag;
        public override void Load() => Instance = this;
        void ICWRLoader.UnLoadData() => Instance = null;
        public void Initialize() {
            if (DontDragTime > 0) {
                DontDragTime--;
            }
            DrawPosition = SupertableUI.Instance.DrawPosition + InSupPosOffset;
            OnMain = SupertableUI.Instance.onMainP && DontDragTime <= 0;
            if (Main.mouseItem.type > ItemID.None && SupertableUI.Instance.onMainP2) {
                DontDragTime = 2;
                OnDrag = false;
                OnMain = false;
            }
        }

        public override void Update() {
            if (SupertableUI.Instance == null) {
                return;
            }

            Initialize();

            if (OnMain) {
                if (keyLeftPressState == KeyPressState.Pressed && !OnDrag) {//如果玩家刚刚按下鼠标左键，并且此时没有开启拖拽状态
                    OnDrag = true;
                    InPosOffsetDragToPos = DrawPosition.To(MousePosition);//记录此时的偏移向量
                }
            }

            if (OnDrag) {
                if (keyLeftPressState == KeyPressState.Released) {
                    OnDrag = false;
                }
                DragVelocity = (DrawPosition + InPosOffsetDragToPos).To(MousePosition);//更新拖拽的速度
                SupertableUI.Instance.DrawPosition += DragVelocity;
            }
            else {
                DragVelocity = Vector2.Zero;
            }

            Prevention();
        }

        public void Prevention() {
            if (SupertableUI.Instance.DrawPosition.X < 0) {
                SupertableUI.Instance.DrawPosition.X = 0;
            }
            if (SupertableUI.Instance.DrawPosition.X + SupertableUI.Instance.Texture.Width > Main.screenWidth) {
                SupertableUI.Instance.DrawPosition.X = Main.screenWidth - SupertableUI.Instance.Texture.Width;
            }
            if (SupertableUI.Instance.DrawPosition.Y < 0) {
                SupertableUI.Instance.DrawPosition.Y = 0;
            }
            if (SupertableUI.Instance.DrawPosition.Y + SupertableUI.Instance.Texture.Height > Main.screenHeight) {
                SupertableUI.Instance.DrawPosition.Y = Main.screenHeight - SupertableUI.Instance.Texture.Height;
            }
        }
    }
}
