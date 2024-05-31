using System;
using System.Collections.Generic;
using DungeonAdventure.Items;

namespace DungeonAdventure.Characters.Models;

/// <summary>
/// Represents a Knight hero character.
/// </summary>
public partial class Knight : Hero
{
    public Knight(float health, float speed, 
        float damageMin, float damageMax, float attackRate,
        float hitChance, float blockChance, 
        IEnumerable<Item> items,
        string visual, string weapon) 
        : base("knight", health, speed, 
            damageMin, damageMax, attackRate, 
            hitChance, blockChance, items, visual, weapon)
    {
    }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="Knight"/> class with default values.
    /// </summary>
    public Knight() 
        : this(125, 120, 
            35, 65, 0.5f,
            0.8f, 0.2f, Array.Empty<Item>(), "Knight", "Sword")
    {
    }
}