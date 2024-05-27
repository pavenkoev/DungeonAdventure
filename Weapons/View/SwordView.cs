using System.Collections.Generic;
using DungeonAdventure.Characters.Views;
using DungeonAdventure.Utils;
using DungeonAdventure.Weapons.Controllers;
using DungeonAdventure.Weapons.Models;
using Godot;

namespace DungeonAdventure.Weapons.View;

/// <summary>
/// Represents the view for a sword weapon.
/// </summary>
public partial class SwordView : WeaponView
{
	[Export] private AnimationPlayer _animationPlayer;
	[Export] private Area2D _collisionArea;

	[Export] private AudioStreamPlayer2D _audioPlayer;
	[Export] private AudioStream[] _attackSounds;

	private SwordController _controller;
	
	private const string SwingAnimationName = "swing";

	/// <summary>
	/// Initializes a new instance of the <see cref="SwordView"/> class.
	/// </summary>
	public SwordView()
	{
		Model = new WeaponModel(0.5f, 50);
		_controller = new SwordController(this, Model);
	}
	
	/// <summary>
	/// Gets the length of the attack animation.
	/// </summary>
	/// <returns>The length of the attack animation.</returns>
	protected override float GetAttackAnimationLength() => _animationPlayer.GetAnimation(SwingAnimationName).Length;
	
	/// <summary>
	/// Determines whether the weapon is currently attacking.
	/// </summary>
	/// <returns>True if the weapon is attacking, otherwise false.</returns>
	public override bool IsAttacking() => _animationPlayer.IsPlaying();
	
	/// <summary>
	/// Called when the node is added to the scene. Initializes the sword.
	/// </summary>
	public override void _Ready()
	{
		_collisionArea.AreaEntered += OnSwordCollision;
	}

	/// <summary>
	/// Processes the sword logic each frame.
	/// </summary>
	/// <param name="delta">The elapsed time since the last frame.</param>
	public override void _Process(double delta)
	{
		_controller.CleanupHitTimes();
	}

	/// <summary>
	/// Attaches the sword to a character.
	/// </summary>
	/// <param name="character">The character to attach the sword to.</param>
	public override void Attach(CharacterView character)
	{ 
		ClearIgnoredBodies();
		AddIgnoredBody(character.HitArea);
	}
	
	/// <summary>
	/// Executes the attack action with the specified damage.
	/// </summary>
	/// <param name="damage">The amount of damage to inflict.</param>
	public override void Attack(float damage)
	{
		_controller.Attack(damage);
		
		_animationPlayer.Stop();
		_animationPlayer.Play(SwingAnimationName, customSpeed: GetAnimationSpeedMultiplier());

		foreach (Node2D node in _collisionArea.GetOverlappingAreas())
		{
			OnSwordCollision(node);
		}
	}

	/// <summary>
	/// Handles the collision of the sword with another body.
	/// </summary>
	/// <param name="node">The body that the sword collides with.</param>
	private void OnSwordCollision(Node2D node)
	{
		_controller.OnSwordCollision(node);
	}

	/// <summary>
	/// Plays an attack sound.
	/// </summary>
	private void PlayAttackSound()
	{
		_audioPlayer.PlayRandomSound(_attackSounds);
	}
}
