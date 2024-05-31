using System.Collections.Generic;
using DungeonAdventure.Items;

namespace DungeonAdventure.Characters.Models;

/// <summary>
/// Factory class for creating character models by name.
/// </summary>
public static class CharacterFactory
{
    /// <summary>
    /// Creates a character model based on the specified name and attributes.
    /// </summary>
    /// <param name="name">The name of the character type to create.</param>
    /// <param name="health">The health of the character.</param>
    /// <param name="speed">The speed of the character.</param>
    /// <param name="damageMin">The minimum damage of the character.</param>
    /// <param name="damageMax">The maximum damage of the character.</param>
    /// <param name="attackRate">The attack rate.</param>
    /// <param name="hitChance">The hit chance of the character.</param>
    /// <param name="blockChance">The block chance of the character.</param>
    /// <param name="items">The initial items of the character.</param>
    /// <param name="visual">The visual name of the character.</param>
    /// <param name="weapon">The weapon name of the character.</param>
    /// <returns>A <see cref="CharacterModel"/> instance based on the specified name, or null if the name is not recognized.</returns>
    public static CharacterModel CreateByName(string name, float health, float speed,
        float damageMin, float damageMax, float attackRate,
        float hitChance, float blockChance,
        IEnumerable<Item> items,
        string visual, string weapon)
    {
        switch (name.ToLower())
        {
            case "knight":
                return new Knight(health, speed, damageMin, damageMax, attackRate,
                    hitChance, blockChance, items, visual, weapon);
            case "rogue":
                return new Rogue(health, speed, damageMin, damageMax, attackRate,
                    hitChance, blockChance, items, visual, weapon);
            case "wizard":
                return new Wizard(health, speed, damageMin, damageMax, attackRate,
                    hitChance, blockChance, items, visual, weapon);
            case "orc":
                return new Orc(health, speed, damageMin, damageMax, attackRate,
                    hitChance, blockChance, items, visual, weapon);
            case "skeleton":
                return new Skeleton(health, speed, damageMin, damageMax, attackRate,
                    hitChance, blockChance, items, visual, weapon);
            case "ghost":
                return new Ghost(health, speed, damageMin, damageMax, attackRate,
                    hitChance, blockChance, items, visual, weapon);
        }

        return null;
    }
}