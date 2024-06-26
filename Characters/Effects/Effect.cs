using DungeonAdventure.Characters.Views;
using Godot;

namespace DungeonAdventure.Characters.Effects;

/// <summary>
/// An abstract base class for effects applied to characters in the game.
/// </summary>
public abstract partial class Effect : Node2D
{
    /// <summary>
    /// The character to which the effect is applied.
    /// </summary>
    protected CharacterView Character;
    
    /// <summary>
    /// The duration of the effect in seconds.
    /// </summary>
    [Export] public float Duration { get; private set; }

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
        Duration = duration;
    }

    /// <summary>
    /// Called when the node is added to the scene. Sets up the timer for the effect duration.
    /// </summary>
    public override void _Ready()
    {
        if (Duration > 0)
            GetTree().CreateTimer(Duration).Timeout += () => QueueFree();
        
        OnStart();
    }

    /// <summary>
    /// Called when the node is about to exit the scene tree.
    /// </summary>
    public override void _ExitTree()
    {
        OnEnd();
    }

    /// <summary>
    /// Sets the character to which the effect will be applied.
    /// </summary>
    /// <param name="character">The character to set.</param>
    public void SetCharacter(CharacterView character)
    {
        Character = character;
    }
    
    /// <summary>
    /// Applies the effect to the specified character.
    /// </summary>
    /// <param name="delta">The elapsed time since the last frame.</param>
    /// <param name="character">The character view to which the effect is applied.</param>
    /// <param name="stats">The character stats to be modified by the effect.</param>
    public abstract void Apply(float delta, Views.CharacterView character, CharacterStats stats);

    /// <summary>
    /// Called when the effect starts. Override to implement custom start logic.
    /// </summary>
    protected virtual void OnStart() {}
    
    /// <summary>
    /// Called when the effect ends. Override to implement custom end logic.
    /// </summary>
    protected virtual void OnEnd() {}
}