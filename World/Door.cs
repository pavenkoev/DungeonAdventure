using DungeonAdventure.Characters;
using Godot;

namespace DungeonAdventure.World;

public partial class Door : Node2D
{
	[Export] private DoorDirection _direction;
	[Export] private Area2D _doorTrigger;
	[Export] private Node2D _spawnPosition;

	public Vector2 SpawnPosition => _spawnPosition.GlobalPosition;
	public override void _Ready()
	{
		_doorTrigger.BodyEntered += OnTriggerBodyEntered;
	}

	private void OnTriggerBodyEntered(Node2D body)
	{
		if (body is not Character)
			return;

		Character character = (Character)body;
		
		GD.Print($"{body} entered {_direction} door");

		Room room = FindRoom(this);
		if (room == null)
		{
			GD.PrintErr("No room found");
			return;
		}
		
		// the method needs to be called in the process stage, currently we are in the physics stage
		Callable.From(() => room.GoThroughTheDoor(character, _direction)).CallDeferred();
	}
	
	private static Room FindRoom(Node node)
	{
		if (node == null)
			return null;

		if (node is Room room)
			return room;

		return FindRoom(node.GetParent());
	}

}
