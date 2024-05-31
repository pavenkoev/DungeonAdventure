using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

namespace DungeonAdventure.World.Generation;

/// <summary>
/// Responsible for generating the map of the dungeon.
/// </summary>
public class MapGenerator
{
    private class RoomInfo
    {
        private RoomType _roomType;

        /// <summary>
        /// Gets the type of the room.
        /// </summary>
        public RoomType RoomType => _roomType;
        
        /// <summary>
        /// Gets the coordinates of the room.
        /// </summary>
        public Vector2I Coordinates { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RoomInfo"/> class with the specified room type and coordinates.
        /// </summary>
        /// <param name="roomType">The type of the room.</param>
        /// <param name="coordinates">The coordinates of the room.</param>
        public RoomInfo(RoomType roomType, Vector2I coordinates)
        {
            _roomType = roomType;
            Coordinates = coordinates;
        }

        /// <summary>
        /// Sets the type of the room.
        /// </summary>
        /// <param name="roomType">The new type of the room.</param>
        public void SetRoomType(RoomType roomType)
        {
            _roomType = roomType;
        }
    }
    
    private readonly Dictionary<Vector2I, RoomInfo> _roomMap = new();
    private readonly List<RoomInfo> _rooms = new();
    private Random _random;
    
    private static readonly Vector2I[] RoomOffsets = new[]
    {
        new Vector2I(1, 0), new Vector2I(-1, 0),
        new Vector2I(0, 1), new Vector2I(0, -1)
    };

    private const int MinNumberOfRooms = 2 + 4; // start + end rooms + 4 pillar rooms
    
    /// <summary>
    /// Generates a map using the specified settings.
    /// </summary>
    /// <param name="settings">The settings to use for generating the map.</param>
    /// <returns>The generated map.</returns>
    public Map Generate(MapGenerationSettings settings)
    {
        _random = new(settings.Seed);
        
        int numOfRooms = settings.NumberOfRooms;
        numOfRooms = Math.Max(numOfRooms, MinNumberOfRooms);
        
        RoomInfo startingRoom = new RoomInfo(RoomType.Start, new Vector2I(0, 0));

        _roomMap[new Vector2I(0, 0)] = startingRoom;
        _rooms.Add(startingRoom);

        int farthestDistance = 0;
        
        for (int i = 0; i < numOfRooms - 1; i++)
        {
            RoomInfo room = GenerateRandomRoom();
            if (room == null)
            {
                GD.PrintErr("Failed to generate a room");
                continue;
            }
            
            Vector2I coord = room.Coordinates;
            
            _roomMap[coord] = room;
            _rooms.Add(room);

            farthestDistance = Math.Max(farthestDistance, (int)coord.Length());
        }
        
        AssignSpecialRooms();

        RoomGenerator roomGenerator = new(settings, _random.Next(), farthestDistance);

        Dictionary<Vector2I, Room> rooms = GenerateRooms(roomGenerator);
        return new Map(rooms, rooms[startingRoom.Coordinates]);
    }

    /// <summary>
    /// Checks if a room exists at the specified coordinates.
    /// </summary>
    /// <param name="coord">The coordinates to check.</param>
    /// <returns>True if a room exists at the specified coordinates, otherwise false.</returns>
    private bool DoesRoomExist(Vector2I coord)
    {
        if (_roomMap.TryGetValue(coord, out RoomInfo room))
            return room != null;
        return false;
    }
    
    /// <summary>
    /// Finds all possible coordinates where a new room can be placed.
    /// </summary>
    /// <returns>A set of possible coordinates for new rooms.</returns>
    private HashSet<Vector2I> FindPossibleRoomCoordinates()
    {
        HashSet<Vector2I> coordinates = new();

        foreach (RoomInfo room in _rooms)
        {
            foreach (Vector2I offset in RoomOffsets)
            {
                Vector2I coord = room.Coordinates + offset;

                if (!DoesRoomExist(coord))
                    coordinates.Add(coord);
            }
        }

        return coordinates;
    }

    /// <summary>
    /// Selects new coordinates for placing a new room based on the number of neighboring rooms.
    /// </summary>
    /// <returns>The coordinates for the new room.</returns>
    private Vector2I SelectNewRoomCoordinates()
    {
        HashSet<Vector2I> possibleCoordinates = FindPossibleRoomCoordinates();
        Dictionary<int, List<Vector2I>> coordinatesGroupedByNeighborCount = new();

        for (int i = 1; i <= RoomOffsets.Length; i++)
        {
            coordinatesGroupedByNeighborCount[i] = new();
        }
        
        foreach (Vector2I coord in possibleCoordinates)
        {
            int neighborCount = GetNumberOfNeighborRooms(coord);
            coordinatesGroupedByNeighborCount[neighborCount].Add(coord);
        }

        int group = 1;
        double value = _random.NextDouble();

        if (value <= 0.8)
            group = 1;
        else if (value <= 0.9)
            group = 2;
        else if (value <= 0.95)
            group = 3;
        else
            group = 4;

        List<Vector2I> list = coordinatesGroupedByNeighborCount[group];
        if (list.Count == 0)
        {
            for (int i = 1; i <= RoomOffsets.Length; i++)
            {
                if (coordinatesGroupedByNeighborCount[i].Count > 0)
                {
                    list = coordinatesGroupedByNeighborCount[i];
                    break;
                }
            }
        }
        
        int index = _random.Next(list.Count);
        return list[index];
    }
    
    /// <summary>
    /// Generates a random room and selects its coordinates.
    /// </summary>
    /// <returns>The generated room information.</returns>
    private RoomInfo GenerateRandomRoom()
    {
        Vector2I coord = SelectNewRoomCoordinates();

        return new RoomInfo(RoomType.Regular, coord);
    }

    /// <summary>
    /// Gets the number of neighboring rooms for a given room coordinate.
    /// </summary>
    /// <param name="roomCoord">The room coordinate to check.</param>
    /// <returns>The number of neighboring rooms.</returns>
    private int GetNumberOfNeighborRooms(Vector2I roomCoord)
    {
        int count = 0;

        foreach (Vector2I offset in RoomOffsets)
        {
            Vector2I coord = roomCoord + offset;
            
            if (DoesRoomExist(coord))
                count++;
        }

        return count;
    }
    
    /// <summary>
    /// Assigns special room types (e.g., exit, pillar rooms) to some of the generated rooms.
    /// </summary>
    private void AssignSpecialRooms()
    {
        List<RoomInfo> sortedRooms = _rooms.Where(r => r.RoomType == RoomType.Regular).ToList();
        sortedRooms.Sort((a, b) => 
            GetNumberOfNeighborRooms(a.Coordinates) - GetNumberOfNeighborRooms(b.Coordinates));

        RoomType[] roomTypes = new[]
        {
            RoomType.Exit,
            RoomType.PillarAbstraction, RoomType.PillarPolymorphism,
            RoomType.PillarEncapsulation, RoomType.PillarInheritance
        };

        for (int i = 0; i < Math.Min(roomTypes.Length, sortedRooms.Count); i++)
        {
            RoomType roomType = roomTypes[i];
            RoomInfo room = sortedRooms[i];
            room.SetRoomType(roomType);
        }
    }

    /// <summary>
    /// Generates the actual rooms in the dungeon using the specified room generator.
    /// </summary>
    /// <param name="roomGenerator">The room generator to use.</param>
    /// <returns>A dictionary of generated rooms indexed by their coordinates.</returns>
    private Dictionary<Vector2I, Room> GenerateRooms(RoomGenerator roomGenerator)
    {
        Dictionary<Vector2I, Room> rooms = new();

        foreach (RoomInfo roomInfo in _rooms)
        {
            Room room = roomGenerator.GenerateRoom(roomInfo.Coordinates, roomInfo.RoomType);
            rooms[room.Coordinates] = room;
        }
        return rooms;
    }

    /// <summary>
    /// Prints the grid of rooms in the dungeon for debugging purposes.
    /// </summary>
    public void PrintGrid()
    {
        Vector2I topLeft = new Vector2I(0, 0);
        Vector2I bottomRight = new Vector2I(0, 0);

        foreach (Vector2I coord in _roomMap.Keys)
        {
            topLeft.X = Math.Min(topLeft.X, coord.X);
            topLeft.Y = Math.Min(topLeft.Y, coord.Y);
            bottomRight.X = Math.Max(bottomRight.X, coord.X);
            bottomRight.Y = Math.Max(bottomRight.Y, coord.Y);
        }
            
        for (int i = topLeft.X - 1; i <= bottomRight.X + 1; i++)
        {
            for (int j = topLeft.Y - 1; j <= bottomRight.Y + 1; j++)
            {
                Vector2I coord = new Vector2I(i, j);
               if (!_roomMap.TryGetValue(coord, out RoomInfo room))
                    GD.PrintRaw("-");
                else
                    GD.PrintRaw(RoomTypeToChar(room.RoomType));
            }
            
            GD.Print();
        }
    }

    /// <summary>
    /// Converts a room type to a character representation for debugging.
    /// </summary>
    /// <param name="type">The room type to convert.</param>
    /// <returns>The character representation of the room type.</returns>
    private static char RoomTypeToChar(RoomType type)
    {
        return type switch
        {
            RoomType.Regular =>'*',
            RoomType.Start => 'S',
            RoomType.Exit => 'O',
            RoomType.PillarAbstraction => 'A',
            RoomType.PillarPolymorphism => 'P',
            RoomType.PillarEncapsulation => 'E',
            RoomType.PillarInheritance => 'I'
        };
    }
}