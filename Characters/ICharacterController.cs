using Godot;

namespace DungeonAdventure.Characters;

public interface ICharacterController
{
    public void PhysicsProcess(double delta);
    public Vector2 GetMoveDirection();
    public AttackSide? GetAttackDirection();
}