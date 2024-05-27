using Godot;

namespace DungeonAdventure.Weapons.View;

/// <summary>
/// Represents an arrow shot by a bow in the game.
/// </summary>
public partial class Arrow : Area2D
{
    /// <summary>
    /// The speed of the arrow.
    /// </summary>
    [Export] private float _speed = 500f;

    private BowView _bow;
    private float _damage;

    /// <summary>
    /// Initializes the arrow with the specified bow and damage.
    /// </summary>
    /// <param name="bow">The bow that shot the arrow.</param>
    /// <param name="damage">The damage the arrow will inflict.</param>
    public void Initialize(BowView bow, float damage)
    {
        _bow = bow;
        _damage = damage;
    }
    
    /// <summary>
    /// Called when the node is added to the scene.
    /// </summary>
    public override void _Ready()
    {
        AreaEntered += OnCollision;
        BodyEntered += OnCollision;
    }

    /// <summary>
    /// Processes the physics-related logic each frame.
    /// </summary>
    /// <param name="delta">The elapsed time since the last frame.</param>
    public override void _PhysicsProcess(double delta)
    {
        Vector2 direction = GlobalTransform.BasisXform(Vector2.Right);
        GlobalTranslate(direction * _speed * (float)delta);
    }

    /// <summary>
    /// Handles the collision of the arrow with another body.
    /// </summary>
    /// <param name="body">The body that the arrow collides with.</param>
    private void OnCollision(Node2D body)
    {
        if (_bow.OnArrowCollision(body, _damage))
            QueueFree();
    }
}