using DungeonAdventure.Characters;
using Godot;

namespace DungeonAdventure.Characters;

[GlobalClass]
public abstract partial class CharacterControllerFactory : Resource
{
    public abstract ICharacterController Create(Character character);
}