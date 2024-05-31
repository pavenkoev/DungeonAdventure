using System.Collections.Generic;
using DungeonAdventure.Items;

namespace DungeonAdventure.Characters.Models;

/// <summary>
/// Represents a hero character in the game.
/// </summary>
public abstract partial class Hero : CharacterModel
{
    protected Hero(string modelName, float health, float speed, 
        float damageMin, float damageMax, 
        float hitChance, float blockChance, 
        IEnumerable<Item> items,
        string visual, string weapon) 
        : base(modelName, health, speed, damageMin, damageMax, hitChance, blockChance, items, visual, weapon)
    {
    }
}