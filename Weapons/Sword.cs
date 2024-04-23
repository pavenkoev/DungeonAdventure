using Godot;
using System;
using System.Collections.Generic;

namespace DungeonAdventure;

public partial class Sword : Node2D
{
	[Export] private AnimationPlayer _animationPlayer;
	[Export] private Area2D _collisionArea;
	[Export] private float _damage = 20.0f;
	[Export] private float _attackRate = 0.25f;
	[Export] private float _attackRange = 20.0f;

	private ulong _lastAttackTimeMs = 0;
	private HashSet<Node2D> _ignoredBodies = new();
	
	// keep track of the bodies the sword hit and times when did it happen
	private Dictionary<Node2D, ulong> _hitTimes = new();

	public float AttackRange => _attackRange;
	
	public override void _Ready()
	{
		_collisionArea.BodyEntered += OnSwordCollision;
	}

	public override void _Process(double delta)
	{
		CleanupHitTimes();
	}

	public void Attach(Node2D character)
	{
		_ignoredBodies.Clear();
		_ignoredBodies.Add(character);
	}

	public void Attack()
	{
		_lastAttackTimeMs = Time.GetTicksMsec();
		
		_animationPlayer.Play("swing");

		foreach (Node2D node in _collisionArea.GetOverlappingBodies())
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
		if (!_ignoredBodies.Contains(body) && CheckIfShouldHitAndUpdateTimes(body))
		{
			GD.Print("HIT: " + body.Name);

			Character character = body as Character;
			if (character != null)
			{
				character.ApplyDamage(_damage);
			}
		}
	}

	// check if we are not hitting the same thing multiple times too soon
	// to prevent extra hits that should not happen
	private bool CheckIfShouldHitAndUpdateTimes(Node2D body)
	{
		ulong currentTime = Time.GetTicksMsec();
		ulong lastHitTime = _hitTimes.GetValueOrDefault(body, 0ul);

		if ((currentTime - lastHitTime) / 1000f < _attackRate)
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
			if ((currentTime - kv.Value) / 1000f >= _attackRate)
				toRemove.Add(kv.Key);
		}

		foreach (Node2D body in toRemove)
		{
			_hitTimes.Remove(body);
		}
	}

	public bool CanAttack()
	{
		return (Time.GetTicksMsec() - _lastAttackTimeMs) / 1000f >= _attackRate;
	}
}
