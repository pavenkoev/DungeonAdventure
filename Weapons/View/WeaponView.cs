using System.Collections.Generic;
using DungeonAdventure.Characters.Views;
using DungeonAdventure.Weapons.Models;
using Godot;

namespace DungeonAdventure.Weapons.View;

/// <summary>
/// Represents the base class for weapon views in the game.
/// </summary>
public abstract partial class WeaponView : Node2D
{
    /// <summary>
    /// The pivot point for the weapon's rotation.
    /// </summary>
    [Export] private Node2D _weaponPivot;

    /// <summary>
    /// Gets the weapon model associated with this weapon view.
    /// </summary>
    public WeaponModel Model { get; protected set; }
    
    private readonly HashSet<Node2D> _ignoredBodies = new();
    
    /// <summary>
    /// Gets the attack range of the weapon.
    /// </summary>
    public float AttackRange => Model.AttackRange;
    
    /// <summary>
    /// Gets the attack rate of the weapon.
    /// </summary>
    public float AttackRate => Model.AttackRate;
    
    /// <summary>
    /// Determines whether the weapon can attack.
    /// </summary>
    /// <returns>True if the weapon can attack, otherwise false.</returns>
    public bool CanAttack() => Model.CanAttack();
    
    /// <summary>
    /// Attaches the weapon to a character.
    /// </summary>
    /// <param name="character">The character to attach the weapon to.</param>
    public abstract void Attach(CharacterView character);
    
    /// <summary>
    /// Executes the attack action with the specified damage.
    /// </summary>
    /// <param name="damage">The amount of damage to inflict.</param>
    public abstract void Attack(float damage);

    /// <summary>
    /// Gets the length of the attack animation.
    /// </summary>
    /// <returns>The length of the attack animation.</returns>
    protected abstract float GetAttackAnimationLength();
    
    /// <summary>
    /// Determines whether the weapon is currently attacking.
    /// </summary>
    /// <returns>True if the weapon is attacking, otherwise false.</returns>
    public abstract bool IsAttacking();

    /// <summary>
    /// Gets the speed multiplier for the attack animation.
    /// </summary>
    /// <returns>The animation speed multiplier.</returns>
    protected float GetAnimationSpeedMultiplier()
    {
        return GetAttackAnimationLength() / Model.AttackRate;
    }
    
    /// <summary>
    /// Clears the list of bodies ignored by the weapon.
    /// </summary>
    public void ClearIgnoredBodies()
    {
        _ignoredBodies.Clear();
    }

    /// <summary>
    /// Adds a body to the list of bodies ignored by the weapon.
    /// </summary>
    /// <param name="body">The body to ignore.</param>
    public void AddIgnoredBody(Node2D body)
    {
        _ignoredBodies.Add(body);
    }

    /// <summary>
    /// Determines whether the specified body is ignored by the weapon.
    /// </summary>
    /// <param name="body">The body to check.</param>
    /// <returns>True if the body is ignored, otherwise false.</returns>
    public bool IsBodyIgnored(Node2D body)
    {
        return _ignoredBodies.Contains(body);
    }
    
    /// <summary>
    /// Sets the attack direction of the weapon based on the specified direction.
    /// </summary>
    /// <param name="direction">The direction to set the weapon attack side.</param>
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