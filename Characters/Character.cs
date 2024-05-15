using System;
using DungeonAdventure.Characters;
using DungeonAdventure.Utils;
using DungeonAdventure.Weapons;
using Godot;

namespace DungeonAdventure.Characters;

public partial class Character : CharacterBody2D
{
	[Export] private CharacterControllerFactory _controllerFactory;
	
	[Export] private float _speed = 80.0f;
	[Export] private float _health = 100.0f;

	[Export] private Sprite2D _sprite;
	[Export] private AnimationPlayer _animationPlayer;
	[Export] private NavigationAgent2D _navigationAgent;
	[Export] private Area2D _hitArea;
	
	[Export] private Weapon _weapon;
	
	[Export] private Node2D _weaponPivot;
	
	[Export] private AudioStreamPlayer2D _audioPlayer;
	[Export] private AudioStream[] _hitSounds;
	[Export] private AudioStream[] _deathSounds;
	
	private ICharacterController _controller;
	private bool _isAlive = true;

	private const string IdleAnimationName = "idle";
	private const string RunAnimationName = "run";
	private const string DeathAnimationName = "death";
	private const float DisappearDelayOnDeath = 2.0f;
	private const float DisappearDuration = 1.0f;
	private const string DisappearTweenProperty = "modulate:a";

	public Weapon Weapon => _weapon;
	public NavigationAgent2D NavigationAgent => _navigationAgent;
	public Area2D HitArea => _hitArea;
	public CollisionObject2D Collision => this;
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
		Vector2? attackDirection = _controller.GetAttackDirection();
		

		if (attackDirection.HasValue)
		{
			SetWeaponAttackSide(attackDirection.Value);

			if (_weapon.CanAttack())
				_weapon.Attack();
		}
	}
	
	private void UpdateAnimation(Vector2 velocity)
	{
		if (!velocity.IsZeroApprox())
		{
			_animationPlayer.Play(RunAnimationName);
			_sprite.FlipH = velocity.X < 0;
		}
		else
		{
			_animationPlayer.Play(IdleAnimationName);
		}
	}
	
	public void ApplyDamage(float damage)
	{
		if (!_isAlive)
			return;
		
		PlayHitSound();
		
		_health -= damage;
		GD.Print("health: " + _health);
		if (_health <= 0)
		{
			Die();
		}
	}

	private void Die()
	{
		PlayDeathSound();
		
		_isAlive = false;
		_animationPlayer.Play(DeathAnimationName);
		GetTree().CreateTimer(DisappearDelayOnDeath).Timeout += () => Disappear();
		
		_weapon.QueueFree();
		_weapon = null;
	}

	private void Disappear()
	{
		GetTree().CreateTween()
			.TweenProperty(this, DisappearTweenProperty, 0, DisappearDuration)
			.Finished += () => QueueFree();
	}

	protected void SetWeaponAttackSide(Vector2 direction)
	{

		Vector2 forward = new Vector2(1, 0);

		_weaponPivot.Rotation = forward.AngleTo(direction);

		if (direction.Y >= direction.X)
			_weaponPivot.Scale = new Vector2(1, -1);
		else
			_weaponPivot.Scale = new Vector2(1, 1);
	}

	private void PlayHitSound()
	{
		_audioPlayer.PlayRandomSound(_hitSounds);
	}

	private void PlayDeathSound()
	{
		_audioPlayer.PlayRandomSound(_deathSounds);
	}
}
