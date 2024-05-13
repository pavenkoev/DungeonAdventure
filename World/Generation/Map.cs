using System.Collections.Generic;
using Godot;

namespace DungeonAdventure.World.Generation;

public class Map
{
    private Dictionary<Vector2I, Room> _rooms;
    private Room _startingRoom;
    
    public IReadOnlyDictionary<Vector2I, Room> Rooms => _rooms;
    public Room StartingRoom => _startingRoom;

    public Map(Dictionary<Vector2I, Room> rooms, Room startingRoom)
    {
        _rooms = rooms;
        _startingRoom = startingRoom;
    }
}