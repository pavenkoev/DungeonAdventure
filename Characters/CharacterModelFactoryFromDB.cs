using DungeonAdventure.Characters.Models;
using Godot;

namespace DungeonAdventure.Characters;

/// <summary>
/// A factory class for creating character models from a database.
/// </summary>
[Tool]
[GlobalClass]
public partial class CharacterModelFactoryFromDB : CharacterModelFactory
{
    /// <summary>
    /// The name of the character to load from the database.
    /// </summary>
    [Export] private string _name = "";
    
    /// <summary>
    /// Creates a character model by loading data from the database.
    /// </summary>
    /// <returns>The character model loaded from the database.</returns>
    public override CharacterModel CreateModel()
    {
        using CharacterData characterData = new();
        return characterData.LoadCharacter(_name);
    }
}