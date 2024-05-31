using System;
using System.Collections.Generic;
using DungeonAdventure.Items;

namespace DungeonAdventure.Characters.Models;

/// <summary>
/// Represents a Wizard hero character.
/// </summary>
public partial class Wizard : Hero
{
    public Wizard(float health, float speed, 
        float damageMin, float damageMax, 
        float hitChance, float blockChance, 
        IEnumerable<Item> items,
        string visual, string weapon) 
        : base("wizard", health, speed, damageMin, damageMax, hitChance, blockChance, items, visual, weapon)
    {
    }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="Wizard"/> class with default values.
    /// </summary>
    public Wizard() 
        : this(75, 150, 
            25, 45, 
            0.7f, 0.3f, Array.Empty<Item>(), "Wizard", "Wand")
    {
    }
}