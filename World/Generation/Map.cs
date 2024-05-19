using System.Collections.Generic;
using Godot;

namespace DungeonAdventure.World.Generation;

public class Map
{
    public IReadOnlyDictionary<Vector2I, Room> Rooms { get; }
    public Room StartingRoom { get; }

    public Map(IReadOnlyDictionary<Vector2I, Room> rooms, Room startingRoom)
    {
        Rooms = rooms;
        StartingRoom = startingRoom;
    }
}