using System;
using System.Collections.Generic;
using DungeonAdventure.Items;
using Godot;
using Item = DungeonAdventure.Items.Item;

namespace DungeonAdventure.Characters.Models;

/// <summary>
/// Represents the model for a character in the game.
/// </summary>
public partial class CharacterModel : RefCounted
{
    /// <summary>
    /// Gets the current health of the character.
    /// </summary>
    public float Health { get; private set; } = 100;
    
    /// <summary>
    /// Gets the maximum health of the character.
    /// </summary>
    public float MaxHealth { get; private set; } = 100;
    
    /// <summary>
    /// Gets a value indicating whether the character is alive.
    /// </summary>
    public bool IsAlive { get; private set; } = true;
    
    /// <summary>
    /// Gets the movement speed of the character.
    /// </summary>
    public float Speed { get; private set; } = 100;

    /// <summary>
    /// Gets the minimum damage the character can inflict.
    /// </summary>
    public float DamageMin { get; private set; } = 10;
    
    /// <summary>
    /// Gets the maximum damage the character can inflict.
    /// </summary>
    public float DamageMax { get; private set; } = 30;

    /// <summary>
    /// Gets the chance to hit an attack.
    /// </summary>
    public float HitChance { get; private set; } = 0.9f;
    
    /// <summary>
    /// Gets the chance to block an attack.
    /// </summary>
    public float BlockChance { get; private set; } = 0.2f;

    /// <summary>
    /// Gets the list of items the character is carrying.
    /// </summary>
    public List<Item> Items { get; } = new();

    /// <summary>
    /// Gets the visual name of the character.
    /// </summary>
    public string VisualName { get; private set; } = "Rogue";
    
    /// <summary>
    /// Gets the name of the weapon the character is using.
    /// </summary>
    public string WeaponName { get; private set; } = "Bow";

    /// <summary>
    /// Signal emitted when the character dies.
    /// </summary>
    [Signal]
    public delegate void CharacterDiedEventHandler();
    
    [Signal]
    public delegate void HealthChangedEventHandler(float health);
    
    /// <summary>
    /// Signal emitted when the items carried by the character change.
    /// </summary>
    [Signal]
    public delegate void ItemsChangedEventHandler();
    
    private Random _random = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="CharacterModel"/> class with specified attributes.
    /// </summary>
    /// <param name="health">The initial and maximum health of the character.</param>
    /// <param name="speed">The movement speed of the character.</param>
    /// <param name="damageMin">The minimum damage the character can inflict.</param>
    /// <param name="damageMax">The maximum damage the character can inflict.</param>
    /// <param name="hitChance">The chance to hit an attack.</param>
    /// <param name="blockChance">The chance to block an attack.</param>
    /// <param name="items">The initial items carried by the character.</param>
    /// <param name="visual">The visual name of the character.</param>
    /// <param name="weapon">The name of the weapon the character is using.</param>

    public CharacterModel(float health, float speed, 
        float damageMin, float damageMax,
        float hitChance, float blockChance,
        IEnumerable<Item> items, string visual, string weapon)
    {
        Health = health;
        MaxHealth = health;
        IsAlive = true;
        Speed = speed;

        DamageMin = damageMin;
        DamageMax = damageMax;

        HitChance = hitChance;
        BlockChance = blockChance;
        
        Items.AddRange(items);

        VisualName = visual;
        WeaponName = weapon;
    }
    
    /// <summary>
    /// Randomizes the damage dealt by the character within the specified range.
    /// </summary>
    /// <returns>The randomized damage value.</returns>
    public float RandomizeDamage()
    {
        return DamageMin + (float)_random.NextDouble() * (DamageMax - DamageMin);
    }

    /// <summary>
    /// Randomizes whether the character misses an attack.
    /// </summary>
    /// <returns>True if the attack misses, otherwise false.</returns>
    public bool RandomizeMiss()
    {
        return _random.NextDouble() > HitChance;
    }

    /// <summary>
    /// Randomizes whether the character blocks an attack.
    /// </summary>
    /// <returns>True if the attack is blocked, otherwise false.</returns>
    public bool RandomizeBlock()
    {
        return _random.NextDouble() <= BlockChance;
    }
    
    /// <summary>
    /// Applies damage to the character and handles death if health drops to zero.
    /// </summary>
    /// <param name="damage">The amount of damage to apply.</param>
    public void ApplyDamage(float damage)
    {
        if (!IsAlive)
            return;

        if (damage <= 0)
            return;
		
        Health = Mathf.Clamp(Health - damage, 0, MaxHealth);

        EmitSignal(SignalName.HealthChanged, Health);
        
        if (Health <= 0)
        {
            Health = 0;
            Die();
        }
    }

    /// <summary>
    /// Kills the character and emits the character died signal.
    /// </summary>
    public void Die()
    {
        Health = 0;
        IsAlive = false;
        
        EmitSignal(SignalName.HealthChanged, Health);
        EmitSignal(SignalName.CharacterDied);
    }
    
    /// <summary>
    /// Heals the character by the specified amount.
    /// </summary>
    /// <param name="value">The amount to heal.</param>
    public void Heal(float value)
    {
        if (!IsAlive)
            return;
        
        Health = Mathf.Clamp(Health + value, 0, MaxHealth);
            
        EmitSignal(SignalName.HealthChanged, Health);
    }

    /// <summary>
    /// Determines whether the character can pick up the specified item.
    /// </summary>
    /// <param name="item">The item to pick up.</param>
    /// <returns>True if the character can pick up the item, otherwise false.</returns>

    public bool CanPickupItem(Item item)
    {
        return true;
    }

    /// <summary>
    /// Adds the specified item to the character's inventory and emits the items changed signal.
    /// </summary>
    /// <param name="item">The item to add.</param>
    public void AddItem(Item item)
    {
        Items.Add(item);
        EmitSignal(SignalName.ItemsChanged);
    }

    /// <summary>
    /// Removes the specified item from the character's inventory and emits the items changed signal.
    /// </summary>
    /// <param name="item">The item to remove.</param>
    public void RemoveItem(Item item)
    {
        Items.Remove(item);
        EmitSignal(SignalName.ItemsChanged);
    }

    /// <summary>
    /// Sets the visual representation of the character.
    /// </summary>
    /// <param name="visual">The name of the visual to set.</param>
    public void SetVisual(string visual)
    {
        VisualName = visual;
    }

    /// <summary>
    /// Sets the weapon of the character.
    /// </summary>
    /// <param name="weapon">The name of the weapon to set.</param>
    public void SetWeapon(string weapon)
    {
        WeaponName = weapon;
    }
}