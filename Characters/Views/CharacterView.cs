using DungeonAdventure.Characters.Controllers;
using DungeonAdventure.Characters.Effects;
using DungeonAdventure.Characters.Indicators;
using DungeonAdventure.Characters.Models;
using DungeonAdventure.Items;
using DungeonAdventure.Utils;
using DungeonAdventure.Weapons;
using DungeonAdventure.Weapons.View;
using Godot;

namespace DungeonAdventure.Characters.Views;

public partial class CharacterView : CharacterBody2D
{
	[Export] public CharacterControllerFactory ControllerFactory { get; set; }
	[Export] public CharacterModelFactory ModelFactory { get; set; }
	
	[Export] private NavigationAgent2D _navigationAgent;
	[Export] private Area2D _hitArea;
	
	[Export] private AudioStreamPlayer2D _audioPlayer;
	[Export] private AudioStream[] _hitSounds;
	[Export] private AudioStream[] _deathSounds;

	private WeaponView _weapon;
	public CharacterModel Model { get; private set; }
	private CharacterController _controller;
	private CharacterVisual _visual;

	private IndicatorManager _indicatorManager;
	private Node _effectsContainer;

	private const string IdleAnimationName = "idle";
	private const string RunAnimationName = "run";
	private const string DeathAnimationName = "death";
	private const float DisappearDelayOnDeath = 2.0f;
	private const float DisappearDuration = 1.0f;
	private const string DisappearTweenProperty = "modulate:a";

	private const int PlayerCollisionLayer = 2;
	private const int EnemyCollisionLayer = 3;
	
	public WeaponView Weapon => _weapon;
	public NavigationAgent2D NavigationAgent => _navigationAgent;
	public Area2D HitArea => _hitArea;
	public CollisionObject2D Collision => this;
	public bool IsAlive => Model.IsAlive;
	
	public override void _Ready()
	{
		Model = ModelFactory.CreateModel();
		_controller = ControllerFactory.Create(this, Model);
		
		_visual = this.FindNodeDown<CharacterVisual>();
		_visual?.QueueFree();
		_visual = SetupVisual(Model.VisualName);
		
		_weapon = this.FindNodeDown<WeaponView>();
		_weapon?.QueueFree();
		_weapon = SetupWeapon(Model.WeaponName);
		
		_indicatorManager = GetNodeOrNull<IndicatorManager>("%IndicatorManager");
		
		_effectsContainer = new Node();
		_effectsContainer.Name = "EffectsContainer";
		AddChild(_effectsContainer);
		
		_weapon.Attach(this);

		Model.CharacterDied += OnDeath;

		_controller.IndicatorManager = _indicatorManager;
		
		SetupCollision();
	}

	private void SetupCollision()
	{
		SetCollisionLayerValue(PlayerCollisionLayer, _controller.IsPlayer);
		SetCollisionLayerValue(EnemyCollisionLayer, !_controller.IsPlayer);
	}

	private CharacterVisual SetupVisual(string name)
	{
		string scenePath = $"res://Characters/Visual/{name.ToLower()}.tscn";
		PackedScene scene = GD.Load<PackedScene>(scenePath);

		if (scene == null)
		{
			GD.PrintErr($"No character visual with name {name}");
			return null;
		}

		CharacterVisual visual = scene.InstantiateOrNull<CharacterVisual>();

		if (visual == null)
		{
			GD.PrintErr($"The visual scene {name} must be of type CharacterVisual");
			return null;
		}

		AddChild(visual);
		
		return visual;
	}

	private WeaponView SetupWeapon(string name)
	{
		string scenePath = $"res://Weapons/{name.ToLower()}.tscn";
		PackedScene scene = GD.Load<PackedScene>(scenePath);

		if (scene == null)
		{
			GD.PrintErr($"No weapon with name {name}");
			return null;
		}

		WeaponView weapon = scene.InstantiateOrNull<WeaponView>();

		if (weapon == null)
		{
			GD.PrintErr($"The weapon scene {name} must be of type Weapon");
			return null;
		}

		AddChild(weapon);
		
		return weapon;
	}
	
	public override void _Process(double delta)
	{
		_controller.Process(delta);
	}

	public override void _PhysicsProcess(double delta)
	{
		_controller.PhysicsProcess(delta);
	}
	
	public void UpdateAnimation(Vector2 velocity)
	{
		if (!velocity.IsZeroApprox())
		{
			_visual.AnimationPlayer.Play(RunAnimationName);
			_visual.Sprite.FlipH = velocity.X < 0;
		}
		else
		{
			_visual.AnimationPlayer.Play(IdleAnimationName);
		}
	}
	
	public void ApplyDamage(float damage)
	{
		_controller.ApplyDamage(damage);
	}

	public void Heal(float value, float duration)
	{
		_controller.Heal(value, duration);
	}

	private void OnDeath()
	{
		PlayDeathSound();
		
		_visual.AnimationPlayer.Play(DeathAnimationName);
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

	public void PlayHitSound()
	{
		_audioPlayer.PlayRandomSound(_hitSounds);
	}

	public void PlayDeathSound()
	{
		_audioPlayer.PlayRandomSound(_deathSounds);
	}

	public bool CanPickupItem(Item item)
	{
		return Model.CanPickupItem(item);
	}

	public void PickupItem(Item item)
	{
		_controller.PickupItem(item);
	}

	public bool CanUseItem(Item item)
	{
		return item.CanUse(this);
	}

	public void UseItem(Item item)
	{
		_controller.UseItem(item);
	}

	public CharacterStats CollectEffects(float delta)
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
