using DungeonAdventure.Characters;
using DungeonAdventure.Characters.Controllers;
using DungeonAdventure.Characters.Models;
using Godot;

namespace DungeonAdventure.Characters;

[GlobalClass]
public partial class EnemyCharacterControllerFactory : CharacterControllerFactory
{
    [Export] private float _chaseDistance = 100f;
    public override CharacterController Create(Views.CharacterView character, CharacterModel model)
    {
        return new EnemyController(character, model, _chaseDistance);
    }
    
}