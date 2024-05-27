using DungeonAdventure.Characters;
using DungeonAdventure.Characters.Controllers;
using DungeonAdventure.Characters.Models;
using Godot;

namespace DungeonAdventure.Characters;

/// <summary>
/// Factory class for creating enemy character controllers.
/// </summary>
[GlobalClass]
public partial class EnemyCharacterControllerFactory : CharacterControllerFactory
{
    /// <summary>
    /// The distance within which the enemy will start chasing the player.
    /// </summary>
    [Export] private float _chaseDistance = 100f;
    
    /// <summary>
    /// Creates an enemy character controller with the specified character view and model.
    /// </summary>
    /// <param name="character">The character view to be controlled.</param>
    /// <param name="model">The model representing the character's data.</param>
    /// <returns>The created enemy character controller.</returns>
    public override CharacterController Create(Views.CharacterView character, CharacterModel model)
    {
        return new EnemyController(character, model, _chaseDistance);
    }
    
}