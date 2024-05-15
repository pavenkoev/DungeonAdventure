using System.Collections.Generic;
using Godot;

namespace DungeonAdventure.World.Generation;

public class Map
{
    private readonly Dictionary<Vector2I, Room> _rooms;

    public IReadOnlyDictionary<Vector2I, Room> Rooms => _rooms;
    public Room StartingRoom { get; }

    public Map(Dictionary<Vector2I, Room> rooms, Room startingRoom)
    {
        _rooms = rooms;
        StartingRoom = startingRoom;
    }
}