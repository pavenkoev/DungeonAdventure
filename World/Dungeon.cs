using System.Collections.Generic;
using System.Numerics;
using DungeonAdventure.Characters;
using DungeonAdventure.Characters.Views;
using DungeonAdventure.Utils;
using DungeonAdventure.World.Generation;
using Godot;

namespace DungeonAdventure.World;

/// <summary>
/// Represents the dungeon in the game, handling room generation, connections, and player movement.
/// </summary>
public partial class Dungeon : Node2D
{
    private bool _doorsEnabled = true;

    [Export] private MapGenerationSettings _mapGenerationSettings;

    [Export] private bool _generate = true;
    
    [Export] private Vector2I _roomDimension = new Vector2I(640, 368);
    [Export] private CharacterView _player;

    [Export] private PackedScene _eastDoorScene;
    [Export] private PackedScene _northDoorScene;
    [Export] private PackedScene _southDoorScene;
    [Export] private PackedScene _westDoorScene;

    private const float DoorDisableAfterInteractionTime = 0.2f;

    /// <summary>
    /// Gets a value indicating whether the doors are enabled.
    /// </summary>
    public bool DoorsEnabled => _doorsEnabled;
    
    /// <summary>
    /// Called when the node is added to the scene. Initializes the dungeon.
    /// </summary>
    public override void _Ready()
    {
        if (!_generate)
            return;

        if (_player == null)
            _player = this.FindPlayer();
        
        MapGenerator mapGenerator = new();
        Map map = mapGenerator.Generate(_mapGenerationSettings);
        mapGenerator.PrintGrid();
        
        InitializeDungeonMap(map);
    }

    /// <summary>
    /// Moves the dungeon in the specified direction.
    /// </summary>
    /// <param name="direction">The direction to move.</param>
    private void Move(DoorDirection direction)
    { 
        Vector2I offset = GetDirectionToOffset(direction); 
        Translate(offset * _roomDimension);
        
        TemporaryDisableDoors();
    }

    /// <summary>
    /// Changes the room for the player, moving them from the current room to the next room.
    /// </summary>
    /// <param name="player">The player character.</param>
    /// <param name="currentRoom">The current room.</param>
    /// <param name="nextRoom">The next room to move to.</param>
    /// <param name="direction">The direction to move through.</param>
    public void ChangeRoom(CharacterView player, Room currentRoom, Room nextRoom, DoorDirection direction)
    {
        DoorDirection exitDirection = Room.GetOppositeDoorDirection(direction);
        Door exitDoor = nextRoom.GetDoorForDirection(exitDirection);
        
        player.Reparent(nextRoom);
        player.GlobalPosition = exitDoor.SpawnPosition;
        
        Move(direction);
        
        currentRoom.Pause();
        nextRoom.Resume();
    }

    /// <summary>
    /// Temporarily disables the doors for a short duration.
    /// </summary>
    private void TemporaryDisableDoors()
    {
        _doorsEnabled = false;
        GetTree().CreateTimer(DoorDisableAfterInteractionTime).Timeout += () => _doorsEnabled = true;
    }

    /// <summary>
    /// Gets the offset for the specified direction.
    /// </summary>
    /// <param name="direction">The direction to get the offset for.</param>
    /// <returns>The offset for the specified direction.</returns>
    private Vector2I GetDirectionToOffset(DoorDirection direction)
    {
        return direction switch
        {
            DoorDirection.North => new Vector2I(0, 1),
            DoorDirection.East => new Vector2I(-1, 0),
            DoorDirection.South => new Vector2I(0, -1),
            DoorDirection.West => new Vector2I(1, 0)
        };
    }

    /// <summary>
    /// Initializes the dungeon map with the specified map data.
    /// </summary>
    /// <param name="map">The map data to initialize the dungeon with.</param>
    private void InitializeDungeonMap(Map map)
    {
        Dictionary<Vector2I, Room> rooms = new();
        
        foreach (Vector2I coordinate in map.Rooms.Keys)
        {
            Generation.Room room = map.Rooms[coordinate];
            
            AddChild(room.Node);
            room.Node.GlobalPosition = coordinate * _roomDimension;

            rooms[coordinate] = room.Node;
            
            room.Node.Pause();
        }
        
        InitializeRoomConnections(rooms);

        Room startingRoom = rooms[map.StartingRoom.Coordinates];
        _player.Reparent(startingRoom);
        
        startingRoom.Resume();
    }
    
    /// <summary>
    /// Initializes the connections between rooms.
    /// </summary>
    /// <param name="rooms">The dictionary of rooms to connect.</param>
    private void InitializeRoomConnections(Dictionary<Vector2I, Room> rooms)
    {
        foreach (Vector2I coordinate in rooms.Keys)
        {
            Room room = rooms[coordinate];

            Room eastRoom = rooms.GetValueOrDefault(coordinate - GetDirectionToOffset(DoorDirection.East));
            Room northRoom = rooms.GetValueOrDefault(coordinate - GetDirectionToOffset(DoorDirection.North));
            Room southRoom = rooms.GetValueOrDefault(coordinate - GetDirectionToOffset(DoorDirection.South));
            Room westRoom = rooms.GetValueOrDefault(coordinate - GetDirectionToOffset(DoorDirection.West));

            if (eastRoom != null)
            {
                Door door = _eastDoorScene.Instantiate<Door>();
                room.AddChild(door);
                room.ConnectRoom(DoorDirection.East, door, eastRoom);
            }
            if (northRoom != null)
            {
                Door door = _northDoorScene.Instantiate<Door>();
                room.AddChild(door);
                room.ConnectRoom(DoorDirection.North, door, northRoom);
            }
            if (southRoom != null)
            {
                Door door = _southDoorScene.Instantiate<Door>();
                room.AddChild(door);
                room.ConnectRoom(DoorDirection.South, door, southRoom);
            }
            if (westRoom != null)
            {
                Door door = _westDoorScene.Instantiate<Door>();
                room.AddChild(door);
                room.ConnectRoom(DoorDirection.West, door, westRoom);
            }
        }
    }
}