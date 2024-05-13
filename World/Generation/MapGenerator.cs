using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

namespace DungeonAdventure.World.Generation;

public class MapGenerator
{
    private Dictionary<Vector2I, Room> _roomMap = new();
    private List<Room> _rooms = new();
    private Room[,] _grid;
    private Random _random = new();
    private static readonly Vector2I[] _roomOffsets = new[]
    {
        new Vector2I(1, 0), new Vector2I(-1, 0),
        new Vector2I(0, 1), new Vector2I(0, -1)
    };

    private const int MinNumberOfRooms = 2 + 4; // start + end rooms + 4 pillar rooms
    
    public Map Generate(int numOfRooms)
    {
        numOfRooms = Math.Max(numOfRooms, MinNumberOfRooms);
        _grid = new Room[numOfRooms, numOfRooms];

        Vector2I centerCoord = new Vector2I(_grid.GetLength(0) / 2, _grid.GetLength(1) / 2);
        Room startingRoom = new Room(RoomType.Start, centerCoord);

        _grid[centerCoord.X, centerCoord.Y] = startingRoom;
        _roomMap[centerCoord] = startingRoom;
        _rooms.Add(startingRoom);

        for (int i = 0; i < numOfRooms - 1; i++)
        {
            Room room = GenerateRandomRoom();
            if (room == null)
            {
                GD.PrintErr("Failed to generate a room");
                continue;
            }
            
            Vector2I coord = room.Coordinates;
            
            _grid[coord.X, coord.Y] = room;
            _roomMap[coord] = room;
            _rooms.Add(room);
        }
        
        AssignSpecialRooms();

        return new Map(_roomMap, startingRoom);
    }

    private bool CheckCoordinates(Vector2I coords)
    {
        return coords.X >= 0 && coords.X < _grid.GetLength(0) &&
               coords.Y >= 0 && coords.Y < _grid.GetLength(1);
    }

    private HashSet<Vector2I> FindPossibleRoomCoordinates()
    {
        HashSet<Vector2I> coordinates = new();

        foreach (Room room in _rooms)
        {
            foreach (Vector2I offset in _roomOffsets)
            {
                Vector2I coord = room.Coordinates + offset;

                if (CheckCoordinates(coord) && _grid[coord.X, coord.Y] == null)
                    coordinates.Add(coord);
            }
        }

        return coordinates;
    }

    private Vector2I SelectNewRoomCoordinates()
    {
        HashSet<Vector2I> possibleCoordinates = FindPossibleRoomCoordinates();
        Dictionary<int, List<Vector2I>> coordinatesGroupedByNeighborCount = new();

        for (int i = 1; i <= _roomOffsets.Length; i++)
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
            for (int i = 1; i <= _roomOffsets.Length; i++)
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
    
    private Room GenerateRandomRoom()
    {
        Vector2I coord = SelectNewRoomCoordinates();

        return new Room(RoomType.Regular, coord);
    }

    private int GetNumberOfNeighborRooms(Vector2I roomCoord)
    {
        int count = 0;

        foreach (Vector2I offset in _roomOffsets)
        {
            Vector2I coord = roomCoord + offset;
            
            if (!CheckCoordinates(coord))
                continue;
            
            if (_grid[coord.X, coord.Y] != null)
                count++;
        }

        return count;
    }
    private void AssignSpecialRooms()
    {
        List<Room> sortedRooms = _rooms.Where(r => r.RoomType == RoomType.Regular).ToList();
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
            Room room = sortedRooms[i];
            room.SetRoomType(roomType);
        }
    }

    public void PrintGrid()
    {
        for (int i = 0; i < _grid.GetLength(0); i++)
        {
            for (int j = 0; j < _grid.GetLength(1); j++)
            {
                Room room = _grid[i, j];
                if (room == null)
                    GD.PrintRaw("-");
                else
                    GD.PrintRaw(RoomTypeToChar(room.RoomType));
            }
            
            GD.Print();
        }
    }

    public static char RoomTypeToChar(RoomType type)
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