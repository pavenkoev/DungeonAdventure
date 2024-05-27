using Godot;

namespace DungeonAdventure.Characters.Effects;

/// <summary>
/// An abstract base class for effects applied to characters in the game.
/// </summary>
public abstract partial class Effect : Node
{
    /// <summary>
    /// The duration of the effect in seconds.
    /// </summary>
    private float _duration;

    /// <summary>
    /// Initializes a new instance of the <see cref="Effect"/> class with a default duration.
    /// </summary>
    public Effect() : this(1) {}
    
    /// <summary>
    /// Initializes a new instance of the <see cref="Effect"/> class with a specified duration.
    /// </summary>
    /// <param name="duration">The duration of the effect in seconds.</param>
    public Effect(float duration)
    {
        _duration = duration;
    }

    /// <summary>
    /// Called when the node is added to the scene. Sets up the timer for the effect duration.
    /// </summary>
    public override void _Ready()
    {
        GetTree().CreateTimer(_duration).Timeout += () => QueueFree();
    }
    
    /// <summary>
    /// Applies the effect to the specified character.
    /// </summary>
    /// <param name="delta">The elapsed time since the last frame.</param>
    /// <param name="character">The character view to which the effect is applied.</param>
    /// <param name="stats">The character stats to be modified by the effect.</param>

    public abstract void Apply(float delta, Views.CharacterView character, CharacterStats stats);
}