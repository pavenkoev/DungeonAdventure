using DungeonAdventure.Characters;
using DungeonAdventure.Characters.Controllers;
using DungeonAdventure.Characters.Models;
using Godot;

namespace DungeonAdventure.Characters;

/// <summary>
/// Factory class for creating player character controllers.
/// </summary>
[GlobalClass]
public partial class PlayerCharacterControllerFactory : CharacterControllerFactory
{
    /// <summary>
    /// Creates a player character controller with the specified character view and model.
    /// </summary>
    /// <param name="character">The character view to be controlled.</param>
    /// <param name="model">The model representing the character's data.</param>
    /// <returns>The created player character controller.</returns>
    public override CharacterController Create(Views.CharacterView character, CharacterModel model)
    {
        return new PlayerController(character, model);
    }
    
}