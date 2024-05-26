namespace DungeonAdventure.Characters.Effects;

public partial class SpeedEffect : Effect
{
    private float _factor;
    
    public SpeedEffect(float factor, float duration) : base(duration)
    {
        _factor = factor;
    }

    public override void Apply(float delta, Views.CharacterView character, CharacterStats stats)
    {
        stats.SpeedModifier *= _factor;
    }
}