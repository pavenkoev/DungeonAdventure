using DungeonAdventure.Characters.Models;
using DungeonAdventure.Items;
using Godot;
using Godot.Collections;
using Item = DungeonAdventure.Items.Item;

namespace DungeonAdventure.Characters;

/// <summary>
/// Factory class for creating character models.
/// </summary>
[Tool]
[GlobalClass]
public abstract partial class CharacterModelFactory : Resource
{
    public abstract CharacterModel CreateModel();
}