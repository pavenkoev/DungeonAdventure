using Godot;

namespace DungeonAdventure.Characters.Views;

/// <summary>
/// Represents the visual aspects of a character in the game.
/// </summary>
public partial class CharacterVisual : Node2D
{
    /// <summary>
    /// Gets the sprite associated with the character visual.
    /// </summary>
    [Export] public Sprite2D Sprite { get; private set; }
    
    /// <summary>
    /// Gets the animation player associated with the character visual.
    /// </summary>
    [Export] public AnimationPlayer AnimationPlayer { get; private set; }
}