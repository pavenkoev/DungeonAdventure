using DungeonAdventure.Characters.Views;
using DungeonAdventure.Items;
using Godot;

namespace DungeonAdventure.World.Rooms.PillarInheritance.Support;

/// <summary>
/// Represents an item that restores the character's weapon.
/// </summary>
public partial class CharacterWeaponItem : Item
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CharacterWeaponItem"/> class with the specified weapon name.
    /// </summary>
    /// <param name="weaponName">The name of the visual to be loaded.</param>
    public CharacterWeaponItem(string weaponName)
    {
        string scenePath = $"res://Weapons/{weaponName.ToLower()}.tscn";
        Visual = GD.Load<PackedScene>(scenePath);
    }
    
    public override void Use(CharacterView character)
    {
        
    }
}