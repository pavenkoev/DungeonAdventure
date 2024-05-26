using DungeonAdventure.Characters;
using DungeonAdventure.Characters.Views;
using Godot;

namespace DungeonAdventure.Items;

[Tool]
[GlobalClass]
public abstract partial class Item : Resource
{
    [Export] public Texture2D Icon { get; private set; }
    [Export] public PackedScene Visual { get; private set; }

    public virtual bool CanUse(CharacterView character) => true;

    public abstract void Use(CharacterView character);
}