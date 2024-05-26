namespace DungeonAdventure.Characters.Effects;

public partial class HealEffect : Effect
{
    private float _healRate;

    public HealEffect(float healAmount, float duration) : base(duration)
    {
        _healRate = healAmount / duration;
    }
    
    public override void Apply(float delta, Views.CharacterView character, CharacterStats stats)
    {
        stats.HealRate += _healRate * delta;
    }
}