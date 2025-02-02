﻿using CalamityMod.Events;
using CalamityMod.World;
using CalamityOverhaul.Common;
using CalamityOverhaul.Content.RemakeItems.Core;
using Terraria;
using Terraria.ID;

namespace CalamityOverhaul.Content.RemakeItems
{
    internal class RMagicConch : ItemOverride
    {
        public override int TargetID => ItemID.MagicConch;
        public override bool DrawingInfo => false;
        public override bool? On_CanUseItem(Item item, Player player) => DontInBossUseItem(player);
        public static bool? DontInBossUseItem(Player player) {
            if (CalamityWorld.death || BossRushEvent.BossRushActive) {
                bool myIsBossTarget = false;
                foreach (var npc in Main.ActiveNPCs) {
                    if (npc.boss) {
                        myIsBossTarget = npc.target == player.whoAmI;
                        break;
                    }
                }
                if (myIsBossTarget) {
                    if (player.whoAmI == Main.myPlayer) {
                        VaultUtils.Text(CWRLocText.GetTextValue("DontUseMagicConch"));
                    }
                    return false;
                }
            }
            return null;
        }
    }

    internal class RDemonConch : ItemOverride
    {
        public override int TargetID => ItemID.MagicConch;
        public override bool DrawingInfo => false;
        public override bool? On_CanUseItem(Item item, Player player) => RMagicConch.DontInBossUseItem(player);
    }
}
