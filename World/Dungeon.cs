using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Ardot.SaveSystems;
using DungeonAdventure.Characters;
using DungeonAdventure.Characters.Views;
using DungeonAdventure.UI;
using DungeonAdventure.Utils;
using DungeonAdventure.World.Generation;
using Godot;
using Godot.Collections;

namespace DungeonAdventure.World;

/// <summary>
/// Represents the dungeon in the game, handling room generation, connections, and player movement.
/// </summary>
public partial class Dungeon : Node2D, ISaveable
{
    private bool _doorsEnabled = true;
    private Map _map;

    [Export] private MapGenerationSettings _mapGenerationSettings;

    [Export] private bool _generate = true;

    [Export] private bool _resetDataBase = true;
    
    [Export] private Vector2I _roomDimension = new Vector2I(640, 368);
    [Export] private CharacterView _player;

    [Export] private PackedScene _eastDoorScene;
    [Export] private PackedScene _northDoorScene;
    [Export] private PackedScene _southDoorScene;
    [Export] private PackedScene _westDoorScene;

    [Export] private PackedScene _endGameScreenScene;

    [Export] private CanvasLayer _ui;
    [Export] private DungeonMapDisplay _mapDisplay;

    [Signal]
    public delegate void GameStartedEventHandler();
    
    [Signal]
    public delegate void GameEndedEventHandler();
    
    private const float DoorDisableAfterInteractionTime = 0.2f;

    /// <summary>
    /// Gets a value indicating whether the doors are enabled.
    /// </summary>
    public bool DoorsEnabled => _doorsEnabled;

    public Map Map => _map;
    public CharacterView Player => _player;
    
    /// <summary>
    /// Called when the node is added to the scene. Initializes the dungeon.
    /// </summary>
    public override void _Ready()
    {
        if (_resetDataBase)
        {
            using CharacterData characterData = new();
            characterData.InitializeData();
        }
        
        if (!_generate)
            return;

        if (_player == null)
            _player = this.FindPlayer();
        
        MapGenerator mapGenerator = new();
        _map = mapGenerator.Generate(_mapGenerationSettings);
        mapGenerator.PrintGrid();
        
        InitializeDungeonMap(_map);
        
        Room startingRoom = _map.Rooms[_map.StartingRoom.Coordinates].Node;
        _player.Reparent(startingRoom);
        
        startingRoom.Resume();
        startingRoom.OnPlayerEntered(_player);
        
        _player.Model.CharacterDied += OnPlayerDeath;
        
        EmitSignal(SignalName.GameStarted);
    }

    public override void _Input(InputEvent @event)
    {
        if (Input.IsActionPressed("toggle_map"))
            _mapDisplay.Visible = !_mapDisplay.Visible;
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
    /// Moves the dungeon in the specified room coordinates.
    /// </summary>
    /// <param name="roomCoords">The coordinates to move to.</param>
    private void Move(Vector2I roomCoords)
    {
        Position = -roomCoords * _roomDimension;
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
        
        currentRoom.OnPlayerExited(player);
        
        player.Reparent(nextRoom);
        player.GlobalPosition = exitDoor.SpawnPosition;
        
        nextRoom.OnPlayerEntered(player);
        
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
        System.Collections.Generic.Dictionary<Vector2I, Room> rooms = new();
        
        foreach (Vector2I coordinate in map.Rooms.Keys)
        {
            Generation.Room room = map.Rooms[coordinate];
            
            AddChild(room.Node);
            room.Node.GlobalPosition = coordinate * _roomDimension;

            rooms[coordinate] = room.Node;
            
            room.Node.Pause();
        }

        if (_player == null)
            _player = this.FindPlayer();
        
        InitializeRoomConnections(rooms);
    }
    
    /// <summary>
    /// Initializes the connections between rooms.
    /// </summary>
    /// <param name="rooms">The dictionary of rooms to connect.</param>
    private void InitializeRoomConnections(System.Collections.Generic.Dictionary<Vector2I, Room> rooms)
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

    /// <summary>
    /// Saves the dungeon data into a SaveData object.
    /// </summary>
    /// <param name="parameters">Additional parameters for saving (not used).</param>
    /// <returns>A SaveData object containing the dungeon data.</returns>
    public SaveData Save(params Variant[] parameters)
    {
        Array<SaveData> rooms = new();
        Room.SaveLoad roomSaveLoad = new();
        
        foreach (Room room in this.FindNodesDown<Room>())
        {
            roomSaveLoad.Room = room;
            rooms.Add(roomSaveLoad.Save());
        }

        CharacterView.SaveLoad characterSaveLoad = new();
        characterSaveLoad.Character = _player;

        roomSaveLoad.Room = _map.StartingRoom.Node;
        
        return new(GetLoadKey(), rooms, characterSaveLoad.GetLoadKey(), roomSaveLoad.GetLoadKey());
    }

    /// <summary>
    /// Loads the dungeon data from a SaveData object.
    /// </summary>
    /// <param name="data">The SaveData object containing the dungeon data.</param>
    /// <param name="parameters">Additional parameters for loading (not used).</param>
    public void Load(SaveData data, params Variant[] parameters)
    {
        List<Room> roomNodes = this.FindNodesDown<Room>().ToList();
        
        foreach (Room room in roomNodes)
        {
            room.GetParent().RemoveChild(room);
            room.QueueFree();
        }

        _player = null;

        Array<SaveData> roomDatas = data[0].AsGodotArray<SaveData>();
        string playerNodeName = data[1].AsString();
        string startingRoomNodeName = data[2].AsString();

        System.Collections.Generic.Dictionary<Vector2I, Generation.Room> rooms = new();
        
        foreach (SaveData roomSaveData in roomDatas)
        {
            Room.SaveLoad saveLoad = new();
            saveLoad.Load(roomSaveData);

            Generation.Room room = new Generation.Room(RoomType.Regular, saveLoad.Room.Coordinates, saveLoad.Room);
            rooms[room.Coordinates] = room;
        }
        
        Generation.Room startingRoom = rooms[new Vector2I(0, 0)];

        _map = new Map(rooms, startingRoom);
        InitializeDungeonMap(_map);

        Room playerRoom = _player.FindRoom();
        Move(playerRoom.Coordinates);
        
        playerRoom.Resume();
        playerRoom.OnPlayerEntered(_player);

        _player.Model.CharacterDied += OnPlayerDeath;
        
        EmitSignal(SignalName.GameStarted);
    }

    /// <summary>
    /// Gets the load key for the dungeon.
    /// </summary>
    /// <param name="parameters">Additional parameters for the load key (not used).</param>
    /// <returns>The load key for the dungeon.</returns>
    public StringName GetLoadKey(params Variant[] parameters)
    {
        return "Dungeon";
    }

    /// <summary>
    /// Handles the player's death event.
    /// Displays the end game screen indicating a loss.
    /// </summary>
    private void OnPlayerDeath()
    {
        _player = null;
        
        EndGameScreen screen = _endGameScreenScene.Instantiate<EndGameScreen>();
        screen.Won = false;
        _ui.AddChild(screen);
    }

    /// <summary>
    /// Handles the event when the player finishes the game successfully.
    /// Displays the end game screen indicating a win.
    /// </summary>
    public void OnPlayerFinishedGame()
    {
        _player.QueueFree();
        _player = null;
        
        EndGameScreen screen = _endGameScreenScene.Instantiate<EndGameScreen>();
        screen.Won = true;
        _ui.AddChild(screen);
    }
}