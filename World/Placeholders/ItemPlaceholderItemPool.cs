using System;
using DungeonAdventure.Items;
using Godot;

namespace DungeonAdventure.World.Placeholders;

/// <summary>
/// Represents a pool of items that can be used by an <see cref="ItemPlaceholder"/> to spawn items in the dungeon.
/// </summary>
[Tool]
[GlobalClass]
public partial class ItemPlaceholderItemPool : Resource
{
    /// <summary>
    /// Gets the array of items in the pool.
    /// </summary>
    [Export] public Item[] Items { get; private set; }

    /// <summary>
    /// Gets a random item from the pool.
    /// </summary>
    /// <param name="random">The random number generator to use.</param>
    /// <returns>A random item from the pool, or null if the pool is empty.</returns>

    public Item GetRandomItem(Random random)
    {
        if (Items == null || Items.Length == 0)
            return null;

        return Items[random.Next(Items.Length)];
    }
}