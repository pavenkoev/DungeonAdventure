using DungeonAdventure.Characters;
using Godot;

namespace DungeonAdventure.Characters;

public class PlayerController : ICharacterController
{
    private Character _character;

    public PlayerController(Character character)
    {
        _character = character;
        
        _character.AddToGroup("player");
    }

    public void PhysicsProcess(double delta)
    {
  
    }

    public Vector2 GetMoveDirection()
    {
        Vector2 direction = new Vector2(0, 0);

        if (Input.IsActionPressed("move_up"))
        {
            direction += new Vector2(0, -1);
        }

        if (Input.IsActionPressed("move_down"))
        {
            direction += new Vector2(0, 1);
        }
		
        if (Input.IsActionPressed("move_left"))
        {
            direction += new Vector2(-1, 0);
        }
		
        if (Input.IsActionPressed("move_right"))
        {
            direction += new Vector2(1, 0);
        }

        direction = direction.Normalized();
        return direction;
    }

    public Vector2? GetAttackDirection()
    {
        Vector2 direction = Vector2.Zero;
		
        if (Input.IsActionPressed("attack_right"))
        {
            direction += new Vector2(1, 0);
        }
        if (Input.IsActionPressed("attack_left"))
        {
            direction += new Vector2(-1, 0);
        }
        if (Input.IsActionPressed("attack_up"))
        {
            direction += new Vector2(0, -1);
        }
        if (Input.IsActionPressed("attack_down"))
        {
            direction += new Vector2(0, 1);
        }

        if (direction.IsZeroApprox())
            return null;

        return direction.Normalized();
    }
}