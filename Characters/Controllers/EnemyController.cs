using DungeonAdventure.Characters.Models;
using DungeonAdventure.Characters.Views;
using DungeonAdventure.Utils;
using Godot;

namespace DungeonAdventure.Characters.Controllers;

public class EnemyController : CharacterController
{
    private readonly float _chaseDistance;
    private Vector2 _nextPathPosition;
    
    public EnemyController(CharacterView character, CharacterModel model, float chaseDistance) : base(character, model)
    {
        _chaseDistance = chaseDistance;
    }

    public override void PhysicsProcess(double delta)
    {
        CharacterView player = View.FindPlayer();
        View.NavigationAgent.TargetPosition = player.Position;
        if (NavigationServer2D.MapIsActive(View.NavigationAgent.GetNavigationMap()))
            _nextPathPosition = View.NavigationAgent.GetNextPathPosition();
    }

    public override Vector2 GetMoveDirection()
    {
        CharacterView player = View.FindPlayer();
        if (player == null)
            return Vector2.Zero;

        Vector2 direction = Vector2.Zero;
        Vector2 characterPosition = View.Position;

        if (player.Position.DistanceTo(characterPosition) <= _chaseDistance)
            direction = (_nextPathPosition - characterPosition).Normalized();

        return direction;
    }

    public override Vector2? GetAttackDirection()
    {
        CharacterView player = View.FindPlayer();
        Vector2 vector = player.Position - View.Position;

        if (vector.Length() <= View.Weapon.AttackRange)
        {
            return vector.Normalized();
        }

        return null;
    }
}