using Godot;

namespace DungeonAdventure.Characters.Effects;

public abstract partial class Effect : Node
{
    private float _duration;
    
    public Effect(float duration)
    {
        _duration = duration;
    }

    public override void _Ready()
    {
        GetTree().CreateTimer(_duration).Timeout += () => QueueFree();
    }
    
    public abstract void Apply(float delta, Character character, CharacterStats stats);
}