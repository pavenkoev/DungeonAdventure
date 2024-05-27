using Godot;

namespace DungeonAdventure.Characters.Indicators;

/// <summary>
/// Represents an indicator for displaying text with a specific color in the game.
/// </summary>
public partial class Indicator : Node2D
{
    /// <summary>
    /// Gets or sets the text to be displayed by the indicator.
    /// </summary>
    [Export] public string Text { get; set; } = "";
    
    /// <summary>
    /// Gets or sets the color of the indicator text.
    /// </summary>
    [Export] public Color Color { get; set; } = new Color(1, 1, 1);

    private AnimationPlayer _animationPlayer;
    private Node2D _colorNode;
    private Label _label;

    /// <summary>
    /// Called when the node is added to the scene. Initializes the indicator.
    /// </summary>
    public override void _Ready()
    {
        _animationPlayer = GetNode<AnimationPlayer>("%AnimationPlayer");
        _colorNode = GetNode<Node2D>("%Color");
        _label = GetNode<Label>("%Label");
        
        _animationPlayer.Play("display");
        
        _label.Text = Text;
        _colorNode.Modulate = Color;
    }

    /// <summary>
    /// Called when the animation is done playing. Frees the indicator from memory.
    /// </summary>
    private void Done()
    {
        QueueFree();
    }
}