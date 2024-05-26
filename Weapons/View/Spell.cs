using Godot;

namespace DungeonAdventure.Weapons.View;

public partial class Spell : Area2D
{
    [Export] private float _speed = 500f;
    
    [Export] private VisibleOnScreenNotifier2D _visibilityNotifier;
    
    private WandView _wand;
    private float _damage;

    public void Initialize(WandView wand, float damage)
    {
        _wand = wand;
        _damage = damage;
    }
    
    public override void _Ready()
    {
        AreaEntered += OnCollision;
        BodyEntered += OnCollision;
    }

    public override void _Process(double delta)
    {
        if (!_visibilityNotifier.IsOnScreen())
            QueueFree();
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector2 direction = GlobalTransform.BasisXform(Vector2.Right);
        GlobalTranslate(direction * _speed * (float)delta);
    }

    private void OnCollision(Node2D body)
    {
        _wand.OnSpellCollision(body, _damage);
    }
}