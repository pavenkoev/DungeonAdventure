using System;
using DungeonAdventure.Characters;
using Godot;
using Godot.Collections;

namespace DungeonAdventure.Characters;

public class EnemyController : ICharacterController
{
    private float _chaseDistance;
    private Character _character;
    private Vector2 _nextPathPosition;

    public EnemyController(Character character, float chaseDistance)
    {
        _character = character;
        _chaseDistance = chaseDistance;
    }

    public void PhysicsProcess(double delta)
    {
        Character player = FindPlayer();
        _character.NavigationAgent.TargetPosition = player.Position;
        if (NavigationServer2D.MapIsActive(_character.NavigationAgent.GetNavigationMap()))
            _nextPathPosition = _character.NavigationAgent.GetNextPathPosition();
    }

    public Vector2 GetMoveDirection()
    {
        Character player = FindPlayer();
        Vector2 direction = new Vector2(0, 0);
        Vector2 characterPosition = _character.Position;

        if (player.Position.DistanceTo(characterPosition) <= _chaseDistance)
            direction = (_nextPathPosition - characterPosition).Normalized();

        return direction;
    }

    public AttackSide? GetAttackDirection()
    {
        Character player = FindPlayer();
        Vector2 vector = player.Position - _character.Position;

        if (vector.Length() <= _character.Weapon.AttackRange)
        {
            return SelectAttackSide(vector);
        }

        return null;
    }
    
    private Character FindPlayer()
    {
        Array<Node> nodes = _character.GetTree().GetNodesInGroup("player");
        return nodes[0] as Character;
    }
    
    private AttackSide SelectAttackSide(Vector2 vector)
    {
        if (vector.Y >= vector.X)
        {
            if (vector.Y >= -vector.X)
                return AttackSide.Down;
            return AttackSide.Left;
        }

        if (vector.Y >= -vector.X)
            return AttackSide.Right;
        return AttackSide.Up;
    }
}