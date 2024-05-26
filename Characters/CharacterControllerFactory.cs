using DungeonAdventure.Characters;
using DungeonAdventure.Characters.Controllers;
using DungeonAdventure.Characters.Models;
using Godot;

namespace DungeonAdventure.Characters;

[GlobalClass]
public abstract partial class CharacterControllerFactory : Resource
{
    public abstract CharacterController Create(Views.CharacterView character, CharacterModel model);
}