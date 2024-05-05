using System;
using DungeonAdventure.Characters;
using Godot;

namespace DungeonAdventure.World;

public partial class Room : Node2D
{
	[Export] private Room _northRoom;
	[Export] private Room _eastRoom;
	[Export] private Room _southRoom;
	[Export] private Room _westRoom;

	[Export] private Door _northDoor;
	[Export] private Door _eastDoor;
	[Export] private Door _southDoor;
	[Export] private Door _westDoor;
	

	public void GoThroughTheDoor(Character player, DoorDirection direction)
	{
		Room nextRoom = GetRoomForDirection(direction);
		if (nextRoom == null)
		{
			GD.PrintErr($"No room for direction {direction}");
			return;
		}

		DoorDirection exitDirection = GetOppositeDoorDirection(direction);
		Door exitDoor = nextRoom.GetDoorForDirection(exitDirection);
	
		player.Reparent(nextRoom);
		player.Position = exitDoor.SpawnPosition;

		Dungeon dungeon = FindDungeon(this);
		dungeon.Move(direction);
		dungeon.GenerateCorridor(_doorTrigger.GlobalPosition, exitDoor._doorTrigger.GlobalPosition);
	}

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
	
	private Door GetDoorForDirection(DoorDirection direction)
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

	private DoorDirection GetOppositeDoorDirection(DoorDirection direction)
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

	private static Dungeon FindDungeon(Node node)
	{
		if (node == null)
			return null;

		if (node is Dungeon dungeon)
			return dungeon;

		return FindDungeon(node.GetParent());
	}
}
