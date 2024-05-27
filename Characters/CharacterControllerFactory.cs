using DungeonAdventure.Characters;
using DungeonAdventure.Characters.Controllers;
using DungeonAdventure.Characters.Models;
using Godot;

namespace DungeonAdventure.Characters;

/// <summary>
/// Abstract factory class for creating character controllers.
/// </summary>
[GlobalClass]
public abstract partial class CharacterControllerFactory : Resource
{
    /// <summary>
    /// Creates a character controller for the specified character and model.
    /// </summary>
    /// <param name="character">The character view to be controlled.</param>
    /// <param name="model">The model representing the character's data.</param>
    /// <returns>The created character controller.</returns>
    public abstract CharacterController Create(Views.CharacterView character, CharacterModel model);
}