﻿using CalamityMod.Items.Weapons.Magic;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.World;
using CalamityOverhaul.Common;
using System;
using System.Reflection;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityOverhaul.Content.Structures
{
    internal class ModifyStructure : ICWRLoader
    {
        internal delegate void SetChest_Delegate(Chest chest, int type, bool hasPlacedLogAndSchematic);
        void ICWRLoader.LoadData() {
            Type draedonStructures = typeof(DraedonStructures);
            MethodInfo getMethod(string key) => draedonStructures.GetMethod(key, BindingFlags.Public | BindingFlags.Static);
            CWRHook.Add(getMethod("FillPlanetoidLaboratoryChest"), OnPlanetoidChest);
            //CWRHook.Add(getMethod("FillPlagueLaboratoryChest"), OnPlagueChest);
        }

        void ICWRLoader.UnLoadData() { }

        private static void AddChestContent(Chest chest, int type, int stack, string text) {
            foreach (var item in chest.item) {
                if (item.type != ItemID.None) {
                    continue;
                }
                CWRMod.Instance.Logger.Info(text);
                item.SetDefaults(type);
                item.stack = stack;
                break;
            }
        }

        private static void OnPlanetoidChest(SetChest_Delegate orig, Chest chest, int type, bool hasPlacedLogAndSchematic) {
            orig.Invoke(chest, type, hasPlacedLogAndSchematic);
            if (hasPlacedLogAndSchematic) {
                AddChestContent(chest, ModContent.ItemType<SHPC>(), 1
                    , VaultUtils.Translation("正在将 SHPC 塞入箱子", "Shoving SHPC into the chest."));
            }
        }

        private static void OnPlagueChest(SetChest_Delegate orig, Chest chest, int type, bool hasPlacedLogAndSchematic) {
            orig.Invoke(chest, type, hasPlacedLogAndSchematic);
            if (hasPlacedLogAndSchematic) {
                AddChestContent(chest, ModContent.ItemType<BlossomFlux>(), 1
                    , VaultUtils.Translation("正在将 Blossom Flux 塞入箱子", "Stuffing Blossom Flux into chest."));
            }
        }
    }
}
