using DungeonAdventure.Characters;
using DungeonAdventure.Weapons;
using Godot;

namespace DungeonAdventure.Characters;

public partial class Character : CharacterBody2D
{
	[Export] private CharacterControllerFactory _controllerFactory;
	
	[Export] protected float _speed = 80.0f;
	[Export] protected float _health = 100.0f;
	
	[Export] protected AnimatedSprite2D _sprite;
	
	[Export] protected Sword _weapon;
	
	[Export] protected Node2D _weaponPivotUp;
	[Export] protected Node2D _weaponPivotDown;
	[Export] protected Node2D _weaponPivotLeft;
	[Export] protected Node2D _weaponPivotRight;
	
	private ICharacterController _controller;

	public Sword Weaponn => _weapon;
	
	public override void _Ready()
	{
		_weapon.Attach(this);
		_controller = _controllerFactory.Create(this);
	}
	
	public override void _Process(double delta)
	{
		Vector2 direction = _controller.GetMoveDirection();
		
		Velocity = direction * _speed;
		
		UpdateAnimation(Velocity);
		
		MoveAndSlide();

		ProcessAttack();
	}

	private void ProcessAttack()
	{
		AttackSide? attackside = _controller.GetAttackDirection();
		

		if (attackside.HasValue)
		{
			SetWeaponAttackSide(attackside.Value);

			if (_weapon.CanAttack())
				_weapon.Attack();
		}
	}
	
	protected void UpdateAnimation(Vector2 velocity)
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
	
	public void ApplyDamage(float damage)
	{
		_health -= damage;
		GD.Print("health: " + _health);
		if (_health <= 0)
		{
			QueueFree();
		}
	}

	protected void SetWeaponAttackSide(AttackSide side)
	{
		switch (side)
		{
			case AttackSide.Up:
				_weapon.Reparent(_weaponPivotUp, false);
				break;
			case AttackSide.Down:
				_weapon.Reparent(_weaponPivotDown, false);
				break;
			case AttackSide.Left:
				_weapon.Reparent(_weaponPivotLeft, false);
				break;
			case AttackSide.Right:
				_weapon.Reparent(_weaponPivotRight, false);
				break;
		}
	}
}
