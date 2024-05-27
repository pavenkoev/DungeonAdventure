using DungeonAdventure.Characters;
using DungeonAdventure.Characters.Views;
using Godot;

namespace DungeonAdventure.Items;

/// <summary>
/// Represents an abstract base class for items in the game.
/// </summary>
[Tool]
[GlobalClass]
public abstract partial class Item : Resource
{
    /// <summary>
    /// Gets the icon texture of the item.
    /// </summary>
    [Export] public Texture2D Icon { get; private set; }
    
    /// <summary>
    /// Gets the visual representation of the item.
    /// </summary>
    [Export] public PackedScene Visual { get; private set; }

    /// <summary>
    /// Determines whether the item can be used by the specified character.
    /// </summary>
    /// <param name="character">The character view to check.</param>
    /// <returns>True if the item can be used, otherwise false.</returns>
    public virtual bool CanUse(CharacterView character) => true;

    /// <summary>
    /// Uses the item on the specified character.
    /// </summary>
    /// <param name="character">The character on which to use the item.</param>
    public abstract void Use(CharacterView character);
}