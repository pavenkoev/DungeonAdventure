using System.Numerics;
using Godot;

namespace DungeonAdventure.World.Generation;

public class Room
{
    private RoomType _roomType;
    private Vector2I _coordinates;
    

    public RoomType RoomType => _roomType;
    public Vector2I Coordinates => _coordinates;

    public Room(RoomType roomType, Vector2I coordinates)
    {
        _roomType = roomType;
        _coordinates = coordinates;
    }

    public void SetRoomType(RoomType roomType)
    {
        _roomType = roomType;
    }
}