using DungeonAdventure.Characters;
using DungeonAdventure.Items;
using Godot;

namespace DungeonAdventure.World.Generation;

[GlobalClass]
public partial class MapGenerationSettings : Resource
{
    [Export] public int NumberOfRooms { get; private set; } = 20;

    [Export] public PackedScene CharacterScene { get; private set; }
    [Export] public CharacterModelFactory[] Enemies { get; private set; }
    
    [Export] public PackedScene ItemObjectScene { get; private set; }

}