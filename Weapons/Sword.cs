using Godot;
using System;
using System.Collections.Generic;
using DungeonAdventure.Characters;

namespace DungeonAdventure.Weapons;

public partial class Sword : Weapon
{
	[Export] private AnimationPlayer _animationPlayer;
	[Export] private Area2D _collisionArea;
	
	// keep track of the bodies the sword hit and times when did it happen
	private Dictionary<Node2D, ulong> _hitTimes = new();
	
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
		
		_animationPlayer.Play("swing");

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

	private Character LocateCharacter(Node node)
	{
		if (node == null)
			return null;
		
		if (node is Character)
			return (Character)node;
		
		return LocateCharacter(node.GetParent());
	}
	
	private void ProcessHit(Node2D body)
	{
		if (!IsBodyIgnored(body) && CheckIfShouldHitAndUpdateTimes(body))
		{
			Character character = LocateCharacter(body);
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

		if ((currentTime - lastHitTime) / 1000f < AttackRate)
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
			if ((currentTime - kv.Value) / 1000f >= AttackRate)
				toRemove.Add(kv.Key);
		}

		foreach (Node2D body in toRemove)
		{
			_hitTimes.Remove(body);
		}
	}
}
