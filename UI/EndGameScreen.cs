using Godot;

namespace DungeonAdventure.UI;

/// <summary>
/// Represents the end game screen displayed to the player upon winning or losing the game.
/// </summary>
public partial class EndGameScreen : Control
{
    [Export] private Label _titleLabel;
    [Export] private PackedScene _menuScene;
    [Export] public bool Won { get; set; } = true;

    private const string WonText = "GAME WON";
    private const string LostText = "GAME LOST";
    
    /// <summary>
    /// Called when the node is added to the scene.
    /// Sets the title label text based on whether the player won or lost the game.
    /// </summary>
    public override void _Ready()
    {
        _titleLabel.Text = Won ? WonText : LostText;
    }

    /// <summary>
    /// Changes the scene to the main menu.
    /// </summary>
    private void BackToMenu()
    {
        QueueFree();
        GetTree().ChangeSceneToPacked(_menuScene);
    }
}