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

/// <summary>
/// Represents the visual and functional representation of a character in the game.
/// </summary>
public partial class CharacterView : CharacterBody2D, IPausable
{
	/// <summary>
	/// Factory to create character controller.
	/// </summary>
	[Export] public CharacterControllerFactory ControllerFactory { get; set; }
	
	/// <summary>
	/// Factory to create character model.
	/// </summary>
	[Export] public CharacterModelFactory ModelFactory { get; set; }
	
	[Export] private NavigationAgent2D _navigationAgent;
	[Export] private Area2D _hitArea;
	
	[Export] private AudioStreamPlayer2D _audioPlayer;
	[Export] private AudioStream[] _hitSounds;
	[Export] private AudioStream[] _deathSounds;

	private bool _isPaused = false;
	
	private WeaponView _weapon;
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
	
	/// <summary>
	/// The character model.
	/// </summary>
	public CharacterModel Model { get; private set; }

	/// <summary>
	/// Gets the weapon view associated with the character.
	/// </summary>
	public WeaponView Weapon => _weapon;
	
	/// <summary>
	/// Gets the navigation agent associated with the character.
	/// </summary>
	public NavigationAgent2D NavigationAgent => _navigationAgent;
	
	/// <summary>
	/// Gets the hit area associated with the character.
	/// </summary>
	public Area2D HitArea => _hitArea;
	
	/// <summary>
	/// Gets the collision object for the character.
	/// </summary>
	public CollisionObject2D Collision => this;
	
	/// <summary>
	/// Gets a value indicating whether the character is alive.
	/// </summary>
	public bool IsAlive => Model.IsAlive;
	
	/// <summary>
	/// Called when the node is added to the scene.
	/// </summary>
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

	/// <summary>
	/// Sets up collision layers for the character.
	/// </summary>
	private void SetupCollision()
	{
		SetCollisionLayerValue(PlayerCollisionLayer, _controller.IsPlayer);
		SetCollisionLayerValue(EnemyCollisionLayer, !_controller.IsPlayer);
	}

	/// <summary>
	/// Sets up the visual representation of the character.
	/// </summary>
	/// <param name="name">The name of the visual scene.</param>
	/// <returns>The instantiated character visual.</returns>
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

	/// <summary>
	/// Sets up the weapon for the character.
	/// </summary>
	/// <param name="name">The name of the weapon scene.</param>
	/// <returns>The instantiated weapon view.</returns>
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
	
	/// <summary>
	/// Processes the character logic each frame.
	/// </summary>
	/// <param name="delta">The elapsed time since the last frame.</param>
	public override void _Process(double delta)
	{
		if (_isPaused)
			return;
		
		_controller.Process(delta);
	}

	/// <summary>
	/// Processes the physics-related logic each frame.
	/// </summary>
	/// <param name="delta">The elapsed time since the last frame.</param>
	public override void _PhysicsProcess(double delta)
	{
		if (_isPaused)
			return;
		
		_controller.PhysicsProcess(delta);
	}
	
	/// <summary>
	/// Updates the character's animation based on velocity.
	/// </summary>
	/// <param name="velocity">The current velocity of the character.</param>
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
	
	/// <summary>
	/// Applies damage to the character.
	/// </summary>
	/// <param name="damage">The amount of damage to apply.</param>
	public void ApplyDamage(float damage)
	{
		_controller.ApplyDamage(damage);
	}

	/// <summary>
	/// Heals the character.
	/// </summary>
	/// <param name="value">The amount to heal.</param>
	/// <param name="duration">The duration of the heal effect.</param>
	public void Heal(float value, float duration)
	{
		_controller.Heal(value, duration);
	}

	/// <summary>
	/// Called when the character dies.
	/// </summary>
	private void OnDeath()
	{
		PlayDeathSound();
		
		_visual.AnimationPlayer.Play(DeathAnimationName);
		GetTree().CreateTimer(DisappearDelayOnDeath).Timeout += () => Disappear();
		
		_weapon.QueueFree();
		_weapon = null;
	}

	/// <summary>
	/// Makes the character disappear after death.
	/// </summary>
	private void Disappear()
	{
		GetTree().CreateTween()
			.TweenProperty(this, DisappearTweenProperty, 0, DisappearDuration)
			.Finished += () => QueueFree();
	}

	/// <summary>
	/// Plays a hit sound.
	/// </summary>
	public void PlayHitSound()
	{
		_audioPlayer.PlayRandomSound(_hitSounds);
	}

	/// <summary>
	/// Plays a death sound.
	/// </summary>
	public void PlayDeathSound()
	{
		_audioPlayer.PlayRandomSound(_deathSounds);
	}

	/// <summary>
	/// Determines whether the character can pick up the specified item.
	/// </summary>
	/// <param name="item">The item to pick up.</param>
	/// <returns>True if the character can pick up the item, otherwise false.</returns>
	public bool CanPickupItem(Item item)
	{
		return Model.CanPickupItem(item);
	}

	/// <summary>
	/// Picks up an item and adds it to the character's inventory.
	/// </summary>
	/// <param name="item">The item to pick up.</param>
	public void PickupItem(Item item)
	{
		_controller.PickupItem(item);
	}

	/// <summary>
	/// Determines whether the character can use the specified item.
	/// </summary>
	/// <param name="item">The item to use.</param>
	/// <returns>True if the character can use the item, otherwise false.</returns>
	public bool CanUseItem(Item item)
	{
		return item.CanUse(this);
	}

	/// <summary>
	/// Uses an item from the character's inventory.
	/// </summary>
	/// <param name="item">The item to use.</param>
	public void UseItem(Item item)
	{
		_controller.UseItem(item);
	}

	/// <summary>
	/// Collects and applies the effects currently affecting the character.
	/// </summary>
	/// <param name="delta">The elapsed time since the last frame.</param>
	/// <returns>The modified character stats.</returns>
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

	/// <summary>
	/// Adds an effect to the character.
	/// </summary>
	/// <param name="effect">The effect to add.</param>
	public void AddEffect(Effect effect)
	{
		_effectsContainer.AddChild(effect);
	}

	/// <summary>
	/// Pauses the character's actions.
	/// </summary>
	public void Pause()
	{
		_isPaused = true;
	}

	/// <summary>
	/// Resumes the character's actions.
	/// </summary>
	public void Resume()
	{
		_isPaused = false;
	}
}
