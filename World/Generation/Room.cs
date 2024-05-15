using System.Numerics;
using Godot;

namespace DungeonAdventure.World.Generation;

public class Room
{
    public RoomType RoomType { get; }
    public Vector2I Coordinates { get; }
    public World.Room Node { get; }

    public Room(RoomType roomType, Vector2I coordinates, World.Room node)
    {
        RoomType = roomType;
        Coordinates = coordinates;
        Node = node;
    }
}