using Godot;

namespace DungeonAdventure.Weapons;

public partial class Arrow : Area2D
{
    [Export] private float _speed = 500f;

    private Bow _bow;

    public void Initialize(Bow bow)
    {
        _bow = bow;
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
        if (_bow.OnArrowCollision(body))
            QueueFree();
    }

}