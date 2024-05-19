using Godot;
using System;
using System.Collections.Generic;
using DungeonAdventure.Characters;
using DungeonAdventure.Utils;

namespace DungeonAdventure.Weapons;

public partial class Sword : Weapon
{
	[Export] private AnimationPlayer _animationPlayer;
	[Export] private Area2D _collisionArea;

	[Export] private AudioStreamPlayer2D _audioPlayer;
	[Export] private AudioStream[] _attackSounds;
	
	// keep track of the bodies the sword hit and times when it happened
	private readonly Dictionary<Node2D, ulong> _hitTimes = new();
	
	private const string SwingAnimationName = "swing";
	private const float MsToSecondFactor = 1.0f / 1000;
	
	public override void _Ready()
	{
		_collisionArea.AreaEntered += OnSwordCollision;
	}

	public override void _Process(double delta)
	{
		CleanupHitTimes();
	}

	public override void Attach(Character character)
	{ 
		ClearIgnoredBodies();
		AddIgnoredBodies(character.HitArea);
	}
	
	public override void Attack()
	{
		SetLastAttackTime();
		
		_animationPlayer.Play(SwingAnimationName);

		foreach (Node2D node in _collisionArea.GetOverlappingAreas())
		{
			ProcessHit(node);
		}
	}

	private void OnSwordCollision(Node2D node)
	{
		if (_animationPlayer.IsPlaying())
			ProcessHit(node);
	}
	
	private void ProcessHit(Node2D body)
	{
		if (!IsBodyIgnored(body) && CheckIfShouldHitAndUpdateTimes(body))
		{
			Character character = body.FindCharacter();
			if (character != null)
			{
				GD.Print("HIT: " + character.Name);
				character.ApplyDamage(Damage);
			}
		}
	}

	// check if we are not hitting the same thing multiple times too soon
	// to prevent extra hits that should not happen
	private bool CheckIfShouldHitAndUpdateTimes(Node2D body)
	{
		ulong currentTime = Time.GetTicksMsec();
		ulong lastHitTime = _hitTimes.GetValueOrDefault(body, 0ul);

		if ((currentTime - lastHitTime) * MsToSecondFactor < AttackRate)
			return false;

		_hitTimes[body] = currentTime;
		return true;
	}

	private void CleanupHitTimes()
	{
		ulong currentTime = Time.GetTicksMsec();
		List<Node2D> toRemove = new();
		foreach (var kv in _hitTimes)
		{
			if ((currentTime - kv.Value) * MsToSecondFactor >= AttackRate)
				toRemove.Add(kv.Key);
		}

		foreach (Node2D body in toRemove)
		{
			_hitTimes.Remove(body);
		}
	}

	private void PlayAttackSound()
	{
		_audioPlayer.PlayRandomSound(_attackSounds);
	}
}
