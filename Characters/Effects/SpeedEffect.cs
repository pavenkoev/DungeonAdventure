namespace DungeonAdventure.Characters.Effects;

/// <summary>
/// Represents an effect that modifies the speed of a character.
/// </summary>
public partial class SpeedEffect : Effect
{
    /// <summary>
    /// The factor by which the speed is modified.
    /// </summary>
    private float _factor;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="SpeedEffect"/> class with default values.
    /// </summary>
    public SpeedEffect() : this(1.5f, 5) {}
    
    /// <summary>
    /// Initializes a new instance of the <see cref="SpeedEffect"/> class with specified values.
    /// </summary>
    /// <param name="factor">The factor by which the speed is modified.</param>
    /// <param name="duration">The duration of the effect in seconds.</param>
    public SpeedEffect(float factor, float duration) : base(duration)
    {
        _factor = factor;
    }

    /// <summary>
    /// Applies the speed effect to the specified character.
    /// </summary>
    /// <param name="delta">The elapsed time since the last frame.</param>
    /// <param name="character">The character view to which the effect is applied.</param>
    /// <param name="stats">The character stats to be modified by the effect.</param>
    public override void Apply(float delta, Views.CharacterView character, CharacterStats stats)
    {
        stats.SpeedModifier *= _factor;
    }
}