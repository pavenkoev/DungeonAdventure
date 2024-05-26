using Godot;

namespace DungeonAdventure.Weapons.View;

public partial class Arrow : Area2D
{
    [Export] private float _speed = 500f;

    private BowView _bow;
    private float _damage;

    public void Initialize(BowView bow, float damage)
    {
        _bow = bow;
        _damage = damage;
    }
    
    public override void _Ready()
    {
        AreaEntered += OnCollision;
        BodyEntered += OnCollision;
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector2 direction = GlobalTransform.BasisXform(Vector2.Right);
        GlobalTranslate(direction * _speed * (float)delta);
    }

    private void OnCollision(Node2D body)
    {
        if (_bow.OnArrowCollision(body, _damage))
            QueueFree();
    }

}