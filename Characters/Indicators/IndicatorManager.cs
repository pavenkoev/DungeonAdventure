using Godot;

namespace DungeonAdventure.Characters.Indicators;

public partial class IndicatorManager : Node2D
{
    [Export] private PackedScene _indicatorScene;

    public void AddIndicator(string text, Color color)
    {
        Indicator indicator = _indicatorScene.Instantiate<Indicator>();
        indicator.Text = text;
        indicator.Color = color;
        AddChild(indicator);
    }
}