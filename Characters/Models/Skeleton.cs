using System;
using System.Collections.Generic;
using DungeonAdventure.Items;

namespace DungeonAdventure.Characters.Models;

/// <summary>
/// Represents a Skeleton monster character.
/// </summary>
public partial class Skeleton : Monster
{
    public Skeleton(float health, float speed, 
        float damageMin, float damageMax, float attackRate,
        float hitChance, float blockChance, 
        IEnumerable<Item> items,
        string visual, string weapon) 
        : base("skeleton", health, speed, 
            damageMin, damageMax, attackRate, 
            hitChance, blockChance, items, visual, weapon)
    {
    }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="Skeleton"/> class with default values.
    /// </summary>
    public Skeleton() 
        : this(100, 120, 
            30, 50, 0.8f,
            0.8f, 0.1f, Array.Empty<Item>(), "Skeleton", "Bow")
    {
    }
}