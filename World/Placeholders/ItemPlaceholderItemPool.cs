using System;
using DungeonAdventure.Items;
using Godot;

namespace DungeonAdventure.World.Placeholders;

[Tool]
[GlobalClass]
public partial class ItemPlaceholderItemPool : Resource
{
    [Export] public Item[] Items { get; private set; }

    public Item GetRandomItem(Random random)
    {
        if (Items == null || Items.Length == 0)
            return null;

        return Items[random.Next(Items.Length)];
    }
    
}