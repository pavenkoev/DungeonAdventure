using Godot;

namespace DungeonAdventure.Characters.Indicators;

public partial class Indicator : Node2D
{
    [Export] public string Text { get; set; } = "";
    [Export] public Color Color { get; set; } = new Color(1, 1, 1);

    private AnimationPlayer _animationPlayer;
    private Node2D _colorNode;
    private Label _label;

    public override void _Ready()
    {
        _animationPlayer = GetNode<AnimationPlayer>("%AnimationPlayer");
        _colorNode = GetNode<Node2D>("%Color");
        _label = GetNode<Label>("%Label");
        
        _animationPlayer.Play("display");
        
        _label.Text = Text;
        _colorNode.Modulate = Color;
    }

    private void Done()
    {
        QueueFree();
    }
}