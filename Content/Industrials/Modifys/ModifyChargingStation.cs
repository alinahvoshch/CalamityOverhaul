﻿using CalamityMod;
using CalamityMod.Items;
using CalamityMod.Items.DraedonMisc;
using CalamityMod.Items.Placeables.DraedonStructures;
using CalamityMod.Tiles.DraedonStructures;
using CalamityOverhaul.Content.Industrials.MaterialFlow;
using CalamityOverhaul.Content.RemakeItems.Core;
using CalamityOverhaul.Content.Tiles.Core;
using InnoVault.TileProcessors;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityOverhaul.Content.Industrials.Modifys
{
    internal class ModifyChargingStationItem : ItemOverride
    {
        public override int TargetID => ModContent.ItemType<ChargingStationItem>();
        public override void SetDefaults(Item item) {
            item.CWR().StorageUE = true;
            item.CWR().ConsumeUseUE = 1000;
        }
    }

    internal class ModifyChargingStation : TileOverride
    {
        public override int TargetID => ModContent.TileType<ChargingStation>();
        public override bool? CanDrop(int i, int j, int type) => false;
        public override bool? RightClick(int i, int j, Tile tile) {
            if (!TileProcessorLoader.AutoPositionGetTP<ChargingStationTP>(i, j, out var tp)) {
                return false;
            }
            //在没打开UI的情况下，如果拿着东西右键可以直接放上去
            if ((!tp.OpenUI && !Main.LocalPlayer.GetItem().IsAir) || Main.keyState.PressingShift()) {
                if (tp is ChargingStationTP chargingStation) {
                    chargingStation.RightEvent();
                }
                return false;
            }

            tp.OpenUI = !tp.OpenUI;
            //如果是开启，就先关掉其他所有的同类实体的UI
            if (tp.OpenUI) {
                foreach (var tp2 in TileProcessorLoader.TP_InWorld) {
                    if (tp2.ID != tp.ID || tp2.WhoAmI == tp.WhoAmI) {
                        continue;
                    }
                    if (tp2 is ChargingStationTP chargingStation) {
                        chargingStation.OpenUI = false;
                    }
                }
            }

            SoundEngine.PlaySound(SoundID.MenuTick);
            return false;
        }
    }

    internal class ChargingStationTP : BaseBattery, ICWRLoader//是的，把这个东西当成是一个电池会更好写
    {
        public override int TargetTileID => ModContent.TileType<ChargingStation>();
        internal static Asset<Texture2D> Panel { get; private set; }
        internal static Asset<Texture2D> SlotTex { get; private set; }
        internal static Asset<Texture2D> BarTop { get; private set; }
        internal static Asset<Texture2D> BarFull { get; private set; }
        internal static Asset<Texture2D> EmptySlot { get; private set; }
        internal bool OpenUI;
        internal float sengs;
        private bool hoverBar;
        private bool hoverChargeBar;
        private bool hoverSlot;
        private bool hoverEmptySlot;
        private bool boverPanel;
        private bool oldLeftDown;
        private Vector2 MousePos;
        internal Item Item = new Item();
        internal Item Empty = new Item();
        public override bool CanDrop => false;
        public override float MaxUEValue => 1000;
        public override int TargetItem => ModContent.ItemType<ChargingStationItem>();
        void ICWRLoader.LoadAsset() {
            Panel = CWRUtils.GetT2DAsset(CWRConstant.UI + "Generator/GeneratorPanel");
            SlotTex = CWRUtils.GetT2DAsset("CalamityMod/UI/DraedonsArsenal/ChargerWeaponSlot");
            BarTop = CWRUtils.GetT2DAsset("CalamityMod/UI/DraedonsArsenal/ChargeMeterBorder");
            BarFull = CWRUtils.GetT2DAsset("CalamityMod/UI/DraedonsArsenal/ChargeMeter");
            EmptySlot = CWRUtils.GetT2DAsset("CalamityMod/UI/DraedonsArsenal/PowerCellSlot_Empty");
        }
        void ICWRLoader.UnLoadData() {
            Panel = null;
            SlotTex = null;
            BarTop = null;
            BarFull = null;
            EmptySlot = null;
        }

        public void RightEvent() {
            Item item = Main.LocalPlayer.GetItem();

            if (Main.keyState.PressingShift()) {
                if (!Item.IsAir && !VaultUtils.isClient) {
                    int type = Item.NewItem(new EntitySource_WorldEvent(), HitBox, Item.Clone());
                    if (!VaultUtils.isSinglePlayer) {
                        NetMessage.SendData(MessageID.SyncItem, -1, -1, null, type, 0f, 0f, 0f, 0, 0, 0);
                    }
                }
                Item.TurnToAir();
                SoundEngine.PlaySound(SoundID.Grab);
                return;
            }

            if (item.IsAir) {
                return;
            }

            if (!Item.IsAir && !VaultUtils.isClient) {
                int type = Item.NewItem(new EntitySource_WorldEvent(), HitBox, Item.Clone());
                if (!VaultUtils.isSinglePlayer) {
                    NetMessage.SendData(MessageID.SyncItem, -1, -1, null, type, 0f, 0f, 0f, 0, 0, 0);
                }
                Item.TurnToAir();
            }

            Item = item.Clone();
            item.TurnToAir();
            SoundEngine.PlaySound(SoundID.Grab);
        }

        public override void Update() {
            if (OpenUI) {
                if (sengs < 1f) {
                    sengs += 0.1f;
                }
                else {
                    UpdateUI();
                }
            }
            else if (sengs > 0f) {
                sengs -= 0.1f;
            }

            if (!Item.IsAir) {
                ChargeWeapon();
            }

            if (!Empty.IsAir) {
                if (MachineData.UEvalue < MaxUEValue - 50) {
                    Empty.stack--;
                    MachineData.UEvalue += 50;
                    MachineData.UEvalue = MathHelper.Clamp(MachineData.UEvalue, 0, MaxUEValue);
                }
            }
        }

        private void UpdateUI() {
            Vector2 drawPos = CenterInWorld + new Vector2(0, -120) * sengs;
            Rectangle mouseRec = Main.MouseWorld.GetRectangle(1);
            hoverSlot = (drawPos - SlotTex.Size() / 2).GetRectangle(SlotTex.Size()).Intersects(mouseRec);
            hoverEmptySlot = (drawPos - SlotTex.Size() / 2 + new Vector2(-56, 0)).GetRectangle(EmptySlot.Size()).Intersects(mouseRec);
            boverPanel = (drawPos - Panel.Size() / 2 * 0.75f).GetRectangle(Panel.Size() * 0.75f).Intersects(mouseRec);
            Vector2 barChargePos = drawPos + new Vector2(40, -30);
            drawPos += new Vector2(-30, 30);
            hoverBar = drawPos.GetRectangle(60, 22).Intersects(mouseRec);
            hoverChargeBar = barChargePos.GetRectangle(30, 62).Intersects(mouseRec);
            MousePos = Main.MouseScreen;

            if (boverPanel) {
                Main.LocalPlayer.mouseInterface = true;
                Main.LocalPlayer.CWR().DontUseItemTime = 2;
            }

            bool justDown = !oldLeftDown && Main.mouseLeft;
            oldLeftDown = Main.mouseLeft;

            if (!Main.mouseItem.IsAir) {
                if (hoverSlot && justDown) {
                    if (Main.mouseItem.Calamity().UsesCharge) {
                        HandlerSlotItem(ref Item);
                    }
                    else {
                        SoundEngine.PlaySound(SoundID.MenuClose);
                        CombatText.NewText(HitBox, new Color(111, 247, 200), "需要放入能够接收能量的物品", false);
                    }
                }

                if (hoverEmptySlot && justDown) {
                    if (Main.mouseItem.type == ModContent.ItemType<DraedonPowerCell>()) {
                        HandlerSlotItem(ref Empty);
                    }
                    else {
                        SoundEngine.PlaySound(SoundID.MenuClose);
                        CombatText.NewText(HitBox, new Color(111, 247, 200), "需要放入电池", false);
                    }
                }
            }
        }

        private void HandlerSlotItem(ref Item setItem) {
            SoundEngine.PlaySound(SoundID.Grab);

            if (setItem.type == ItemID.None) {
                setItem = Main.mouseItem.Clone();
                Main.mouseItem.TurnToAir();
            }
            else {
                if (setItem.type == Main.mouseItem.type) {
                    setItem.stack += Main.mouseItem.stack;
                    Main.mouseItem.TurnToAir();
                }
                else {
                    Item swopItem = setItem.Clone();
                    setItem = Main.mouseItem.Clone();
                    Main.mouseItem = swopItem;
                }
            }
        }

        private void ChargeWeapon() {
            if (MachineData.UEvalue < 0.1f) {
                return;
            }
            CalamityGlobalItem calamityItem = Item.Calamity();
            if (calamityItem.UsesCharge) {
                if (calamityItem.Charge < calamityItem.MaxCharge) {
                    calamityItem.Charge += 0.1f;
                    MachineData.UEvalue -= 0.1f;
                    SpawnDust();
                }
                calamityItem.Charge = MathHelper.Clamp(calamityItem.Charge, 0, calamityItem.MaxCharge);
            }
        }

        private void SpawnDust() {
            int dustID = 182;
            int numDust = 8;
            for (int i = 0; i < numDust; i += 2) {
                float pairSpeed = Main.rand.NextFloat(0.5f, 7f);
                float pairScale = 1f;

                Dust d = Dust.NewDustDirect(CenterInWorld, 0, 0, dustID);
                d.velocity = Vector2.UnitX * pairSpeed;
                d.scale = pairScale;
                d.noGravity = true;

                d = Dust.NewDustDirect(CenterInWorld, 0, 0, dustID);
                d.velocity = Vector2.UnitX * -pairSpeed;
                d.scale = pairScale;
                d.noGravity = true;
            }
        }

        public override void MachineKill() {
            if (VaultUtils.isClient) {
                return;
            }

            int type;
            if (!Item.IsAir) {
                type = Item.NewItem(new EntitySource_WorldEvent(), HitBox, Item);
                Item.TurnToAir();
                if (VaultUtils.isServer) {
                    NetMessage.SendData(MessageID.SyncItem, -1, -1, null, type, 0f, 0f, 0f, 0, 0, 0);
                }
            }

            if (!Empty.IsAir) {
                type = Item.NewItem(new EntitySource_WorldEvent(), HitBox, Empty);
                Empty.TurnToAir();
                if (VaultUtils.isServer) {
                    NetMessage.SendData(MessageID.SyncItem, -1, -1, null, type, 0f, 0f, 0f, 0, 0, 0);
                }
            }

            Item chargingStation = new Item(ModContent.ItemType<ChargingStationItem>());
            chargingStation.CWR().UEValue = MachineData.UEvalue;
            type = Item.NewItem(new EntitySource_WorldEvent(), HitBox, chargingStation);
            if (VaultUtils.isServer) {
                NetMessage.SendData(MessageID.SyncItem, -1, -1, null, type, 0f, 0f, 0f, 0, 0, 0);
            }
        }

        private void DrawChargeBar(SpriteBatch spriteBatch, Vector2 drawPos, float ueRatio) {
            Texture2D texture = CWRUtils.GetT2DValue(CWRConstant.UI + "Generator/ElectricPower");
            Texture2D texture2 = CWRUtils.GetT2DValue(CWRConstant.UI + "Generator/ElectricPowerFull");
            Texture2D texture3 = CWRUtils.GetT2DValue(CWRConstant.UI + "Generator/ElectricPowerGlow");

            float uiRatio = 1 - ueRatio;
            Rectangle full = new Rectangle(0, (int)(texture2.Height * uiRatio), texture2.Width, (int)(texture2.Height * ueRatio));

            drawPos += new Vector2(40, -30);
            Vector2 position = drawPos + new Vector2(8, 36 + full.Y) / 2;

            Main.spriteBatch.Draw(texture, drawPos, null, Color.White * sengs, 0, Vector2.Zero, 0.5f, SpriteEffects.None, 0);
            Main.spriteBatch.Draw(texture2, position, full, Color.White * sengs, 0, Vector2.Zero, 0.5f, SpriteEffects.None, 0);
            Main.spriteBatch.Draw(texture3, drawPos, null, Color.White * sengs, 0, Vector2.Zero, 0.5f, SpriteEffects.None, 0);
        }

        public void DrawUI(SpriteBatch spriteBatch) {
            Vector2 drawPos = CenterInWorld - Main.screenPosition + new Vector2(0, -120) * sengs;
            Vector2 emptyPos = drawPos + new Vector2(-56, 0);
            spriteBatch.Draw(Panel.Value, drawPos, null, Color.White, 0, Panel.Size() / 2, 0.75f * sengs, SpriteEffects.None, 0);
            spriteBatch.Draw(SlotTex.Value, drawPos, null, Color.White, 0, SlotTex.Size() / 2, sengs, SpriteEffects.None, 0);
            spriteBatch.Draw(EmptySlot.Value, emptyPos, null, Color.White, 0, SlotTex.Size() / 2, sengs, SpriteEffects.None, 0);

            Vector2 origDrawPos = drawPos;
            drawPos += new Vector2(-30, 30);
            int uiBarByWidthSengs = (int)(BarFull.Value.Width * (MachineData.UEvalue / MaxUEValue));
            // 绘制温度相关的图像
            Rectangle fullRec = new Rectangle(0, 0, uiBarByWidthSengs, BarFull.Value.Height);
            Main.spriteBatch.Draw(BarTop.Value, drawPos, null, Color.White * sengs, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            Main.spriteBatch.Draw(BarFull.Value, drawPos + new Vector2(10, 0), fullRec, Color.White * sengs, 0, Vector2.Zero, 1, SpriteEffects.None, 0);

            if (!Item.IsAir) {
                VaultUtils.SimpleDrawItem(spriteBatch, Item.type, origDrawPos, 34);
                if (Item.stack > 1) {
                    Utils.DrawBorderStringFourWay(spriteBatch, FontAssets.MouseText.Value, Item.stack.ToString()
                    , origDrawPos.X - 10, origDrawPos.Y + 12, Color.White, Color.Black, new Vector2(0.3f), 0.6f);
                }

                CalamityGlobalItem calamityItem = Item.Calamity();
                if (calamityItem.UsesCharge) {
                    DrawChargeBar(spriteBatch, origDrawPos, calamityItem.Charge / calamityItem.MaxCharge);
                    // 如果鼠标在主页面中，显示信息
                    if (hoverChargeBar) {
                        Utils.DrawBorderStringFourWay(spriteBatch, FontAssets.MouseText.Value
                            , (((int)calamityItem.Charge) + "/" + ((int)calamityItem.MaxCharge) + "UE").ToString()
                            , origDrawPos.X + 40, origDrawPos.Y, Color.White, Color.Black, new Vector2(0.3f), 0.5f);
                    }
                }
            }
            else {
                DrawChargeBar(spriteBatch, origDrawPos, 0f);
            }

            if (!Empty.IsAir) {
                VaultUtils.SimpleDrawItem(spriteBatch, Empty.type, emptyPos + new Vector2(-1, 1), 34);
                if (Empty.stack > 1) {
                    Utils.DrawBorderStringFourWay(spriteBatch, FontAssets.MouseText.Value, Empty.stack.ToString()
                    , emptyPos.X - 10, emptyPos.Y + 12, Color.White, Color.Black, new Vector2(0.3f), 0.6f);
                }
            }

            if (hoverBar) {
                Utils.DrawBorderStringFourWay(spriteBatch, FontAssets.MouseText.Value
                    , (((int)MachineData.UEvalue) + "/" + ((int)MaxUEValue) + "UE").ToString()
                    , MousePos.X - 10, MousePos.Y + 20, Color.White, Color.Black, new Vector2(0.3f), 0.6f);
            }

            if (hoverSlot && Item.type == ItemID.None && Main.mouseItem.type == ItemID.None) {
                Utils.DrawBorderStringFourWay(spriteBatch, FontAssets.MouseText.Value, "放入充能物品"
                    , MousePos.X - 10, MousePos.Y + 20, Color.White, Color.Black, new Vector2(0.3f), 0.6f);
            }

            if (hoverEmptySlot && Empty.type == ItemID.None && Main.mouseItem.type == ItemID.None) {
                Utils.DrawBorderStringFourWay(spriteBatch, FontAssets.MouseText.Value, "放入电池"
                    , MousePos.X - 10, MousePos.Y + 20, Color.White, Color.Black, new Vector2(0.3f), 0.6f);
            }
        }

        public override void Draw(SpriteBatch spriteBatch) {
            Vector2 drawPos = CenterInWorld + new Vector2(0, -24);
            if (Item.type > ItemID.None) {
                VaultUtils.SimpleDrawItem(spriteBatch, Item.type, drawPos - Main.screenPosition
                    , 1f, 0, Lighting.GetColor((int)(drawPos.X / 16), (int)(drawPos.Y / 16)));
            }
        }

        public override void FrontDraw(SpriteBatch spriteBatch) {
            if (OpenUI || sengs > 0) {
                DrawUI(spriteBatch);
            }
        }
    }
}
