using DungeonAdventure.Characters;
using DungeonAdventure.Characters.Views;
using DungeonAdventure.Utils;
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
		if (body is not CharacterView)
			return;

		CharacterView character = (CharacterView)body;
		
		GD.Print($"{body} entered {_direction} door");

		Room room = this.FindRoom();
		if (room == null)
		{
			GD.PrintErr("No room found");
			return;
		}
		
		// the method needs to be called in the process stage, currently we are in the physics stage
		Callable.From(() => room.GoThroughTheDoor(character, _direction)).CallDeferred();
	}
}
