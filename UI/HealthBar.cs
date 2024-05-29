using DungeonAdventure.Characters.Views;
using DungeonAdventure.Utils;
using Godot;

namespace DungeonAdventure.UI;

/// <summary>
/// Represents a health bar UI element that updates based on the character's health.
/// </summary>
public partial class HealthBar : Control
{
    /// <summary>
    /// The progress bar representing the health bar.
    /// </summary>
    [Export] private TextureProgressBar _progressBar;

    /// <summary>
    /// Called when the node is added to the scene.
    /// </summary>
    public override void _Ready()
    {
        CallDeferred(nameof(Initialize));
    }

    /// <summary>
    /// Initializes the health bar, setting up the health change event and progress bar values.
    /// </summary>
    private void Initialize()
    {
        CharacterView character = this.FindCharacter();
        character.Model.HealthChanged += OnHealthChanged;

        _progressBar.MinValue = 0;
        _progressBar.MaxValue = character.Model.MaxHealth;
        _progressBar.Value = character.Model.Health;
    }

    /// <summary>
    /// Called when the character's health changes. Updates the progress bar value.
    /// </summary>
    /// <param name="value">The new health value.</param>
    private void OnHealthChanged(float value)
    {
        _progressBar.Value = value;
    }
}

