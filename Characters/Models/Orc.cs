using System;
using System.Collections.Generic;
using DungeonAdventure.Items;

namespace DungeonAdventure.Characters.Models;

/// <summary>
/// Represents a Orc monster character.
/// </summary>
public partial class Orc : Monster
{
    public Orc(float health, float speed, 
        float damageMin, float damageMax, 
        float hitChance, float blockChance, 
        IEnumerable<Item> items,
        string visual, string weapon) 
        : base("orc", health, speed, damageMin, damageMax, hitChance, blockChance, items, visual, weapon)
    {
    }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="Orc"/> class with default values.
    /// </summary>
    public Orc() 
        : this(200, 80, 
            30, 60, 
            0.6f, 0.1f, Array.Empty<Item>(), "Orc", "Sword")
    {
    }
}