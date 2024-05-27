using Godot;

namespace DungeonAdventure.World.Rooms.PillarEncapsulation.Support;

/// <summary>
/// Represents a button in the pillar of encapsulation room that can be pressed to trigger an event.
/// </summary>
public partial class Button : Node2D
{
    /// <summary>
    /// The node representing the button's off state.
    /// </summary>
    [Export] private Node2D _off;
    
    /// <summary>
    /// The node representing the button's on state.
    /// </summary>
    [Export] private Node2D _on;
    
    /// <summary>
    /// The area that detects when the button is pressed.
    /// </summary>
    [Export] private Area2D _pressArea;
    
    /// <summary>
    /// Signal emitted when the button is pressed.
    /// </summary>
    [Signal]
    public delegate void PressedEventHandler();
    
    /// <summary>
    /// Called when the node is added to the scene. Initializes the button.
    /// </summary>
    public override void _Ready()
    {
        _pressArea.BodyEntered += OnPress;
    }

    /// <summary>
    /// Handles the press event when a body enters the press area.
    /// </summary>
    /// <param name="body">The body that pressed the button.</param>
    private void OnPress(Node2D body)
    {
        if (_on.Visible)
            return;
        
        _off.Visible = false;
        _on.Visible = true;

        EmitSignal(SignalName.Pressed);
    }
}