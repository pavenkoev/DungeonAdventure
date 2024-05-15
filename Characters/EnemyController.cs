using System;
using DungeonAdventure.Characters;
using DungeonAdventure.Utils;
using Godot;
using Godot.Collections;

namespace DungeonAdventure.Characters;

public class EnemyController : ICharacterController
{
    private readonly float _chaseDistance;
    private readonly Character _character;
    private Vector2 _nextPathPosition;
    
    public EnemyController(Character character, float chaseDistance)
    {
        _character = character;
        _chaseDistance = chaseDistance;
    }

    public void PhysicsProcess(double delta)
    {
        Character player = _character.FindPlayer();
        _character.NavigationAgent.TargetPosition = player.Position;
        if (NavigationServer2D.MapIsActive(_character.NavigationAgent.GetNavigationMap()))
            _nextPathPosition = _character.NavigationAgent.GetNextPathPosition();
    }

    public Vector2 GetMoveDirection()
    {
        Character player = _character.FindPlayer();
        if (player == null)
            return Vector2.Zero;

        Vector2 direction = Vector2.Zero;
        Vector2 characterPosition = _character.Position;

        if (player.Position.DistanceTo(characterPosition) <= _chaseDistance)
            direction = (_nextPathPosition - characterPosition).Normalized();

        return direction;
    }

    public Vector2? GetAttackDirection()
    {
        Character player = _character.FindPlayer();
        Vector2 vector = player.Position - _character.Position;

        if (vector.Length() <= _character.Weapon.AttackRange)
        {
            return vector.Normalized();
        }

        return null;
    }
}