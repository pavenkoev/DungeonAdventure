using System.Collections.Generic;
using DungeonAdventure.Characters.Views;
using DungeonAdventure.Weapons.Models;
using Godot;

namespace DungeonAdventure.Weapons.View;

public abstract partial class WeaponView : Node2D
{
    [Export] private Node2D _weaponPivot;

    public WeaponModel Model { get; protected set; }
    
    private readonly HashSet<Node2D> _ignoredBodies = new();
    
    public virtual float AttackRange => Model.AttackRange;
    public virtual float AttackRate => Model.AttackRate;
    public bool CanAttack() => Model.CanAttack();
    public abstract void Attach(CharacterView character);
    public abstract void Attack(float damage);

    protected abstract float GetAttackAnimationLength();
    public abstract bool IsAttacking();

    protected float GetAnimationSpeedMultiplier()
    {
        return GetAttackAnimationLength() / Model.AttackRate;
    }
    
    public void ClearIgnoredBodies()
    {
        _ignoredBodies.Clear();
    }

    public void AddIgnoredBodies(Node2D body)
    {
        _ignoredBodies.Add(body);
    }

    public bool IsBodyIgnored(Node2D body)
    {
        return _ignoredBodies.Contains(body);
    }
    
    
    public void SetWeaponAttackSide(Vector2 direction)
    {
        Vector2 forward = new Vector2(1, 0);

        _weaponPivot.Rotation = forward.AngleTo(direction);

        if (direction.Y >= direction.X)
            _weaponPivot.Scale = new Vector2(1, -1);
        else
            _weaponPivot.Scale = new Vector2(1, 1);
    }
}