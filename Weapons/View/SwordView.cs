using System.Collections.Generic;
using DungeonAdventure.Characters.Views;
using DungeonAdventure.Utils;
using DungeonAdventure.Weapons.Controllers;
using DungeonAdventure.Weapons.Models;
using Godot;

namespace DungeonAdventure.Weapons.View;

public partial class SwordView : WeaponView
{
	[Export] private AnimationPlayer _animationPlayer;
	[Export] private Area2D _collisionArea;

	[Export] private AudioStreamPlayer2D _audioPlayer;
	[Export] private AudioStream[] _attackSounds;

	private SwordController _controller;
	
	private const string SwingAnimationName = "swing";

	public SwordView()
	{
		Model = new WeaponModel(0.5f, 50);
		_controller = new SwordController(this, Model);
	}
	
	protected override float GetAttackAnimationLength() => _animationPlayer.GetAnimation(SwingAnimationName).Length;
	public override bool IsAttacking() => _animationPlayer.IsPlaying();
	
	public override void _Ready()
	{
		_collisionArea.AreaEntered += OnSwordCollision;
	}

	public override void _Process(double delta)
	{
		_controller.CleanupHitTimes();
	}

	public override void Attach(CharacterView character)
	{ 
		ClearIgnoredBodies();
		AddIgnoredBodies(character.HitArea);
	}
	
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

	private void OnSwordCollision(Node2D node)
	{
		_controller.OnSwordCollision(node);
	}

	private void PlayAttackSound()
	{
		_audioPlayer.PlayRandomSound(_attackSounds);
	}
}
