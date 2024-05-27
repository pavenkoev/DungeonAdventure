using Godot;

namespace DungeonAdventure.World.Placeholders;

/// <summary>
/// Represents a placeholder for item spawn locations in the dungeon.
/// </summary>
public partial class ItemPlaceholder : Placeholder
{
    /// <summary>
    /// Gets the item pool associated with this placeholder.
    /// </summary>
    [Export] public ItemPlaceholderItemPool ItemPool { get; private set; }
    
    /// <summary>
    /// The probability of an item spawning at this placeholder.
    /// </summary>
    [Export] public float Probability = 0.5f;
}