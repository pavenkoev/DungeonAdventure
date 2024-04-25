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

    public AttackSide? GetAttackDirection()
    {
        AttackSide? attackSide = null;
		
        if (Input.IsActionJustPressed("attack_right"))
        {
            attackSide = AttackSide.Right;
        } else if (Input.IsActionJustPressed("attack_left"))
        {
            attackSide = AttackSide.Left;
        } else if (Input.IsActionJustPressed("attack_up"))
        {
            attackSide = AttackSide.Up;
        } else if (Input.IsActionJustPressed("attack_down"))
        {
            attackSide = AttackSide.Down;
        }

        return attackSide;
    }
}