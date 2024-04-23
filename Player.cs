using System;
using Godot;

namespace DungeonAdventure;

public partial class Player : Character
{
	
	
	public override void _Process(double delta)
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
		
		Velocity = direction * _speed;
		
		UpdateAnimation(Velocity);
		
		MoveAndSlide();

		ProcessAttack();
	}

	private void ProcessAttack()
	{
		AttackSide? attackside = null;
		
		if (Input.IsActionJustPressed("attack_right"))
		{
			attackside = AttackSide.Right;
		} else if (Input.IsActionJustPressed("attack_left"))
		{
			attackside = AttackSide.Left;
		} else if (Input.IsActionJustPressed("attack_up"))
		{
			attackside = AttackSide.Up;
		} else if (Input.IsActionJustPressed("attack_down"))
		{
			attackside = AttackSide.Down;
		}

		if (attackside.HasValue)
		{
			SetWeaponAttackSide(attackside.Value);

			if (_weapon.CanAttack())
				_weapon.Attack();
		}
	}
}
