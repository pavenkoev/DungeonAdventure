using DungeonAdventure.Characters;
using DungeonAdventure.Characters.Views;
using DungeonAdventure.Utils;
using Godot;

namespace DungeonAdventure.World.Rooms.Exit.Support;

/// <summary>
/// Represents the exit in an exit room, handling the opening of doors and the end game condition.
/// </summary>
public partial class Exit : Node2D
{
    /// <summary>
    /// The doors that need to be opened to access the exit.
    /// </summary>
    [Export] private ExitDoor[] _doors;
    
    /// <summary>
    /// The area that detects when a character has entered the exit.
    /// </summary>
    [Export] private Area2D _exitArea;

    private int _openDoorsCount = 0;

    /// <summary>
    /// Called when the node is added to the scene. Initializes the exit area.
    /// </summary>
    public override void _Ready()
    {
        _exitArea.BodyEntered += OnExitAreaBodyEntered;
    }
    
    /// <summary>
    /// Opens the next door in the sequence.
    /// </summary>
    public void OpenNextDoor()
    {
        if (_openDoorsCount >= _doors.Length)
            return;
        
        _doors[_openDoorsCount].Open();
        _openDoorsCount++;
    }

    /// <summary>
    /// Called when a body enters the exit area.
    /// </summary>
    /// <param name="body">The body that entered the exit area.</param>
    private void OnExitAreaBodyEntered(Node2D body)
    {
        CharacterView character = body.FindCharacter();
        if (character == null)
            return;
        
        GD.Print("GAME WON!!!");
        GetTree().Quit();
    }
}