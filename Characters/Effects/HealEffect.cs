namespace DungeonAdventure.Characters.Effects;

/// <summary>
/// Represents an effect that heals a character over time.
/// </summary>
public partial class HealEffect : Effect
{
    /// <summary>
    /// The rate at which the character is healed.
    /// </summary>
    private float _healRate;

    /// <summary>
    /// Initializes a new instance of the <see cref="HealEffect"/> class with default values.
    /// </summary>
    public HealEffect() : this(50, 5) {}
    
    /// <summary>
    /// Initializes a new instance of the <see cref="HealEffect"/> class with specified values.
    /// </summary>
    /// <param name="healAmount">The total amount to heal over the duration.</param>
    /// <param name="duration">The duration of the effect in seconds.</param>
    public HealEffect(float healAmount, float duration) : base(duration)
    {
        _healRate = healAmount / duration;
    }
    
    /// <summary>
    /// Applies the heal effect to the specified character.
    /// </summary>
    /// <param name="delta">The elapsed time since the last frame.</param>
    /// <param name="character">The character view to which the effect is applied.</param>
    /// <param name="stats">The character stats to be modified by the effect.</param>
    public override void Apply(float delta, Views.CharacterView character, CharacterStats stats)
    {
        stats.HealRate += _healRate * delta;
    }
}