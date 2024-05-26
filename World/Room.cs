using System;
using DungeonAdventure.Characters;
using DungeonAdventure.Characters.Views;
using DungeonAdventure.Utils;
using Godot;

namespace DungeonAdventure.World;

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
        
        // DoorDirection exitDirection = GetOppositeDoorDirection(direction);
        // Door exitDoor = nextRoom.GetDoorForDirection(exitDirection);
        //
        // player.Reparent(nextRoom);
        // player.GlobalPosition = exitDoor.SpawnPosition;
        //
        // dungeon.Move(direction);
    }

    public Room GetRoomForDirection(DoorDirection direction)
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

    public void Pause()
    {
        foreach (IPausable obj in this.FindNodesDown<IPausable>(false))
        {
            obj.Pause();
        }
    }

    public void Resume()
    {
        foreach (IPausable obj in this.FindNodesDown<IPausable>(false))
        {
            obj.Resume();
        }
    }
}