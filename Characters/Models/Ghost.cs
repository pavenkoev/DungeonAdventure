using System;
using System.Collections.Generic;
using DungeonAdventure.Items;

namespace DungeonAdventure.Characters.Models;

/// <summary>
/// Represents a Ghost monster character.
/// </summary>
public partial class Ghost : Monster
{
    public Ghost(float health, float speed, 
        float damageMin, float damageMax, float attackRate,
        float hitChance, float blockChance, 
        IEnumerable<Item> items,
        string visual, string weapon) 
        : base("ghost", health, speed, 
            damageMin, damageMax, attackRate, 
            hitChance, blockChance, items, visual, weapon)
    {
    }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="Ghost"/> class with default values.
    /// </summary>
    public Ghost() 
        : this(100, 120, 
            30, 50, 1.2f,
            0.8f, 0.1f, Array.Empty<Item>(), "Ghost", "Wand")
    {
    }
}