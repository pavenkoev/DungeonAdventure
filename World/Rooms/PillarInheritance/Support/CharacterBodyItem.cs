using DungeonAdventure.Characters.Views;
using DungeonAdventure.Items;
using Godot;

namespace DungeonAdventure.World.Rooms.PillarInheritance.Support;

/// <summary>
/// Represents an item that restores the character's body.
/// </summary>
public partial class CharacterBodyItem : Item
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CharacterBodyItem"/> class with the specified visual name.
    /// </summary>
    /// <param name="visualName">The name of the visual to be loaded.</param>
    public CharacterBodyItem(string visualName)
    {
        string scenePath = $"res://Characters/Visual/{visualName.ToLower()}.tscn";
        Visual = GD.Load<PackedScene>(scenePath);
    }
    
    public override void Use(CharacterView character)
    {
        
    }
}