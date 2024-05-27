using Godot;

namespace DungeonAdventure.World.Rooms.Exit.Support;

/// <summary>
/// Represents an exit door that can be opened to allow passage.
/// </summary>
public partial class ExitDoor : Node2D
{
    /// <summary>
    /// The animation player responsible for playing the door animations.
    /// </summary>
    [Export] private AnimationPlayer _animationPlayer;
    
    /// <summary>
    /// The static body that blocks the exit until the door is opened.
    /// </summary>
    [Export] private StaticBody2D _block;

    /// <summary>
    /// Opens the door, playing the open animation and removing the blocking body.
    /// </summary>
    public void Open()
    {
        _animationPlayer.Play("open");
        _block.QueueFree();
    }
}