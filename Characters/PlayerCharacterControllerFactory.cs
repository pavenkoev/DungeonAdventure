using DungeonAdventure.Characters;
using DungeonAdventure.Characters.Controllers;
using DungeonAdventure.Characters.Models;
using Godot;

namespace DungeonAdventure.Characters;

[GlobalClass]
public partial class PlayerCharacterControllerFactory : CharacterControllerFactory
{
    public override CharacterController Create(Views.CharacterView character, CharacterModel model)
    {
        return new PlayerController(character, model);
    }
    
}