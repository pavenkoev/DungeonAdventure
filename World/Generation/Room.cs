using System.Numerics;
using Godot;

namespace DungeonAdventure.World.Generation;

public class Room
{
    private RoomType _roomType;
    private Vector2I _coordinates;
    private PackedScene _scene;
    

    public RoomType RoomType => _roomType;
    public Vector2I Coordinates => _coordinates;
    public PackedScene Scene => _scene;
    public Room(RoomType roomType, Vector2I coordinates, PackedScene scene)
    {
        _roomType = roomType;
        _coordinates = coordinates;
        _scene = scene;
    }
}