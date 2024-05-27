using DungeonAdventure.Characters;
using DungeonAdventure.Characters.Views;
using DungeonAdventure.Utils;
using Godot;

namespace DungeonAdventure.World;

/// <summary>
/// Represents a door in the dungeon.
/// </summary>
public partial class Door : Node2D
{
	/// <summary>
	/// The direction the door is facing.
	/// </summary>
	[Export] private DoorDirection _direction;
	
	/// <summary>
	/// The area trigger for detecting when a character enters the door.
	/// </summary>
	[Export] private Area2D _doorTrigger;
	
	/// <summary>
	/// The position where the character spawns when entering the door.
	/// </summary>
	[Export] private Node2D _spawnPosition;

	/// <summary>
	/// Gets the global spawn position for the door.
	/// </summary>
	public Vector2 SpawnPosition => _spawnPosition.GlobalPosition;
	
	/// <summary>
	/// Called when the node is added to the scene. Initializes the door.
	/// </summary>
	public override void _Ready()
	{
		_doorTrigger.BodyEntered += OnTriggerBodyEntered;
	}

	/// <summary>
	/// Called when a body enters the door trigger area.
	/// </summary>
	/// <param name="body">The body that entered the trigger area.</param>
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
