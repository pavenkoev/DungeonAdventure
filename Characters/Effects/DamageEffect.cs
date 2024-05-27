using System;

namespace DungeonAdventure.Characters.Effects;

/// <summary>
/// Represents an effect that modifies the damage output of a character.
/// </summary>
public partial class DamageEffect : Effect
{
    /// <summary>
    /// The factor by which the damage is modified.
    /// </summary>
    private float _factor;

    /// <summary>
    /// Initializes a new instance of the <see cref="DamageEffect"/> class with default values.
    /// </summary>
    public DamageEffect() : this (1.5f, 5) {}
    
    /// <summary>
    /// Initializes a new instance of the <see cref="DamageEffect"/> class with specified values.
    /// </summary>
    /// <param name="factor">The factor by which the damage is modified.</param>
    /// <param name="duration">The duration of the effect in seconds.</param>
    public DamageEffect(float factor, float duration) : base(duration)
    {
        _factor = factor;
    }

    /// <summary>
    /// Applies the damage effect to the specified character.
    /// </summary>
    /// <param name="delta">The elapsed time since the last frame.</param>
    /// <param name="character">The character view to which the effect is applied.</param>
    /// <param name="stats">The character stats to be modified by the effect.</param>
    public override void Apply(float delta, Views.CharacterView character, CharacterStats stats)
    {
        stats.DamageModifier *= _factor;
    }
}