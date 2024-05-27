using DungeonAdventure.Characters;
using DungeonAdventure.Items;
using Godot;

namespace DungeonAdventure.World.Generation;

[GlobalClass]
public partial class MapGenerationSettings : Resource
{
    [Export] public int NumberOfRooms { get; set; } = 20;

    [Export] public PackedScene CharacterScene { get; set; }
    [Export] public CharacterModelFactory[] Enemies { get; set; }
    
    [Export] public PackedScene ItemObjectScene { get; set; }

}