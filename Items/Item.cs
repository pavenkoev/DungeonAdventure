using DungeonAdventure.Characters;
using Godot;

namespace DungeonAdventure.Items;

[Tool]
[GlobalClass]
public abstract partial class Item : Resource
{
    [Export] public Texture2D Icon { get; private set; }
    [Export] public PackedScene Visual { get; private set; }

    public abstract void Use(Character character);
}