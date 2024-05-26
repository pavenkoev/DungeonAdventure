using System;

namespace DungeonAdventure.Characters.Effects;

public partial class DamageEffect : Effect
{
    private float _factor;

    public DamageEffect(float factor, float duration) : base(duration)
    {
        _factor = factor;
    }

    public override void Apply(float delta, Views.CharacterView character, CharacterStats stats)
    {
        stats.DamageModifier *= _factor;
    }

}