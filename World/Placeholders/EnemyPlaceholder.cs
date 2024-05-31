using Godot;

namespace DungeonAdventure.World.Placeholders;

/// <summary>
/// Represents a placeholder for enemy spawn locations in the dungeon.
/// </summary>
public partial class EnemyPlaceholder : Placeholder
{
    /// <summary>
    /// The probability of an enemy spawning at this placeholder.
    /// </summary>
    [Export] public float Probability = 0.5f;
}