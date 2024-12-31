﻿using CalamityMod;
using CalamityOverhaul.Common;
using CalamityOverhaul.Content.Projectiles.Weapons.Ranged.Core;
using InnoVault.UIHandles;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;

namespace CalamityOverhaul.Content.UIs
{
    internal class CartridgeHolderUI : UIHandle
    {
        public static CartridgeHolderUI Instance => UIHandleLoader.GetUIHandleOfType<CartridgeHolderUI>();
        public static Texture2D TextureValue;
        public static float JARSengs;
        private Item handItem => player.ActiveItem();
        private int bulletNum => player.ActiveItem().CWR().NumberBullets;
        private float otherPotData;
        private int Time;
        public override bool Active {
            get {
                if (!CWRServerConfig.Instance.MagazineSystem) {
                    return false;
                }
                return handItem.type == ItemID.None ? false : handItem.CWR().HasCartridgeHolder;
            }
        }

        public override void Update() {
            CWRItems cwrItem = handItem.CWR();
            if (TextureValue != null) {
                UIHitBox = new Rectangle((int)DrawPosition.X, (int)DrawPosition.Y, TextureValue.Width, TextureValue.Height);
                if (cwrItem.CartridgeType == CartridgeUIEnum.Magazines)
                    UIHitBox = new Rectangle((int)DrawPosition.X, (int)DrawPosition.Y, TextureValue.Width, TextureValue.Height / 6);
                hoverInMainPage = UIHitBox.Intersects(new Rectangle((int)MousePosition.X, (int)MousePosition.Y, 1, 1));
            }
            if (hoverInMainPage) {
                bool mr2 = true;
                if (player.CWR().TryGetInds_BaseFeederGun(out BaseFeederGun gun)) {
                    if (gun.SafeMousetStart2) {
                        mr2 = false;
                    }
                }
                //int mr = DownStartR();
                int mr = (int)keyRightPressState;
                if (mr == 1 && mr2) {
                    SoundEngine.PlaySound(CWRSound.loadTheRounds, player.Center);
                    foreach (Item i in cwrItem.MagazineContents) {
                        if (i.type == ItemID.None || i.stack <= 0) {
                            continue;
                        }
                        if (!i.CWR().AmmoProjectileReturn) {
                            continue;
                        }
                        player.QuickSpawnItem(player.FromObjectGetParent(), new Item(i.type), i.stack);
                    }
                    cwrItem.InitializeMagazine();
                    cwrItem.SpecialAmmoState = SpecialAmmoStateEnum.ordinary;
                }
            }

            if (player.CWR().PlayerIsKreLoadTime > 0) {
                otherPotData -= 0.8f;
            }
            else {
                otherPotData = 0;
            }

            Time++;
        }

        public void Initialize() {
            CWRItems cwrItem = handItem.CWR();
            if (cwrItem.CartridgeType == CartridgeUIEnum.CartridgeHolder) {
                DrawPosition = new Vector2(20, Main.screenHeight - 100);
                string key = "BulletCard";
                string key2 = "";
                if (cwrItem.SpecialAmmoState == SpecialAmmoStateEnum.napalmBomb) {
                    key2 = "_napalmBomb";
                }
                if (cwrItem.SpecialAmmoState == SpecialAmmoStateEnum.armourPiercer) {
                    key2 = "_armourPiercer";
                }
                if (cwrItem.SpecialAmmoState == SpecialAmmoStateEnum.highExplosive) {
                    key2 = "_highExplosive";
                }
                if (cwrItem.SpecialAmmoState == SpecialAmmoStateEnum.dragonBreath) {
                    key2 = "_dragonBreath";
                }
                if (handItem.useAmmo == AmmoID.Rocket) {
                    key = "GrenadeRound";
                }
                TextureValue = CWRUtils.GetT2DValue($"CalamityOverhaul/Assets/UIs/{key}" + key2);
            }
            if (cwrItem.CartridgeType == CartridgeUIEnum.Magazines) {
                DrawPosition = new Vector2(60, Main.screenHeight - 100);
                TextureValue = CWRUtils.GetT2DValue("CalamityOverhaul/Assets/UIs/Magazines");
            }
            if (cwrItem.CartridgeType == CartridgeUIEnum.JAR) {
                DrawPosition = new Vector2(60, Main.screenHeight - 100);
                TextureValue = CWRUtils.GetT2DValue("CalamityOverhaul/Assets/UIs/JAR");
            }
            DrawPosition += new Vector2(CWRServerConfig.Instance.CartridgeUI_Offset_X_Value
                , -CWRServerConfig.Instance.CartridgeUI_Offset_Y_Value);
        }

        public override void Draw(SpriteBatch spriteBatch) {
            Initialize();
            CWRItems cwrItem = handItem.CWR();
            if (cwrItem.CartridgeType == CartridgeUIEnum.CartridgeHolder) {
                int offsetX = 0;
                if (handItem.useAmmo == AmmoID.Rocket) {
                    offsetX = 10;
                }
                spriteBatch.Draw(TextureValue, DrawPosition, null, Color.White, 0f, Vector2.Zero, 1, SpriteEffects.None, 0);
                Utils.DrawBorderStringFourWay(spriteBatch, FontAssets.ItemStack.Value, bulletNum.ToString()
                    , DrawPosition.X + 50 + offsetX, DrawPosition.Y + 0, Color.AliceBlue, Color.Black, Vector2.Zero, 1.3f);
                Utils.DrawBorderStringFourWay(spriteBatch, FontAssets.ItemStack.Value, "Max"
                    , DrawPosition.X + 50 + offsetX, DrawPosition.Y + 22, Color.Gold, Color.Black, Vector2.Zero, 1f);
                Utils.DrawBorderStringFourWay(spriteBatch, FontAssets.ItemStack.Value, cwrItem.AmmoCapacity.ToString()
                    , DrawPosition.X + 85 + offsetX, DrawPosition.Y + 22, Color.Gold, Color.Black, Vector2.Zero, 1.05f);
            }
            if (cwrItem.CartridgeType == CartridgeUIEnum.Magazines) {
                Rectangle rectangle = CWRUtils.GetRec(TextureValue, 6 - bulletNum, 7);
                spriteBatch.Draw(TextureValue, DrawPosition + rectangle.Size() / 2, rectangle, Color.White
                    , otherPotData, rectangle.Size() / 2, 1, SpriteEffects.None, 0);
                Utils.DrawBorderStringFourWay(spriteBatch, FontAssets.ItemStack.Value, bulletNum.ToString()
                    , DrawPosition.X + 50, DrawPosition.Y + 0, Color.AliceBlue, Color.Black, Vector2.Zero, 1.3f);
                Utils.DrawBorderStringFourWay(spriteBatch, FontAssets.ItemStack.Value, "Max"
                    , DrawPosition.X + 50, DrawPosition.Y + 22, Color.Gold, Color.Black, Vector2.Zero, 1f);
                Utils.DrawBorderStringFourWay(spriteBatch, FontAssets.ItemStack.Value, cwrItem.AmmoCapacity.ToString()
                    , DrawPosition.X + 85, DrawPosition.Y + 22, Color.Gold, Color.Black, Vector2.Zero, 1.05f);
            }
            if (cwrItem.CartridgeType == CartridgeUIEnum.JAR) {
                Texture2D jar2 = CWRUtils.GetT2DValue("CalamityOverhaul/Assets/UIs/JAR_Full");
                Texture2D ctb = CWRUtils.GetT2DValue("CalamityOverhaul/Assets/UIs/JAR_CTB");
                JARSengs = MathHelper.Lerp(JARSengs, bulletNum / (float)cwrItem.AmmoCapacity, 0.05f);
                float sengs = jar2.Height * (1 - JARSengs);
                Rectangle rectangle = new(0, (int)sengs, jar2.Width, (int)(jar2.Height - sengs) + 1);
                spriteBatch.Draw(TextureValue, DrawPosition, null, Color.White, 0f, Vector2.Zero, 1, SpriteEffects.None, 0);
                spriteBatch.Draw(jar2, DrawPosition + new Vector2(4, sengs + 6), rectangle, Color.White, 0f, Vector2.Zero, 1, SpriteEffects.None, 0);
                spriteBatch.Draw(ctb, DrawPosition + new Vector2(4, 6), null, Color.White, 0f, Vector2.Zero, 1, SpriteEffects.None, 0);
            }
            if (hoverInMainPage) {
                string text = $"{CWRLocText.GetTextValue("CartridgeHolderUI_Text1")}\n";
                int value = 0;
                if (cwrItem.MagazineContents != null && cwrItem.MagazineContents.Length > 0) {
                    foreach (Item i in cwrItem.MagazineContents) {
                        if (i == null) {
                            continue;
                        }
                        if (i.type != ItemID.None && i.ammo != AmmoID.None) {
                            text += $"{i.Name} {CWRLocText.GetTextValue("CartridgeHolderUI_Text2")}: {i.stack}\n";
                            value++;
                        }
                    }
                }
                if (value == 0) {
                    text += CWRLocText.GetTextValue("CartridgeHolderUI_Text3");
                    value = 1;
                }

                Utils.DrawBorderStringFourWay(spriteBatch, FontAssets.ItemStack.Value, text
                    , MousePosition.X + 0, MousePosition.Y - 40 - value * 24, Color.AliceBlue, Color.Black, Vector2.Zero, 1f);
                Utils.DrawBorderStringFourWay(spriteBatch, FontAssets.ItemStack.Value, CWRLocText.GetTextValue("CartridgeHolderUI_Text4")
                    , MousePosition.X + 0, MousePosition.Y + 50, Color.Goldenrod, Color.Black, Vector2.Zero, 1.1f);
            }
        }
    }
}
