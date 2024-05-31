using DungeonAdventure.Characters.Views;

namespace DungeonAdventure.Characters.Effects;

/// <summary>
/// Represents an effect that modifies the attack rate of a character.
/// </summary>
public partial class AttackRateEffect : Effect
{
    /// <summary>
    /// The factor by which the attack rate is modified.
    /// </summary>
    private float _factor;

    /// <summary>
    /// Initializes a new instance of the <see cref="AttackRateEffect"/> class with default values.
    /// </summary>
    public AttackRateEffect() : this (1.5f, 5) {}
    
    /// <summary>
    /// Initializes a new instance of the <see cref="AttackRateEffect"/> class with specified values.
    /// </summary>
    /// <param name="factor">The factor by which the attack rate is modified.</param>
    /// <param name="duration">The duration of the effect in seconds.</param>
    public AttackRateEffect(float factor, float duration) : base(duration)
    {
        _factor = factor;
    }

    /// <summary>
    /// Applies the attack rate effect to the specified character.
    /// </summary>
    /// <param name="delta">The elapsed time since the last frame.</param>
    /// <param name="character">The character view to which the effect is applied.</param>
    /// <param name="stats">The character stats to be modified by the effect.</param>
    public override void Apply(float delta, Views.CharacterView character, CharacterStats stats)
    {
        stats.AttackRateModifier *= _factor;
    }
}