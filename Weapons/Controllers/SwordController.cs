using System.Collections.Generic;
using DungeonAdventure.Characters.Views;
using DungeonAdventure.Utils;
using DungeonAdventure.Weapons.Models;
using DungeonAdventure.Weapons.View;
using Godot;

namespace DungeonAdventure.Weapons.Controllers;

public class SwordController
{
    private WeaponModel _model;
    private SwordView _view;
    private float _damage = 0;
    
    // keep track of the bodies the sword hit and times when it happened
    private readonly Dictionary<Node2D, ulong> _hitTimes = new();
    
    private const float MsToSecondFactor = 1.0f / 1000;

    public SwordController(SwordView view, WeaponModel model)
    {
        _model = model;
        _view = view;
    }

    public void Attack(float damage)
    {
	    if (!_model.CanAttack() || _view.IsAttacking())
		    return;
	    
	    _model.SetLastAttackTime();
	    _damage = damage;
    }
    
    public void OnSwordCollision(Node2D node)
    {
    	if (_view.IsAttacking())
    		ProcessHit(node, _damage);
    }
    
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

    // check if we are not hitting the same thing multiple times too soon
    // to prevent extra hits that should not happen
    private bool CheckIfShouldHitAndUpdateTimes(Node2D body)
    {
    	ulong currentTime = Time.GetTicksMsec();
    	ulong lastHitTime = _hitTimes.GetValueOrDefault(body, 0ul);

    	if ((currentTime - lastHitTime) * MsToSecondFactor < _model.AttackRate)
    		return false;

    	_hitTimes[body] = currentTime;
    	return true;
    }

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