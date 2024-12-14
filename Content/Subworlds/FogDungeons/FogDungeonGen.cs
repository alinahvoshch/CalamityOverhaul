using Terraria.IO;
using Terraria.WorldBuilding;

internal class FogDungeonGen : GenPass
{
    private const int WorldWidth = 2000;
    private const int WorldHeight = 2000;
    private const int RoomMinSize = 8;
    private const int RoomMaxSize = 16;
    private const int MainRoomSize = 50;

    public FogDungeonGen() : base("Fog Dungeon", 1) { }

    protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration) {
        progress.Message = "正在创建地牢...";
    }
}
