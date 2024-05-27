using System.Collections.Generic;
using Godot;

namespace DungeonAdventure.World.Generation;

/// <summary>
/// Represents the map of the dungeon, containing rooms and the starting room.
/// </summary>
public class Map
{
    /// <summary>
    /// Gets the dictionary of rooms in the map, indexed by their coordinates.
    /// </summary>
    public IReadOnlyDictionary<Vector2I, Room> Rooms { get; }
    
    /// <summary>
    /// Gets the starting room of the map.
    /// </summary>
    public Room StartingRoom { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Map"/> class with the specified rooms and starting room.
    /// </summary>
    /// <param name="rooms">The dictionary of rooms in the map.</param>
    /// <param name="startingRoom">The starting room of the map.</param>
    public Map(IReadOnlyDictionary<Vector2I, Room> rooms, Room startingRoom)
    {
        Rooms = rooms;
        StartingRoom = startingRoom;
    }
}