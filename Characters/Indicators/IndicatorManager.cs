using Godot;

namespace DungeonAdventure.Characters.Indicators;

/// <summary>
/// Manages the indicators displayed for character actions and events.
/// </summary>
public partial class IndicatorManager : Node2D
{
    /// <summary>
    /// The packed scene used to instantiate new indicators.
    /// </summary>
    [Export] private PackedScene _indicatorScene;

    /// <summary>
    /// Adds a new indicator with the specified text and color.
    /// </summary>
    /// <param name="text">The text to display in the indicator.</param>
    /// <param name="color">The color of the indicator text.</param>
    public void AddIndicator(string text, Color color)
    {
        Indicator indicator = _indicatorScene.Instantiate<Indicator>();
        indicator.Text = text;
        indicator.Color = color;
        AddChild(indicator);
    }
}