using DungeonAdventure.Characters;
using DungeonAdventure.Items;
using Godot;

namespace DungeonAdventure.World.Generation;

/// <summary>
/// Represents the settings used for generating the dungeon map.
/// </summary>
[GlobalClass]
public partial class MapGenerationSettings : Resource
{
    /// <summary>
    /// Gets or sets the number of rooms to generate in the dungeon.
    /// </summary>
    [Export] public int NumberOfRooms { get; set; } = 20;

    /// <summary>
    /// Gets or sets the seed used for map generation.
    /// </summary>
    [Export] public int Seed { get; set; } = 0;

    /// <summary>
    /// Gets or sets the scene for the character view.
    /// </summary>
    [Export] public PackedScene CharacterScene { get; set; }
    
    /// <summary>
    /// Gets or sets the array of enemy character factories.
    /// </summary>
    [Export] public CharacterModelFactory[] Enemies { get; set; }
    
    /// <summary>
    /// Gets or sets the scene for item objects.
    /// </summary>
    [Export] public PackedScene ItemObjectScene { get; set; }
}