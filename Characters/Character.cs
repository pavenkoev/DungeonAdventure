using System;
using DungeonAdventure.Characters;
using DungeonAdventure.Utils;
using DungeonAdventure.Weapons;
using Godot;
using System.Data.SQLite;

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
	
	[Export] protected Weapon _weapon;
	
	[Export] private Node2D _weaponPivot;
	
	[Export] private AudioStreamPlayer2D _audioPlayer;
	[Export] private AudioStream[] _hitSounds;
	[Export] private AudioStream[] _deathSounds;
	[Export] public int CharacterId { get; set; }
	
	private ICharacterController _controller;
	private bool _isAlive = true;

	public Weapon Weapon => _weapon;
	public NavigationAgent2D NavigationAgent => _navigationAgent;
	public Area2D HitArea => _hitArea;
	public CollisionObject2D Collision => this;
	public bool IsAlive => _isAlive;
	
	public override void _Ready()
	{
		_weapon.Attach(this);
		_controller = _controllerFactory.Create(this);
		
		TestHeroClass();
	}
	
	public void TestHeroClass()
	{
		// Test the Hero class by fetching a Hero by ID and printing its properties
		Hero hero = Hero.GetHeroById(1);
		if (hero != null)
		{
			GD.Print($"Hero: {hero.Name}, Strength: {hero.Strength}, Speed: {hero.Speed}, Health: {hero.Health}");
		}
		else
		{
			GD.Print("Hero with ID 1 not found.");
		}
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
