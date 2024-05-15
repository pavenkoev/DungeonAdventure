using System.Collections.Generic;
using DungeonAdventure.Characters;
using Godot;

namespace DungeonAdventure.Weapons;

public abstract partial class Weapon : Node2D
{
    
    [Export] private float _damage = 20.0f;
    [Export] private float _attackRate = 0.25f;
    [Export] private float _attackRange = 300.0f;
    
    private ulong _lastAttackTimeMs = 0;
    private readonly HashSet<Node2D> _ignoredBodies = new();

    private const float MsToSecondFactor = 1.0f / 1000;

    public virtual float Damage => _damage;
    public virtual float AttackRange => _attackRange;
    public virtual float AttackRate => _attackRate;
    public abstract void Attach(Character character);
    public abstract void Attack();
    public virtual bool CanAttack() => CheckAttackRate();

    protected void ClearIgnoredBodies()
    {
        _ignoredBodies.Clear();
    }

    protected void AddIgnoredBodies(Node2D body)
    {
        _ignoredBodies.Add(body);
    }

    protected bool IsBodyIgnored(Node2D body)
    {
        return _ignoredBodies.Contains(body);
    }

    protected bool CheckAttackRate()
    {
        return (Time.GetTicksMsec() - _lastAttackTimeMs) * MsToSecondFactor >= AttackRate;
    }

    protected void SetLastAttackTime()
    {
        _lastAttackTimeMs = Time.GetTicksMsec();
    }
}