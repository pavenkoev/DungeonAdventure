using System;
using System.Collections.Generic;
using DungeonAdventure.Items;
using Godot;

namespace DungeonAdventure.Characters.Models;

public partial class CharacterModel : RefCounted
{
    public float Health { get; private set; } = 100;
    public float MaxHealth { get; private set; } = 100;
    public bool IsAlive { get; private set; } = true;
    public float Speed { get; private set; } = 100;

    public float DamageMin { get; private set; } = 10;
    public float DamageMax { get; private set; } = 30;

    public float HitChance { get; private set; } = 0.9f;
    public float BlockChance { get; private set; } = 0.2f;

    public List<Item> Items { get; } = new();

    public string VisualName { get; private set; } = "Rogue";
    public string WeaponName { get; private set; } = "Bow";

    [Signal]
    public delegate void CharacterDiedEventHandler();
    [Signal]
    public delegate void ItemsChangedEventHandler();
    
    private Random _random = new();

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
    
    public float RandomizeDamage()
    {
        return DamageMin + (float)_random.NextDouble() * (DamageMax - DamageMin);
    }

    public bool RandomizeMiss()
    {
        return _random.NextDouble() > HitChance;
    }

    public bool RandomizeBlock()
    {
        return _random.NextDouble() <= BlockChance;
    }
    
    public void ApplyDamage(float damage)
    {
        if (!IsAlive)
            return;

        if (damage <= 0)
            return;
		
        Health -= damage;
        if (Health <= 0)
        {
            Health = 0;
            Die();
        }
        
        GD.Print("health: " + Health);
    }

    public void Die()
    {
        Health = 0;
        IsAlive = false;
        EmitSignal(SignalName.CharacterDied);
    }
    
    public void Heal(float value)
    {
        if (!IsAlive)
            return;
        
        Health += value;
        
        if (Health > MaxHealth)
            Health = MaxHealth;
            
        GD.Print("health: " + Health);
    }

    public bool CanPickupItem(Item item)
    {
        return true;
    }

    public void AddItem(Item item)
    {
        Items.Add(item);
        EmitSignal(SignalName.ItemsChanged);
    }

    public void RemoveItem(Item item)
    {
        Items.Remove(item);
        EmitSignal(SignalName.ItemsChanged);
    }
}