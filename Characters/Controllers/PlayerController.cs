using DungeonAdventure.Characters.Models;
using DungeonAdventure.Characters.Views;
using Godot;

namespace DungeonAdventure.Characters.Controllers;

public class PlayerController : CharacterController
{
    private const string PlayerGroup = "player";

    private const string MoveUpAction = "move_up";
    private const string MoveDownAction = "move_down";
    private const string MoveLeftAction = "move_left";
    private const string MoveRightAction = "move_right";
    
    private const string AttackUpAction = "attack_up";
    private const string AttackDownAction = "attack_down";
    private const string AttackLeftAction = "attack_left";
    private const string AttackRightAction = "attack_right";
    
    
    public PlayerController(CharacterView character, CharacterModel model) : base(character, model)
    {
        character.AddToGroup(PlayerGroup);
    }

    public override bool IsPlayer => true;

    public override Vector2 GetMoveDirection()
    {
        Vector2 direction = new Vector2(0, 0);

        if (Input.IsActionPressed(MoveUpAction))
        {
            direction += new Vector2(0, -1);
        }

        if (Input.IsActionPressed(MoveDownAction))
        {
            direction += new Vector2(0, 1);
        }
		
        if (Input.IsActionPressed(MoveLeftAction))
        {
            direction += new Vector2(-1, 0);
        }
		
        if (Input.IsActionPressed(MoveRightAction))
        {
            direction += new Vector2(1, 0);
        }

        direction = direction.Normalized();
        return direction;
    }

    public override Vector2? GetAttackDirection()
    {
        Vector2 direction = Vector2.Zero;
		
        if (Input.IsActionPressed(AttackRightAction))
        {
            direction += new Vector2(1, 0);
        }
        if (Input.IsActionPressed(AttackLeftAction))
        {
            direction += new Vector2(-1, 0);
        }
        if (Input.IsActionPressed(AttackUpAction))
        {
            direction += new Vector2(0, -1);
        }
        if (Input.IsActionPressed(AttackDownAction))
        {
            direction += new Vector2(0, 1);
        }

        if (direction.IsZeroApprox())
            return null;

        return direction.Normalized();
    }
}