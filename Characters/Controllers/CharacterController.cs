using DungeonAdventure.Characters.Effects;
using DungeonAdventure.Characters.Indicators;
using DungeonAdventure.Characters.Models;
using DungeonAdventure.Characters.Views;
using DungeonAdventure.Items;
using Godot;
using Item = DungeonAdventure.Items.Item;

namespace DungeonAdventure.Characters.Controllers;

/// <summary>
/// Represents an abstract base class for character controllers in the game.
/// </summary>
public abstract class CharacterController
{
    /// <summary>
    /// The view component associated with this character.
    /// </summary>
    protected CharacterView View;
    
    /// <summary>
    /// The model component associated with this character.
    /// </summary>
    protected CharacterModel Model;
    
    /// <summary>
    /// Manages the indicators for this character.
    /// </summary>
    public IndicatorManager IndicatorManager { get; set; }

    /// <summary>
    /// Stores the current character statistics.
    /// </summary>
    private CharacterStats _stats = new();
    
    /// <summary>
    /// Initializes a new instance of the <see cref="CharacterController"/> class.
    /// </summary>
    /// <param name="view">The view component.</param>
    /// <param name="model">The model component.</param>
    public CharacterController(CharacterView view, CharacterModel model)
    {
        View = view;
        Model = model;
    }

    /// <summary>
    /// Gets a value indicating whether this character is a player.
    /// </summary>
    public virtual bool IsPlayer => false;
    
    /// <summary>
    /// Processes the character logic each frame.
    /// </summary>
    /// <param name="delta">The elapsed time since the last frame.</param>
    public virtual void Process(double delta)
    {
        if (!Model.IsAlive)
            return;
        
        _stats = View.CollectEffects((float)delta);
		
        Vector2 direction = GetMoveDirection();

        if (_stats.HealRate != 0)
            Model.Heal(_stats.HealRate);
		
        View.Velocity = direction * Model.Speed * _stats.SpeedModifier;
		
        View.UpdateAnimation(View.Velocity);
		
        View.MoveAndSlide();

        ProcessAttack(_stats.DamageModifier);
    }
    
    /// <summary>
    /// Processes the physics-related logic each frame.
    /// </summary>
    /// <param name="delta">The elapsed time since the last frame.</param>
    public virtual void PhysicsProcess(double delta) {}
    
    /// <summary>
    /// Gets the direction for character movement.
    /// </summary>
    /// <returns>A <see cref="Vector2"/> representing the movement direction.</returns>
    public abstract Vector2 GetMoveDirection();
    
    /// <summary>
    /// Gets the direction for character attack.
    /// </summary>
    /// <returns>A nullable <see cref="Vector2"/> representing the attack direction.</returns>
    public abstract Vector2? GetAttackDirection();

    /// <summary>
    /// Applies damage to the character.
    /// </summary>
    /// <param name="damage">The amount of damage to apply.</param>
    public virtual void ApplyDamage(float damage)
    {
        if (!Model.IsAlive)
            return;

        if (damage <= 0)
        {
            IndicatorManager?.AddIndicator($"MISS", new Color(0.8f, 0.8f, 0.8f));
            return;
        }

        if (Model.RandomizeBlock())
        {
            IndicatorManager?.AddIndicator($"BLOCK", new Color(0.8f, 0.8f, 0.8f));
            return;
        }
		
        IndicatorManager?.AddIndicator($"-{(int)damage}", new Color(0.8f, 0, 0));
		
        View.PlayHitSound();
		
        Model.ApplyDamage(damage);
    }

    /// <summary>
    /// Heals the character.
    /// </summary>
    /// <param name="value">The amount of healing to apply.</param>
    /// <param name="duration">The duration over which to apply the healing.</param>
    public virtual void Heal(float value, float duration)
    {
        if (!Model.IsAlive)
            return;
		
        IndicatorManager?.AddIndicator($"+{(int)value}", new Color(0, 0.8f, 0));
		
        if (duration <= 0)
            Model.Heal(value);
        else
            View.AddEffect(new HealEffect(value, duration));
    }

    /// <summary>
    /// Determines whether the character can attack.
    /// </summary>
    /// <returns>True if the character can attack, otherwise false.</returns>
    public virtual bool CanAttack() => View.Weapon.CanAttack();
    
    /// <summary>
    /// Processes the character's attack logic.
    /// </summary>
    /// <param name="damageModifier">The modifier to apply to the attack damage.</param>
    private void ProcessAttack(float damageModifier)
    {
        Vector2? attackDirection = GetAttackDirection();
        
        if (attackDirection.HasValue)
        {
            View.Weapon.SetWeaponAttackSide(attackDirection.Value);

            if (CanAttack())
            {
                float damage = -1;
                if (!Model.RandomizeMiss())
                    damage = Model.RandomizeDamage() * damageModifier;
                View.Weapon.Attack(damage);
            }
        }
    }

    /// <summary>
    /// Determines whether the character can pick up the specified item.
    /// </summary>
    /// <param name="item">The item to check.</param>
    /// <returns>True if the character can pick up the item, otherwise false.</returns>
    public virtual bool CanPickupItem(Item item)
    {
        return Model.CanPickupItem(item);
    }
    
    /// <summary>
    /// Picks up an item and adds it to the character's inventory.
    /// </summary>
    /// <param name="item">The item to pick up.</param>
    public virtual void PickupItem(Item item)
    {
        Model.AddItem(item);
    }

    /// <summary>
    /// Determines whether the character can use the specified item.
    /// </summary>
    /// <param name="item">The item to check.</param>
    /// <returns>True if the character can use the item, otherwise false.</returns>
    public virtual bool CanUseItem(Item item)
    {
        return item.CanUse(View);
    }
    
    /// <summary>
    /// Uses an item from the character's inventory.
    /// </summary>
    /// <param name="item">The item to use.</param>
    public virtual void UseItem(Item item)
    {
        item.Use(View);
        Model.RemoveItem(item);
    }
}