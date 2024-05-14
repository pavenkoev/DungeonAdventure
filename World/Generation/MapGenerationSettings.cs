using Godot;

namespace DungeonAdventure.World.Generation;

[GlobalClass]
public partial class MapGenerationSettings : Resource
{
    [Export] public int NumberOfRooms { get; private set; } = 20;

    [Export] public PackedScene[] Enemies { get; private set; }
}