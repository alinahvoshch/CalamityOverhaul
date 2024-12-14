using CalamityMod.Items.SummonItems;
using CalamityOverhaul.Content.RemakeItems.Core;
using CalamityOverhaul.Content.Subworlds.FogDungeons;
using SubworldLibrary;
using Terraria;
using Terraria.ModLoader;

namespace CalamityOverhaul.Content.RemakeItems
{
    internal class RNecroplasmicBeacon : BaseRItem
    {
        public override int TargetID => ModContent.ItemType<NecroplasmicBeacon>();
        public override bool FormulaSubstitution => false;
        public override bool DrawingInfo => false;
        public override bool? On_CanUseItem(Item item, Player player) {
            if (DungeonWorld.Active) {
                return true;
            }
            return base.CanUseItem(item, player);
        }
        public override bool? On_UseItem(Item item, Player player) {
            if (!DungeonWorld.Active) {
                DungeonWorld.Enter();
            }
            else {
                SubworldSystem.Exit();
            }
            return true;
        }
    }
}
