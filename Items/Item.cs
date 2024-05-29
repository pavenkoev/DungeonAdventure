using DungeonAdventure.Characters.Views;
using DungeonAdventure.Items.View;
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
    /// The id of the item to locate item's visual.
    /// </summary>
    public abstract string ItemId { get; }
    
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

    public ItemVisual LoadVisual()
    {
        string path = $"res://Items/{ItemId}.tres";
        if (!ResourceLoader.Exists(path))
            return null;
        return GD.Load<ItemVisual>(path);
    }
}