using Godot;

namespace DungeonAdventure.Characters;

public interface ICharacterController
{
    public Vector2 GetMoveDirection();
    public AttackSide? GetAttackDirection();
}