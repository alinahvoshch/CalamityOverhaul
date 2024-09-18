﻿using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace CalamityOverhaul.Content.UIs.Core
{
    public abstract class CWRUIPanel
    {
        /// <summary>
        /// 屏幕的缩放因子，在多数情况下用于矫正屏幕坐标
        /// </summary>
        public static float ScreenScale => Main.maxScreenW / (float)Main.screenWidth;

        /// <summary>
        /// 获取玩家对象，一般为 LocalPlayer ，因为运行UI代码的只有可能是当前段玩家，也就是本地玩家
        /// </summary>
        public static Player player => Main.LocalPlayer;

        /// <summary>
        /// 获取用户的鼠标在屏幕上的位置，这个属性一般在绘制函数以外的地方使用，
        /// 因为绘制函数中不需要屏幕因子的坐标矫正，直接使用 Main.MouseScreen 即可
        /// </summary>
        public virtual Vector2 MousePosition => Main.MouseScreen;

        /// <summary>
        /// 一个纹理的占位，可以重写它用于获取UI的主要纹理
        /// </summary>
        public virtual Texture2D Texture => CWRUtils.GetT2DValue(CWRConstant.Placeholder);

        /// <summary>
        /// 绘制的位置，这一般意味着UI矩形的左上角
        /// </summary>
        public Vector2 DrawPosition;

        /// <summary>
        /// UI的矩形
        /// </summary>
        public Rectangle UIHitBox;

        /// <summary>
        /// 是否绘制文字
        /// </summary>
        public bool drawText;

        /// <summary>
        /// 鼠标状态
        /// </summary>
        public bool onMous;

        /// <summary>
        /// 旧的左键按下状态
        /// </summary>
        public bool oldDownL;

        /// <summary>
        /// 当前的左键按下状态
        /// </summary>
        public bool downL;

        /// <summary>
        /// 旧的右键按下状态
        /// </summary>
        public bool oldDownR;

        /// <summary>
        /// 当前的右键按下状态
        /// </summary>
        public bool downR;

        /// <summary>
        /// 是否需要拖动
        /// </summary>
        public bool drag;

        /// <summary>
        /// 帧索引前置
        /// </summary>
        public int frameCount;

        /// <summary>
        /// 帧索引
        /// </summary>
        public int frame;

        public int time;

        public int time2;

        /// <summary>
        /// UI的宽度
        /// </summary>
        public int wid;

        /// <summary>
        /// UI的高度
        /// </summary>
        public int hig;

        /// <summary>
        /// UI绘制缩放
        /// </summary>
        public float texScale;

        public virtual void Load() { }

        public virtual void Initialize() { }

        public virtual void Update(GameTime gameTime) { }

        public virtual void Draw(SpriteBatch spriteBatch) { }

        /// <summary>
        /// 防止UI脱离屏幕范围
        /// </summary>
        public virtual void PreventionTransgression() {
            int maxH = Main.maxScreenH - hig;
            int maxW = Main.maxScreenW - wid;
            if (DrawPosition.X < 0) {
                DrawPosition.X = 0;
            }

            if (DrawPosition.Y < 0) {
                DrawPosition.Y = 0;
            }

            if (DrawPosition.X > maxW) {
                DrawPosition.X = maxW;
            }

            if (DrawPosition.Y > maxH) {
                DrawPosition.Y = maxH;
            }
        }

        /// <summary>
        /// 检查鼠标是否在UI上
        /// </summary>
        public virtual bool CheckOnMous() {
            return UIHitBox.Intersects(new Rectangle((int)MousePosition.X, (int)MousePosition.Y, 1, 1));
        }

        public virtual int DownStartL() {
            oldDownL = downL;
            downL = player.PressKey();
            return downL && oldDownL ? 3 : downL && !oldDownL ? 1 : oldDownL && !downL ? 2 : 0;
        }

        public virtual int DownStartR() {
            oldDownR = downR;
            downR = player.PressKey(false);
            return downR && oldDownR ? 3 : downR && !oldDownR ? 1 : oldDownR && !downR ? 2 : 0;
        }
    }
}
