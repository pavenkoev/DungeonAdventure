using DungeonAdventure.Characters.Models;
using DungeonAdventure.Items;
using Godot;
using Godot.Collections;

namespace DungeonAdventure.Characters;

/// <summary>
/// A factory class for manually creating character models with specified attributes.
/// </summary>
[Tool]
[GlobalClass]
public partial class CharacterModelFactoryManual : CharacterModelFactory
{
    [Export] private float _speed = 80.0f;
    [Export] private float _health = 100.0f;
    [Export] private float _damageMin = 10f;
    [Export] private float _damageMax = 30f;
    [Export] private float _attackRate = 1;
    [Export] private float _hitChance = 0.8f;
    [Export] private float _blockChance = 0.3f;

    [Export] private Array<Item> _items = new();

    [Export] private string _visual = "Rogue";
    [Export] private string _weapon = "Bow";
    
    /// <summary>
    /// Creates a new character model with the specified attributes.
    /// </summary>
    /// <returns>The created character model.</returns>
    public override CharacterModel CreateModel()
    {
        return new CharacterModel("", _health, _speed, _damageMin, _damageMax, _attackRate,
            _hitChance, _blockChance, _items, 
            _visual, _weapon);
    }
}