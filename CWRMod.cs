global using InnoVault;
global using Microsoft.Xna.Framework;
using CalamityOverhaul.Content.NPCs.Core;
using CalamityOverhaul.Content.RemakeItems.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.Core;

namespace CalamityOverhaul
{
    public class CWRMod : Mod
    {
        #region Date
        //-HoCha113 - 2024/9/19/ 3:45
        //不要使用惰性加载，这是愚蠢的，要知道有的Mod会在外部调用这个，
        //或者有的钩子是往InnoVault上挂载的，那个时候这个单例很可能还没来得及加载，然后把一切都毁掉
        //-Chram - 2024/9/20/13:45
        //不，只要注意就行，这个字段被调用的频率极高，使用惰性加载是个不错的习惯，我们只需要自己注意
        //并提醒别人不要在错误的线程上调用这个单例就行了
        //-HoCha113 - 2024/9/20/ 14:32
        //神皇在上，这是异端发言，你不能把整个系统的安危寄托在所有人可以遵守开发守则上，况且我们根本没有那个东西
        internal static CWRMod Instance { get; private set; }
        internal static bool Suitableversion_improveGame { get; private set; }
        internal static List<Mod> LoadMods { get; private set; }
        internal static List<ICWRLoader> ILoaders { get; private set; } = [];
        internal static List<ItemOverride> ItemOverrideInstances { get; private set; } = [];
        internal static List<NPCCustomizer> NPCCustomizerInstances { get; private set; } = [];
        internal static Dictionary<int, ItemOverride> ItemIDToOverrideDic { get; private set; } = [];
        internal static GlobalHookList<GlobalItem> CWR_InItemLoader_Set_Shoot_Hook { get; private set; }
        internal static GlobalHookList<GlobalItem> CWR_InItemLoader_Set_CanUse_Hook { get; private set; }
        internal static GlobalHookList<GlobalItem> CWR_InItemLoader_Set_UseItem_Hook { get; private set; }
        internal Mod musicMod = null;
        internal Mod betterWaveSkipper = null;
        internal Mod fargowiltasSouls = null;
        internal Mod catalystMod = null;
        internal Mod weaponOut = null;
        internal Mod weaponDisplay = null;
        internal Mod weaponDisplayLite = null;
        internal Mod magicBuilder = null;
        internal Mod magicStorage = null;
        internal Mod improveGame = null;
        internal Mod luiafk = null;
        internal Mod terrariaOverhaul = null;
        internal Mod thoriumMod = null;
        internal Mod narakuEye = null;
        internal Mod coolerItemVisualEffect = null;
        internal Mod gravityDontFlipScreen = null;
        internal Mod infernum = null;

        #endregion

        public override object Call(params object[] args) => ModCall.Hander(args);

        public override void PostSetupContent() {
            {
                LoadMods = [.. ModLoader.Mods];
            }

            {
                foreach (var rItem in ItemOverrideInstances) {
                    rItem.SetReadonlyTargetID = rItem.TargetID;
                    rItem.SetStaticDefaults();
                    if (rItem.CanLoadLocalization) {
                        _ = rItem.DisplayName;
                        _ = rItem.Tooltip;
                    }
                }
            }

            {
                ItemIDToOverrideDic = [];
                foreach (ItemOverride ritem in ItemOverrideInstances) {
                    ItemIDToOverrideDic.Add(ritem.SetReadonlyTargetID, ritem);
                }
                Instance.Logger.Info($"{ItemIDToOverrideDic.Count} key pair is loaded into the RItemIndsDict");
            }

            {
                NPCCustomizerInstances = VaultUtils.GetSubclassInstances<NPCCustomizer>();
            }

            {
                GlobalHookList<GlobalItem> getItemLoaderHookTargetValue(string key)
                    => (GlobalHookList<GlobalItem>)typeof(ItemLoader).GetField(key, BindingFlags.NonPublic | BindingFlags.Static)?.GetValue(null);
                CWR_InItemLoader_Set_Shoot_Hook = getItemLoaderHookTargetValue("HookShoot");
                CWR_InItemLoader_Set_CanUse_Hook = getItemLoaderHookTargetValue("HookCanUseItem");
                CWR_InItemLoader_Set_UseItem_Hook = getItemLoaderHookTargetValue("HookUseItem");
            }

            {
                Suitableversion_improveGame = false;
                if (improveGame != null) {
                    Suitableversion_improveGame = improveGame.Version >= new Version(1, 7, 1, 7);
                }
            }

            //加载一次ID列表，从这里加载可以保障所有内容已经添加好了
            CWRLoad.Setup();
            foreach (var i in ILoaders) {
                i.SetupData();
                if (!Main.dedServ) {
                    i.LoadAsset();
                }
            }
        }

        public override void Load() {
            Instance = this;
            FindMod();

            ILoaders = VaultUtils.GetSubInterface<ICWRLoader>();
            foreach (var setup in ILoaders) {
                setup.LoadData();
            }
        }

        public override void Unload() {
            foreach (var setup in ILoaders) {
                setup.UnLoadData();
            }

            emptyMod();
            LoadMods?.Clear();
            ILoaders?.Clear();
            ItemOverrideInstances?.Clear();
            ItemIDToOverrideDic?.Clear();
            NPCCustomizerInstances?.Clear();
            CWR_InItemLoader_Set_Shoot_Hook = null;
            CWR_InItemLoader_Set_CanUse_Hook = null;
            CWR_InItemLoader_Set_UseItem_Hook = null;
            CWRLoad.UnLoad();
            Instance = null;
        }

        public override void HandlePacket(BinaryReader reader, int whoAmI) => CWRNetWork.HandlePacket(this, reader, whoAmI);

        private void emptyMod() {
            musicMod = null;
            betterWaveSkipper = null;
            fargowiltasSouls = null;
            catalystMod = null;
            weaponOut = null;
            weaponDisplay = null;
            magicBuilder = null;
            improveGame = null;
            luiafk = null;
            terrariaOverhaul = null;
            thoriumMod = null;
            narakuEye = null;
            coolerItemVisualEffect = null;
            gravityDontFlipScreen = null;
            infernum = null;
        }

        public void FindMod() {
            emptyMod();
            ModLoader.TryGetMod("CalamityModMusic", out musicMod);
            ModLoader.TryGetMod("BetterWaveSkipper", out betterWaveSkipper);
            ModLoader.TryGetMod("FargowiltasSouls", out fargowiltasSouls);
            ModLoader.TryGetMod("CatalystMod", out catalystMod);
            ModLoader.TryGetMod("WeaponOut", out weaponOut);
            ModLoader.TryGetMod("WeaponDisplay", out weaponDisplay);
            ModLoader.TryGetMod("WeaponDisplayLite", out weaponDisplayLite);
            ModLoader.TryGetMod("MagicBuilder", out magicBuilder);
            ModLoader.TryGetMod("MagicStorage", out magicStorage);
            ModLoader.TryGetMod("ImproveGame", out improveGame);
            ModLoader.TryGetMod("miningcracks_take_on_luiafk", out luiafk);
            ModLoader.TryGetMod("TerrariaOverhaul", out terrariaOverhaul);
            ModLoader.TryGetMod("ThoriumMod", out thoriumMod);
            ModLoader.TryGetMod("NarakuEye", out narakuEye);
            ModLoader.TryGetMod("CoolerItemVisualEffect", out coolerItemVisualEffect);
            ModLoader.TryGetMod("GravityDontFlipScreen", out gravityDontFlipScreen);
            ModLoader.TryGetMod("InfernumMode", out infernum);
        }
    }
}