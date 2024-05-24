using Godot;

namespace DungeonAdventure.Weapons;

public partial class Spell : Area2D
{
    [Export] private float _speed = 500f;
    
    [Export] private VisibleOnScreenNotifier2D _visibilityNotifier;
    
    private Wand _wand;

    public void Initialize(Wand wand)
    {
        _wand = wand;
    }
    
    public override void _Ready()
    {
        AreaEntered += OnCollision;
        BodyEntered += OnCollision;

        _visibilityNotifier.ScreenExited += QueueFree;
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector2 direction = GlobalTransform.BasisXform(Vector2.Right);
        GlobalTranslate(direction * _speed * (float)delta);
    }

    private void OnCollision(Node2D body)
    {
        _wand.OnSpellCollision(body);
    }
}