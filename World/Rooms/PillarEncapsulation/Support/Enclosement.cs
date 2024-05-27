using Godot;

namespace DungeonAdventure.World.Rooms.PillarEncapsulation.Support;

/// <summary>
/// Represents an enclosement that can be opened to allow passage in the pillar of encapsulation room.
/// </summary>
public partial class Enclosement : Node2D
{
    /// <summary>
    /// The animation player responsible for playing the door animations.
    /// </summary>
    [Export] private AnimationPlayer _animationPlayer;
    
    /// <summary>
    /// The static body that blocks the entrance until the door is opened.
    /// </summary>
    [Export] private StaticBody2D _block;

    /// <summary>
    /// Indicates whether the enclosement is opened.
    /// </summary>
    [Export] private bool _isOpened = false;

    /// <summary>
    /// Called when the node is added to the scene. Initializes the enclosement.
    /// </summary>
    public override void _Ready()
    {
        if (_isOpened)
            Open();
    }

    /// <summary>
    /// Opens the door, playing the open animation and removing the blocking body.
    /// </summary>
    public void Open()
    {
        _isOpened = true;
        _animationPlayer.Play("open");
        _block.QueueFree();
    }
}