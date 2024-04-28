using DungeonAdventure.Characters;
using DungeonAdventure.Weapons;
using Godot;

namespace DungeonAdventure.Characters;

public partial class Character : CharacterBody2D
{
	[Export] private CharacterControllerFactory _controllerFactory;
	
	[Export] protected float _speed = 80.0f;
	[Export] protected float _health = 100.0f;

	[Export] protected Sprite2D _sprite;
	[Export] protected AnimationPlayer _animationPlayer;
	[Export] protected NavigationAgent2D _navigationAgent;
	[Export] protected Area2D _hitArea;
	
	[Export] protected Sword _weapon;
	
	[Export] protected Node2D _weaponPivotUp;
	[Export] protected Node2D _weaponPivotDown;
	[Export] protected Node2D _weaponPivotLeft;
	[Export] protected Node2D _weaponPivotRight;
	
	private ICharacterController _controller;
	private bool _isAlive = true;

	public Sword Weapon => _weapon;
	public NavigationAgent2D NavigationAgent => _navigationAgent;
	public Area2D HitArea => _hitArea;
	public bool IsAlive => _isAlive;
	
	public override void _Ready()
	{
		_weapon.Attach(this);
		_controller = _controllerFactory.Create(this);
	}
	
	public override void _Process(double delta)
	{
		if (!_isAlive)
			return;

		Vector2 direction = _controller.GetMoveDirection();
		
		Velocity = direction * _speed;
		
		UpdateAnimation(Velocity);
		
		MoveAndSlide();

		ProcessAttack();
	}

	public override void _PhysicsProcess(double delta)
	{
		_controller.PhysicsProcess(delta);
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
			_animationPlayer.Play("run");
			_sprite.FlipH = velocity.X < 0;
		}
		else
		{
			_animationPlayer.Play("idle");
		}
	}
	
	public void ApplyDamage(float damage)
	{
		if (!_isAlive)
			return;
		
		_health -= damage;
		GD.Print("health: " + _health);
		if (_health <= 0)
		{
			Die();
		}
	}

	private void Die()
	{
		_isAlive = false;
		_animationPlayer.Play("death");
		GetTree().CreateTimer(2).Timeout += () => Disappear();
		
		_weapon.QueueFree();
		_weapon = null;
	}

	private void Disappear()
	{
		GetTree().CreateTween()
			.TweenProperty(this, "modulate:a", 0, 1)
			.Finished += () => QueueFree();
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
