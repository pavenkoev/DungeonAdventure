using DungeonAdventure.Characters.Models;
using DungeonAdventure.Characters.Views;
using DungeonAdventure.Utils;
using Godot;
using Item = DungeonAdventure.Items.Item;

namespace DungeonAdventure.Characters.Controllers;

/// <summary>
/// Represents a controller for enemy characters in the game.
/// </summary>
public class EnemyController : CharacterController
{
    /// <summary>
    /// The distance within which the enemy will start chasing the player.
    /// </summary>
    private readonly float _chaseDistance;
    
    /// <summary>
    /// The next position on the path to the player.
    /// </summary>
    private Vector2 _nextPathPosition;

    /// <summary>
    /// The closest distance an enemy can approach the player.
    /// </summary>
    private const float MinChaseDistance = 20.0f;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="EnemyController"/> class.
    /// </summary>
    /// <param name="character">The view component associated with the enemy character.</param>
    /// <param name="model">The model component associated with the enemy character.</param>
    /// <param name="chaseDistance">The distance within which the enemy will start chasing the player.</param>
    public EnemyController(CharacterView character, CharacterModel model, float chaseDistance = 100) : base(character, model)
    {
        _chaseDistance = chaseDistance;
    }

    /// <summary>
    /// Processes the physics-related logic for the enemy character each frame.
    /// </summary>
    /// <param name="delta">The elapsed time since the last frame.</param>
    public override void PhysicsProcess(double delta)
    {
        CharacterView player = View.FindPlayer();
        if (player == null)
            return;
        
        View.NavigationAgent.TargetPosition = player.Position;
        if (NavigationServer2D.MapIsActive(View.NavigationAgent.GetNavigationMap()))
            _nextPathPosition = View.NavigationAgent.GetNextPathPosition();
    }

    /// <summary>
    /// Gets the direction for enemy character movement.
    /// </summary>
    /// <returns>A <see cref="Vector2"/> representing the movement direction.</returns>
    public override Vector2 GetMoveDirection()
    {
        CharacterView player = View.FindPlayer();
        if (player == null)
            return Vector2.Zero;

        Vector2 direction = Vector2.Zero;
        Vector2 characterPosition = View.Position;

        float distanceToPlayer = player.Position.DistanceTo(characterPosition);
        
        if (distanceToPlayer <= MinChaseDistance)
            return Vector2.Zero;
        
        if (distanceToPlayer <= _chaseDistance)
            direction = (_nextPathPosition - characterPosition).Normalized();

        return direction;
    }

    /// <summary>
    /// Gets the direction for enemy character attack.
    /// </summary>
    /// <returns>A nullable <see cref="Vector2"/> representing the attack direction.</returns>
    public override Vector2? GetAttackDirection()
    {
        CharacterView player = View.FindPlayer();
        if (player == null)
            return null;
        
        Vector2 vector = player.Position - View.Position;

        if (vector.Length() <= View.Weapon.AttackRange)
        {
            return vector.Normalized();
        }

        return null;
    }

    /// <summary>
    /// Determines whether the character can pick up the specified item.
    /// </summary>
    /// <param name="item">The item to pick up.</param>
    /// <returns>True if the character can pick up the item, otherwise false.</returns>
    public override bool CanPickupItem(Item item)
    {
        return false;
    }
}