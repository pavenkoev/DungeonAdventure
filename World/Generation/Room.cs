using System.Numerics;
using Godot;

namespace DungeonAdventure.World.Generation;

public class Room
{
    private RoomType _roomType;
    private Vector2I _coordinates;
    private World.Room _node;
    

    public RoomType RoomType => _roomType;
    public Vector2I Coordinates => _coordinates;
    public World.Room Node => _node;
    public Room(RoomType roomType, Vector2I coordinates, World.Room node)
    {
        _roomType = roomType;
        _coordinates = coordinates;
        _node = node;
    }
}