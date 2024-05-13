using DungeonAdventure.World.Generation;
using Godot;

namespace DungeonAdventure.World;

public partial class Dungeon : Node2D
{
    [Export] private Vector2I _roomDimension = new Vector2I(640, 368);

    public override void _Ready()
    {
        MapGenerator mapGenerator = new();
        Map map = mapGenerator.Generate(20);
        mapGenerator.PrintGrid();
    }

    public void Move(DoorDirection direction)
    {
        Vector2I offset = GetDirectionToOffset(direction);
       Translate(offset * _roomDimension);
    }

    private Vector2I GetDirectionToOffset(DoorDirection direction)
    {
        return direction switch
        {
            DoorDirection.North => new Vector2I(0, 1),
            DoorDirection.East => new Vector2I(-1, 0),
            DoorDirection.South => new Vector2I(0, -1),
            DoorDirection.West => new Vector2I(1, 0)
        };
    }

}