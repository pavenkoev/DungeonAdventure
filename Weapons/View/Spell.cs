using Godot;

namespace DungeonAdventure.Weapons.View;

/// <summary>
/// Represents a spell cast by a wand in the game.
/// </summary>
public partial class Spell : Area2D
{
    /// <summary>
    /// The speed of the spell.
    /// </summary>
    [Export] private float _speed = 500f;
    
    /// <summary>
    /// The visibility notifier to determine if the spell is on screen.
    /// </summary>
    [Export] private VisibleOnScreenNotifier2D _visibilityNotifier;
    
    private WandView _wand;
    private float _damage;

    /// <summary>
    /// Initializes the spell with the specified wand and damage.
    /// </summary>
    /// <param name="wand">The wand that cast the spell.</param>
    /// <param name="damage">The damage the spell will inflict.</param>
    public void Initialize(WandView wand, float damage)
    {
        _wand = wand;
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
    /// Processes the logic each frame.
    /// </summary>
    /// <param name="delta">The elapsed time since the last frame.</param>
    public override void _Process(double delta)
    {
        if (!_visibilityNotifier.IsOnScreen())
            QueueFree();
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
    /// Handles the collision of the spell with another body.
    /// </summary>
    /// <param name="body">The body that the spell collides with.</param>
    private void OnCollision(Node2D body)
    {
        _wand.OnSpellCollision(body, _damage);
    }
}