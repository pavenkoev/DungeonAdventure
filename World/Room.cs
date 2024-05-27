using System;
using DungeonAdventure.Characters;
using DungeonAdventure.Characters.Views;
using DungeonAdventure.Utils;
using Godot;

namespace DungeonAdventure.World;

/// <summary>
/// Represents a room in the dungeon.
/// </summary>
public partial class Room : Node2D, IPausable
{
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
}