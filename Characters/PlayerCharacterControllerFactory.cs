using DungeonAdventure.Characters;
using Godot;

namespace DungeonAdventure.Characters;

[GlobalClass]
public partial class PlayerCharacterControllerFactory : CharacterControllerFactory
{
    public override ICharacterController Create(Character character)
    {
        return new PlayerController(character);
    }
    
}