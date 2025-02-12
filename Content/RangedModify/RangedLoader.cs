﻿using CalamityOverhaul.Common;
using CalamityOverhaul.Content.RangedModify.Core;
using CalamityOverhaul.Content.RangedModify.UI;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria;
using Terraria.ID;

namespace CalamityOverhaul.Content.RangedModify
{
    public class RangedLoader : ICWRLoader
    {
        public delegate Item On_ChooseAmmo_Delegate(object obj, Item weapon);
        public static List<GlobalRanged> GlobalRangeds { get; private set; } = [];
        public static Dictionary<Type, Asset<Texture2D>> TypeToGlowAsset { get; private set; } = [];
        void ICWRLoader.LoadData() {
            GlobalRangeds = VaultUtils.GetSubclassInstances<GlobalRanged>();
            MethodBase chooseAmmoMethod = typeof(Player).GetMethod("ChooseAmmo", BindingFlags.Public | BindingFlags.Instance);
            CWRHook.Add(chooseAmmoMethod, OnChooseAmmoHook);
        }
        void ICWRLoader.LoadAsset() {
            var indss = VaultUtils.GetSubclassInstances<BaseHeldRanged>();
            TypeToGlowAsset = [];
            foreach (var ranged in indss) {
                if (ranged.GlowTexPath != "") {
                    TypeToGlowAsset.Add(ranged.GetType(), CWRUtils.GetT2DAsset(ranged.GlowTexPath));
                }
            }
            indss.Clear();
        }
        void ICWRLoader.UnLoadData() {
            GlobalRangeds?.Clear();
            TypeToGlowAsset?.Clear();
        }

        private static void ModifyInCartridgeGun(Item weapon, ref Item ammo) {
            if (!CWRLoad.ItemIsGun[weapon.type]) {
                return;
            }
            CWRItems cwrItem = weapon.CWR();
            if (!cwrItem.HasCartridgeHolder) {
                return;
            }
            //这个部分用于修复弹匣系统的伤害判定，原版只考虑背包内的弹药，所以这里需要进行拦截修改使其考虑到弹匣供弹
            if (CWRServerConfig.Instance.MagazineSystem) {
                Item newAmmo = cwrItem.GetSelectedBullets();
                if (newAmmo.type > ItemID.None) {
                    ammo = newAmmo;
                }
            }

            foreach (var gGun in GlobalRangeds) {
                Item gAmmo = gGun.ChooseAmmo(weapon);
                if (gAmmo != null) {
                    ammo = gAmmo;
                }
            }
        }

        private static void ModifyInBow(Item weapon, ref Item ammo) {
            if (!GlobalBow.BowActive) {
                return;
            }

            if (ArrowHolderUI.targetAmmo == null
                || ArrowHolderUI.targetAmmo.type == ItemID.None
                || ArrowHolderUI.targetAmmo.ammo != AmmoID.Arrow) {
                return;
            }

            foreach (var item in Main.LocalPlayer.inventory) {
                if (item.type != ArrowHolderUI.targetAmmo.type) {
                    continue;
                }
                ammo = item;
                break;
            }
        }

        public static Item OnChooseAmmoHook(On_ChooseAmmo_Delegate orig, object obj, Item weapon) {
            Item ammo = null;

            ModifyInCartridgeGun(weapon, ref ammo);
            ModifyInBow(weapon, ref ammo);

            if (ammo == null) {
                ammo = orig.Invoke(obj, weapon);
            }

            return ammo;
        }
    }
}
