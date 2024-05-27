using System.Numerics;
using Godot;

namespace DungeonAdventure.World.Generation;

/// <summary>
/// Represents a room in the dungeon generation process.
/// </summary>
public class Room
{
    /// <summary>
    /// Gets the type of the room.
    /// </summary>
    public RoomType RoomType { get; }
    
    /// <summary>
    /// Gets the coordinates of the room.
    /// </summary>
    public Vector2I Coordinates { get; }
    
    /// <summary>
    /// Gets the actual room node in the world.
    /// </summary>
    public World.Room Node { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Room"/> class with the specified room type, coordinates, and node.
    /// </summary>
    /// <param name="roomType">The type of the room.</param>
    /// <param name="coordinates">The coordinates of the room.</param>
    /// <param name="node">The actual room node in the world.</param>
    public Room(RoomType roomType, Vector2I coordinates, World.Room node)
    {
        RoomType = roomType;
        Coordinates = coordinates;
        Node = node;
    }
}