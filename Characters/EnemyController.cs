using System;
using DungeonAdventure.Characters;
using Godot;
using Godot.Collections;

namespace DungeonAdventure.Characters;

public class EnemyController : ICharacterController
{
    private float _chaseDistance;
    private Character _character;

    public EnemyController(Character character, float chaseDistance)
    {
        _character = character;
        _chaseDistance = chaseDistance;
    }
    
    public Vector2 GetMoveDirection()
    {
        Character player = FindPlayer();
        Vector2 direction = new Vector2(0, 0);
        Vector2 characterPosition = _character.Position;

        if (player.Position.DistanceTo(characterPosition) <= _chaseDistance)
            direction = (player.Position - characterPosition).Normalized();

        return direction;
    }

    public AttackSide? GetAttackDirection()
    {
        Character player = FindPlayer();
        Vector2 vector = player.Position - _character.Position;

        if (vector.Length() <= _character.Weaponn.AttackRange)
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