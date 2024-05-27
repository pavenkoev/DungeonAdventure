using DungeonAdventure.Characters.Models;
using DungeonAdventure.Characters.Views;
using Godot;

namespace DungeonAdventure.Characters.Controllers;

/// <summary>
/// Represents a controller for player characters in the game.
/// </summary>
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
    
    /// <summary>
    /// Initializes a new instance of the <see cref="PlayerController"/> class.
    /// </summary>
    /// <param name="character">The view component associated with the player character.</param>
    /// <param name="model">The model component associated with the player character.</param>
    public PlayerController(CharacterView character, CharacterModel model) : base(character, model)
    {
        character.AddToGroup(PlayerGroup);
    }

    /// <summary>
    /// Gets a value indicating whether this character is a player.
    /// </summary>
    public override bool IsPlayer => true;

    /// <summary>
    /// Gets the direction for player character movement based on input actions.
    /// </summary>
    /// <returns>A <see cref="Vector2"/> representing the movement direction.</returns>
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

    /// <summary>
    /// Gets the direction for player character attack based on input actions.
    /// </summary>
    /// <returns>A nullable <see cref="Vector2"/> representing the attack direction.</returns>
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