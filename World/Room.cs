using System;
using Ardot.SaveSystems;
using DungeonAdventure.Characters;
using DungeonAdventure.Characters.Views;
using DungeonAdventure.Items.View;
using DungeonAdventure.Utils;
using DungeonAdventure.World.Placeholders;
using Godot;
using Godot.Collections;
using Array = Godot.Collections.Array;

namespace DungeonAdventure.World;

/// <summary>
/// Represents a room in the dungeon.
/// </summary>
public partial class Room : Node2D, IPausable
{
    [Export] public Vector2I Coordinates { get; set; }
    
    [Export] private Room _northRoom;
    [Export] private Room _eastRoom;
    [Export] private Room _southRoom;
    [Export] private Room _westRoom;

    [Export] private Door _northDoor;
    [Export] private Door _eastDoor;
    [Export] private Door _southDoor;
    [Export] private Door _westDoor;

    /// <summary>
    /// Connects a room and door to this room in the specified direction.
    /// </summary>
    /// <param name="direction">The direction to connect the room and door.</param>
    /// <param name="door">The door to connect.</param>
    /// <param name="room">The room to connect.</param>
    public void ConnectRoom(DoorDirection direction, Door door, Room room)
    {
        switch (direction)
        {
            case DoorDirection.East:
                _eastDoor = door;
                _eastRoom = room;
                break;
            case DoorDirection.North:
                _northDoor = door;
                _northRoom = room;
                break;
            case DoorDirection.South:
                _southDoor = door;
                _southRoom = room;
                break;
            case DoorDirection.West:
                _westDoor = door;
                _westRoom = room;
                break;
        }
    }
    
    /// <summary>
    /// Allows the player to go through the door in the specified direction.
    /// </summary>
    /// <param name="player">The player character.</param>
    /// <param name="direction">The direction to move through the door.</param>
    public void GoThroughTheDoor(CharacterView player, DoorDirection direction)
    {
        Dungeon dungeon = this.FindDungeon();
        if (!dungeon.DoorsEnabled)
            return;
        
        Room nextRoom = GetRoomForDirection(direction);
        if (nextRoom == null)
        {
            GD.PrintErr($"No room for direction {direction}");
            return;
        }

        dungeon.ChangeRoom(player, this, nextRoom, direction);
    }

    /// <summary>
    /// Gets the connected room for the specified direction.
    /// </summary>
    /// <param name="direction">The direction to get the room.</param>
    /// <returns>The connected room, or null if no room is connected.</returns>
    private Room GetRoomForDirection(DoorDirection direction)
    {
        return direction switch
        {
            DoorDirection.North => _northRoom,
            DoorDirection.East => _eastRoom,
            DoorDirection.South => _southRoom,
            DoorDirection.West => _westRoom,
            _ => null
        };
    }
    
    /// <summary>
    /// Gets the door for the specified direction.
    /// </summary>
    /// <param name="direction">The direction to get the door.</param>
    /// <returns>The door in the specified direction, or null if no door exists.</returns>
    public Door GetDoorForDirection(DoorDirection direction)
    {
        return direction switch
        {
            DoorDirection.North => _northDoor,
            DoorDirection.East => _eastDoor,
            DoorDirection.South => _southDoor,
            DoorDirection.West => _westDoor,
            _ => null
        };
    }

    /// <summary>
    /// Gets the opposite direction for the specified door direction.
    /// </summary>
    /// <param name="direction">The direction to get the opposite for.</param>
    /// <returns>The opposite door direction.</returns>
    public static DoorDirection GetOppositeDoorDirection(DoorDirection direction)
    {
        return direction switch
        {
            DoorDirection.North => DoorDirection.South,
            DoorDirection.East => DoorDirection.West,
            DoorDirection.South => DoorDirection.North,
            DoorDirection.West => DoorDirection.East,
            _ => throw new ArgumentException("Invalid DoorDirection")
        };
    }

    /// <summary>
    /// Called when the player enters the room.
    /// </summary>
    /// <param name="player">The player character.</param>
    public virtual void OnPlayerEntered(CharacterView player) {}
    
    /// <summary>
    /// Called when the player exits the room.
    /// </summary>
    /// <param name="player">The player character.</param>
    public virtual void OnPlayerExited(CharacterView player) {}

    /// <summary>
    /// Pauses all pausable objects in the room.
    /// </summary>
    public virtual void Pause()
    {
        foreach (IPausable obj in this.FindNodesDown<IPausable>(false))
        {
            obj.Pause();
        }
    }

    /// <summary>
    /// Resumes all pausable objects in the room.
    /// </summary>
    public virtual void Resume()
    {
        foreach (IPausable obj in this.FindNodesDown<IPausable>(false))
        {
            obj.Resume();
        }
    }

    /// <summary>
    /// Clears all placeholders from the current room.
    /// </summary>
    private void ClearPlaceholders()
    {
        foreach (Placeholder placeholder in this.FindNodesDown<Placeholder>())
        {
            placeholder.QueueFree();
        }
    }

    /// <summary>
    /// Class for saving and loading room data.
    /// </summary>
    public class SaveLoad : ISaveable
    {
        /// <summary>
        /// Gets or sets the room to be saved or loaded.
        /// </summary>
        public Room Room { get; set; }
        
        /// <summary>
        /// Saves the room data into a SaveData object.
        /// </summary>
        /// <param name="parameters">Additional parameters for saving (not used).</param>
        /// <returns>A SaveData object containing the room's data.</returns>
        public SaveData Save(params Variant[] parameters)
        {
            Array<SaveData> characters = new();
            Array<SaveData> items = new();

            CharacterView.SaveLoad characterSaveLoad = new();
            ItemView.SaveLoad itemSaveLoad = new();
            
            foreach (CharacterView character in Room.FindNodesDown<CharacterView>())
            {
                if(!character.IsAlive)
                    continue;
                
                characterSaveLoad.Character = character;
                characters.Add(characterSaveLoad.Save());
            }

            foreach (ItemView item in Room.FindNodesDown<ItemView>())
            {
                itemSaveLoad.Item = item;
                items.Add(itemSaveLoad.Save());
            }

            Dictionary<string, Variant> data = new();
            data["scene"] = Room.SceneFilePath;
            data["coordinates"] = Room.Coordinates;
            data["characters"] = characters;
            data["items"] = items;
        
            return new(GetLoadKey(), data);
        }

        /// <summary>
        /// Loads the room data from a SaveData object.
        /// </summary>
        /// <param name="data">The SaveData object containing the room's data.</param>
        /// <param name="parameters">Additional parameters for loading (not used).</param>
        public void Load(SaveData data, params Variant[] parameters)
        {
            Dictionary<string, Variant> p = data[0].AsGodotDictionary<string, Variant>();
            string scenePath = p["scene"].AsString();
            Vector2I coordinates = p["coordinates"].AsVector2I();
            Array<SaveData> characters = p["characters"].AsGodotArray<SaveData>();
            Array<SaveData> items = p["items"].AsGodotArray<SaveData>();

            PackedScene roomScene = GD.Load<PackedScene>(scenePath);
            Room = roomScene.Instantiate<Room>();
            Room.Coordinates = coordinates;
            Room.ClearPlaceholders();

            CharacterView.SaveLoad characterSaveLoad = new();
            ItemView.SaveLoad itemSaveLoad = new();

            foreach (SaveData characterSaveData in characters)
            {
                characterSaveLoad.Load(characterSaveData);
                if (characterSaveLoad.Character != null)
                {
                    Room.AddChild(characterSaveLoad.Character);
                }
            }
            
            foreach (SaveData itemSaveData in items)
            {
                itemSaveLoad.Load(itemSaveData);
                if (itemSaveLoad.Item != null)
                {
                    Room.AddChild(itemSaveLoad.Item);
                }
            }
        }

        /// <summary>
        /// Gets the load key for the room.
        /// </summary>
        /// <param name="parameters">Additional parameters for the load key (not used).</param>
        /// <returns>The load key for the room.</returns>
        public StringName GetLoadKey(params Variant[] parameters) => $"Room_{Room.NativeInstance}";
    }
}