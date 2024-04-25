using DungeonAdventure.Characters;
using Godot;

namespace DungeonAdventure.Characters;

[GlobalClass]
public partial class EnemyCharacterControllerFactory : CharacterControllerFactory
{
    [Export] private float _chaseDistance = 100f;
    public override ICharacterController Create(Character character)
    {
        return new EnemyController(character, _chaseDistance);
    }
    
}