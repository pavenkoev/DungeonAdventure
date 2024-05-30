using System;
using System.Collections.Generic;
using DungeonAdventure.Items;

namespace DungeonAdventure.Characters.Models;

/// <summary>
/// Represents a Rogue hero character.
/// </summary>
public partial class Rogue : Hero
{
    public Rogue(float health, float speed, 
        float damageMin, float damageMax, 
        float hitChance, float blockChance, 
        IEnumerable<Item> items,
        string visual, string weapon) 
        : base(health, speed, damageMin, damageMax, hitChance, blockChance, items, visual, weapon)
    {
    }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="Rogue"/> class with default values.
    /// </summary>
    public Rogue() 
        : this(75, 150, 
            20, 40, 
            0.8f, 0.4f, Array.Empty<Item>(), "Rogue", "Bow")
    {
    }
}