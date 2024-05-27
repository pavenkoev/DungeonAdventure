using System.Collections.Generic;
using DungeonAdventure.Characters.Views;
using DungeonAdventure.Utils;
using DungeonAdventure.Weapons.Models;
using DungeonAdventure.Weapons.View;
using Godot;

namespace DungeonAdventure.Weapons.Controllers;

/// <summary>
/// Controls the behavior of a sword weapon.
/// </summary>
public class SwordController
{
    private WeaponModel _model;
    private SwordView _view;
    private float _damage = 0;
    
    // keep track of the bodies the sword hit and times when it happened
    private readonly Dictionary<Node2D, ulong> _hitTimes = new();
    
    private const float MsToSecondFactor = 1.0f / 1000;

    /// <summary>
    /// Initializes a new instance of the <see cref="SwordController"/> class.
    /// </summary>
    /// <param name="view">The view component of the sword.</param>
    /// <param name="model">The model component of the sword.</param>
    public SwordController(SwordView view, WeaponModel model)
    {
        _model = model;
        _view = view;
    }

    /// <summary>
    /// Executes the attack action with the specified damage.
    /// </summary>
    /// <param name="damage">The amount of damage to inflict.</param>
    public void Attack(float damage)
    {
	    if (!_model.CanAttack() || _view.IsAttacking())
		    return;
	    
	    _model.SetLastAttackTime();
	    _damage = damage;
    }
    
    /// <summary>
    /// Handles the collision of the sword with another body.
    /// </summary>
    /// <param name="node">The body that the sword collides with.</param>
    public void OnSwordCollision(Node2D node)
    {
    	if (_view.IsAttacking())
    		ProcessHit(node, _damage);
    }
    
    /// <summary>
    /// Processes a hit on a body and applies damage if applicable.
    /// </summary>
    /// <param name="body">The body to hit.</param>
    /// <param name="damageModifier">The damage modifier to apply.</param>
    private void ProcessHit(Node2D body, float damageModifier)
    {
    	if (!_view.IsBodyIgnored(body) && CheckIfShouldHitAndUpdateTimes(body))
    	{
    		CharacterView character = body.FindCharacter();
    		if (character != null)
    		{
    			GD.Print("HIT: " + character.Name);
    			character.ApplyDamage(damageModifier);
    		}
    	}
    }

    /// <summary>
    /// Checks if the sword should hit the body again and updates the hit times.
    /// Prevents hitting the same body multiple times too soon.
    /// </summary>
    /// <param name="body">The body to check.</param>
    /// <returns>True if the sword should hit the body, otherwise false.</returns>
    private bool CheckIfShouldHitAndUpdateTimes(Node2D body)
    {
    	ulong currentTime = Time.GetTicksMsec();
    	ulong lastHitTime = _hitTimes.GetValueOrDefault(body, 0ul);

    	if ((currentTime - lastHitTime) * MsToSecondFactor < _model.AttackRate)
    		return false;

    	_hitTimes[body] = currentTime;
    	return true;
    }

    /// <summary>
    /// Cleans up the hit times dictionary by removing entries that are no longer valid.
    /// </summary>
    public void CleanupHitTimes()
    {
    	ulong currentTime = Time.GetTicksMsec();
    	List<Node2D> toRemove = new();
    	foreach (var kv in _hitTimes)
    	{
    		if ((currentTime - kv.Value) * MsToSecondFactor >= _model.AttackRate)
    			toRemove.Add(kv.Key);
    	}

    	foreach (Node2D body in toRemove)
    	{
    		_hitTimes.Remove(body);
    	}
    }
}