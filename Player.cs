using System;
using Godot;

namespace DungeonAdventure;

public partial class Player : CharacterBody2D
{
	[Export] private float _speed = 128.0f;
	[Export] private AnimatedSprite2D _sprite;

	[Export] private Sword _weapon;

	[Export] private Node2D _weaponPivotUp;
	[Export] private Node2D _weaponPivotDown;
	[Export] private Node2D _weaponPivotLeft;
	[Export] private Node2D _weaponPivotRight;
	
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
		bool isAttacking = false;
		
		if (Input.IsActionJustPressed("attack_right"))
		{
			_weapon.Reparent(_weaponPivotRight, false);
			isAttacking = true;
		} else if (Input.IsActionJustPressed("attack_left"))
		{
			_weapon.Reparent(_weaponPivotLeft, false);
			isAttacking = true;
		} else if (Input.IsActionJustPressed("attack_up"))
		{
			_weapon.Reparent(_weaponPivotUp, false);
			isAttacking = true;
		} else if (Input.IsActionJustPressed("attack_down"))
		{
			_weapon.Reparent(_weaponPivotDown, false);
			isAttacking = true;
		}

		if (isAttacking)
		{
			_weapon.Attack();
		}
	}
	
	private void UpdateAnimation(Vector2 velocity)
	{
		if (!velocity.IsZeroApprox())
		{
			_sprite.Play("run");
			_sprite.FlipH = velocity.X < 0;
		}
		else
		{
			_sprite.Play("idle");
		}
	}
}
