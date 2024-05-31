using System.Collections.Generic;
using Ardot.SaveSystems;
using DungeonAdventure.Characters.Controllers;
using DungeonAdventure.Characters.Effects;
using DungeonAdventure.Characters.Indicators;
using DungeonAdventure.Characters.Models;
using DungeonAdventure.Items;
using DungeonAdventure.Utils;
using DungeonAdventure.Weapons;
using DungeonAdventure.Weapons.View;
using Godot;
using Godot.Collections;
using Array = System.Array;
using Item = DungeonAdventure.Items.Item;

namespace DungeonAdventure.Characters.Views;

/// <summary>
/// Represents the visual and functional representation of a character in the game.
/// </summary>
public partial class CharacterView : CharacterBody2D, IPausable
{
	/// <summary>
	/// Factory to create character controller.
	/// </summary>
	[Export]
	public CharacterControllerFactory ControllerFactory { get; set; }

	/// <summary>
	/// Factory to create character model.
	/// </summary>
	[Export]
	public CharacterModelFactory ModelFactory { get; set; }

	[Export] private NavigationAgent2D _navigationAgent;
	[Export] private Area2D _hitArea;

	[Export] private AudioStreamPlayer2D _audioPlayer;
	[Export] private AudioStream[] _hitSounds;
	[Export] private AudioStream[] _deathSounds;

	private bool _isPaused = false;

	private WeaponView _weapon;
	private CharacterVisual _visual;

	private CharacterController _controller;
	private IndicatorManager _indicatorManager;
	private Node2D _effectsContainer;

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
	/// The character controller.
	/// </summary>
	public CharacterController Controller { get => _controller; set => SetController(value); }
	
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
	/// Gets a value indicating whether the character is a player.
	/// </summary>
	public bool IsPlayer => Controller.IsPlayer;

	/// <summary>
	/// Called when the node is added to the scene.
	/// </summary>
	public override void _Ready()
	{
		if (Model == null)
			Model = ModelFactory.CreateModel();
		
		if (Controller == null)
			Controller = ControllerFactory.Create(this, Model);

		UpdateVisual(Model.VisualName);

		UpdateWeapon(Model.WeaponName);

		_indicatorManager = GetNodeOrNull<IndicatorManager>("%IndicatorManager");

		_effectsContainer = new Node2D();
		_effectsContainer.Name = "EffectsContainer";
		AddChild(_effectsContainer);

		Model.CharacterDied += OnDeath;

		Controller.IndicatorManager = _indicatorManager;

		SetupCollision();
	}

	/// <summary>
	/// Sets up collision layers for the character.
	/// </summary>
	private void SetupCollision()
	{
		SetCollisionLayerValue(PlayerCollisionLayer, Controller.IsPlayer);
		SetCollisionLayerValue(EnemyCollisionLayer, !Controller.IsPlayer);
	}


	/// <summary>
	/// Updates the visual representation of the character.
	/// </summary>
	public void UpdateVisual(string name)
	{
		CharacterVisual currentVisual = this.FindNodeDown<CharacterVisual>();
		currentVisual?.QueueFree();

		string scenePath = $"res://Characters/Visual/{name.ToLower()}.tscn";
		PackedScene scene = GD.Load<PackedScene>(scenePath);

		if (scene == null)
		{
			GD.PrintErr($"No character visual with name {name}");
			return;
		}

		_visual = scene.InstantiateOrNull<CharacterVisual>();

		if (_visual == null)
		{
			GD.PrintErr($"The visual scene {name} must be of type CharacterVisual");
			return;
		}

		AddChild(_visual);
	}


	/// <summary>
	/// Updates the character's weapon.
	/// </summary>
	public void UpdateWeapon(string name)
	{
		WeaponView currentWeapon = this.FindNodeDown<WeaponView>();
		currentWeapon?.QueueFree();

		string scenePath = $"res://Weapons/{name.ToLower()}.tscn";
		PackedScene scene = GD.Load<PackedScene>(scenePath);

		if (scene == null)
		{
			GD.PrintErr($"No weapon with name {name}");
			return;
		}

		_weapon = scene.InstantiateOrNull<WeaponView>();

		if (_weapon == null)
		{
			GD.PrintErr($"The weapon scene {name} must be of type Weapon");
			return;
		}

		AddChild(_weapon);

		_weapon.Attach(this);
	}

	/// <summary>
	/// Processes the character logic each frame.
	/// </summary>
	/// <param name="delta">The elapsed time since the last frame.</param>
	public override void _Process(double delta)
	{
		if (_isPaused)
			return;

		Controller.Process(delta);
	}

	/// <summary>
	/// Processes the physics-related logic each frame.
	/// </summary>
	/// <param name="delta">The elapsed time since the last frame.</param>
	public override void _PhysicsProcess(double delta)
	{
		if (_isPaused)
			return;

		Controller.PhysicsProcess(delta);
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
		Controller.ApplyDamage(damage);
	}

	/// <summary>
	/// Heals the character.
	/// </summary>
	/// <param name="value">The amount to heal.</param>
	/// <param name="duration">The duration of the heal effect.</param>
	public void Heal(float value, float duration)
	{
		Controller.Heal(value, duration);
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
		return Controller.CanPickupItem(item);
	}

	/// <summary>
	/// Picks up an item and adds it to the character's inventory.
	/// </summary>
	/// <param name="item">The item to pick up.</param>
	public void PickupItem(Item item)
	{
		Controller.PickupItem(item);
	}

	/// <summary>
	/// Determines whether the character can use the specified item.
	/// </summary>
	/// <param name="item">The item to use.</param>
	/// <returns>True if the character can use the item, otherwise false.</returns>
	public bool CanUseItem(Item item)
	{
		return Controller.CanUseItem(item);
	}

	/// <summary>
	/// Uses an item from the character's inventory.
	/// </summary>
	/// <param name="item">The item to use.</param>
	public void UseItem(Item item)
	{
		Controller.UseItem(item);
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
		effect.SetCharacter(this);
		_effectsContainer.AddChild(effect);
	}

	/// <summary>
	/// Removes an effect from the character.
	/// </summary>
	/// <param name="effect">The effect to remove.</param>
	public void RemoveEffect(Effect effect)
	{
		_effectsContainer.RemoveChild(effect);
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

	/// <summary>
	/// Sets the character controller.
	/// </summary>
	private void SetController(CharacterController controller)
	{
		_controller = controller;
		_controller.IndicatorManager = _indicatorManager;
	}

	/// <summary>
	/// Class for saving and loading character data.
	/// </summary>
	public class SaveLoad : ISaveable
	{
		/// <summary>
		/// Gets or sets the character view to be saved or loaded.
		/// </summary>
		public CharacterView Character { get; set; }
		
		/// <summary>
		/// Saves the character data into a SaveData object.
		/// </summary>
		/// <param name="parameters">Additional parameters for saving (not used).</param>
		/// <returns>A SaveData object containing the character's data.</returns>
		public SaveData Save(params Variant[] parameters)
		{
			Array<string> items = new();
			foreach (Item item in Character.Model.Items)
			{
				if (!string.IsNullOrEmpty(item.ItemId))
					items.Add(item.ItemId);
			}

			Godot.Collections.Dictionary<string, Variant> data = new();

			data["key"] = GetLoadKey();
			data["scene_path"] = Character.SceneFilePath;
			data["position"] = Character.Position;
			data["model_name"] = Character.Model.ModelName;
			data["is_enemy"] = Character.Controller is EnemyController;
			data["health"] = Character.Model.Health;
			data["max_health"] = Character.Model.MaxHealth;
			data["speed"] = Character.Model.Speed;
			data["damage_max"] = Character.Model.DamageMax;
			data["damage_min"] = Character.Model.DamageMin;
			data["attack_rate"] = Character.Model.AttackRate;
			data["hit_chance"] = Character.Model.HitChance;
			data["block_chance"] = Character.Model.BlockChance;
			data["visual_name"] = Character.Model.VisualName;
			data["weapon_name"] = Character.Model.WeaponName;
			data["items"] = items;
			
			return new SaveData(GetLoadKey(), data);
		}

		/// <summary>
		/// Loads the character data from a SaveData object.
		/// </summary>
		/// <param name="data">The SaveData object containing the character's data.</param>
		/// <param name="parameters">Additional parameters for loading (not used).</param>
		public void Load(SaveData data, params Variant[] parameters)
		{
			Godot.Collections.Dictionary<string, Variant> p = data[0].AsGodotDictionary<string, Variant>();

			string key = p["key"].AsString();
			string scenePath = p["scene_path"].AsString();
			Vector2 position = p["position"].AsVector2();
			string modelName = p["model_name"].AsString();
			bool isEnemy = p["is_enemy"].AsBool();
			float health = p["health"].AsSingle();
			float maxHealth = p["max_health"].AsSingle();
			float speed = p["speed"].AsSingle();
			float damageMax = p["damage_max"].AsSingle();
			float damageMin = p["damage_min"].AsSingle();
			float attackRate = p["attack_rate"].AsSingle();
			float hitChance = p["hit_chance"].AsSingle();
			float blockChance = p["block_chance"].AsSingle();
			string visualName = p["visual_name"].AsString();
			string weaponName = p["weapon_name"].AsString();
			Array<string> itemIds = p["items"].AsGodotArray<string>();

			CharacterModel model;
			List<Item> items = new();
			
			foreach (string itemId in itemIds)
			{
				string itemResourcePath = $"res://Items/Resource/{itemId}.tres";
				Item item = GD.Load<Item>(itemResourcePath);
				items.Add(item);
			}
			
			if (!string.IsNullOrEmpty(modelName))
			{
				model = CharacterFactory.CreateByName(modelName, maxHealth, speed, damageMin, damageMax, attackRate, 
					hitChance, blockChance,
					items, visualName, weaponName);
			}
			else
			{
				model = new CharacterModel("", maxHealth, speed, damageMin, damageMax,  attackRate, 
					hitChance, blockChance,
					items, visualName, weaponName);
			}

			model.ApplyDamage(maxHealth - health);
			
			PackedScene scene = GD.Load<PackedScene>(scenePath);
			Character = scene.Instantiate<CharacterView>();

			CharacterController controller =
				isEnemy ? new EnemyController(Character, model) : new PlayerController(Character, model);

			Character.Model = model;
			Character.Controller = controller;

			Character.Position = position;
			Character.Name = key;
		}

		/// <summary>
		/// Gets the load key for the character.
		/// </summary>
		/// <param name="parameters">Additional parameters for the load key (not used).</param>
		/// <returns>The load key for the character.</returns>
		public StringName GetLoadKey(params Variant[] parameters) => $"Character_{Character.NativeInstance}";
	}
}
