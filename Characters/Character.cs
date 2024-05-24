using System;
using DungeonAdventure.Characters;
using DungeonAdventure.Characters.Effects;
using DungeonAdventure.Characters.Indicators;
using DungeonAdventure.Items;
using DungeonAdventure.Utils;
using DungeonAdventure.Weapons;
using Godot;
using Godot.Collections;

namespace DungeonAdventure.Characters;

public partial class Character : CharacterBody2D
{
	[Export] private CharacterControllerFactory _controllerFactory;
	
	[Export] private float _speed = 80.0f;
	[Export] private float _health = 100.0f;
	[Export] private float _damageMin = 10f;
	[Export] private float _damageMax = 30f;
	[Export] private float _hitChance = 0.8f;
	[Export] private float _blockChance = 0.3f;

	[Export] public Array<Item> Items { get; private set; } = new();
	
	[Export] private Sprite2D _sprite;
	[Export] private AnimationPlayer _animationPlayer;
	[Export] private NavigationAgent2D _navigationAgent;
	[Export] private Area2D _hitArea;
	
	[Export] private Weapon _weapon;
	
	[Export] private Node2D _weaponPivot;
	
	[Export] private AudioStreamPlayer2D _audioPlayer;
	[Export] private AudioStream[] _hitSounds;
	[Export] private AudioStream[] _deathSounds;

	[Signal]
	public delegate void ItemsChangedEventHandler();

	private Random _random = new();
	private ICharacterController _controller;
	private bool _isAlive = true;

	private IndicatorManager _indicatorManager;
	private Node _effectsContainer;
	private CharacterStats _stats = new();

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
		_indicatorManager = GetNodeOrNull<IndicatorManager>("%IndicatorManager");
		
		_effectsContainer = new Node();
		_effectsContainer.Name = "EffectsContainer";
		AddChild(_effectsContainer);
		
		_weapon.Attach(this);
		_controller = _controllerFactory.Create(this);
	}
	
	public override void _Process(double delta)
	{
		if (!_isAlive)
			return;

		_stats = ApplyEffects((float)delta);
		
		Vector2 direction = _controller.GetMoveDirection();

		if (_stats.HealRate != 0)
			_health += _stats.HealRate;
		
		Velocity = direction * _speed * _stats.SpeedModifier;
		
		UpdateAnimation(Velocity);
		
		MoveAndSlide();

		ProcessAttack(_stats.DamageModifier);
	}

	public override void _PhysicsProcess(double delta)
	{
		_controller.PhysicsProcess(delta);
	}

	private void ProcessAttack(float damageModifier)
	{
		Vector2? attackDirection = _controller.GetAttackDirection();
		

		if (attackDirection.HasValue)
		{
			SetWeaponAttackSide(attackDirection.Value);

			if (_weapon.CanAttack())
			{
				float damage = -1;
				if (!RandomizeMiss())
					damage = RandomizeDamage() * damageModifier;
				_weapon.Attack(damage);
			}
		}
	}

	private float RandomizeDamage()
	{
		return _damageMin + (float)_random.NextDouble() * (_damageMax - _damageMin);
	}

	private bool RandomizeMiss()
	{
		return _random.NextDouble() > _hitChance;
	}

	private bool RandomizeBlock()
	{
		return _random.NextDouble() <= _blockChance;
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

		if (damage <= 0)
		{
			_indicatorManager?.AddIndicator($"MISS", new Color(0.8f, 0.8f, 0.8f));
			return;
		}

		if (RandomizeBlock())
		{
			_indicatorManager?.AddIndicator($"BLOCK", new Color(0.8f, 0.8f, 0.8f));
			return;
		}
		
		_indicatorManager?.AddIndicator($"-{(int)damage}", new Color(0.8f, 0, 0));
		
		PlayHitSound();
		
		_health -= damage;
		GD.Print("health: " + _health);
		if (_health <= 0)
		{
			Die();
		}
	}

	public void Heal(float value, float duration)
	{
		if (!_isAlive)
			return;
		
		_indicatorManager?.AddIndicator($"+{(int)value}", new Color(0, 0.8f, 0));
		
		if (duration <= 0)
			_health += value;
		else
			AddEffect(new HealEffect(value, duration));
		
		GD.Print("health: " + _health);
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

	private void SetWeaponAttackSide(Vector2 direction)
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

	public bool CanPickupItem(Item item)
	{
		return true;
	}

	public void PickupItem(Item item)
	{
		Items.Add(item);
		EmitSignal(SignalName.ItemsChanged);
	}

	public bool CanUseItem(Item item)
	{
		return item.CanUse(this);
	}

	public void UseItem(Item item)
	{
		item.Use(this);
		Items.Remove(item);
		EmitSignal(SignalName.ItemsChanged);
	}

	private CharacterStats ApplyEffects(float delta)
	{
		CharacterStats stats = new();

		for (int i = 0; i < _effectsContainer.GetChildCount(); i++)
		{
			Effect effect = _effectsContainer.GetChildOrNull<Effect>(i);
			if (effect == null)
				continue;
			
			effect.Apply(delta, this, stats);
		}
		
		return stats;
	}

	public void AddEffect(Effect effect)
	{
		_effectsContainer.AddChild(effect);
	}
}
