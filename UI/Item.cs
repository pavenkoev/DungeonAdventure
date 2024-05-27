using Godot;

namespace DungeonAdventure.UI;

/// <summary>
/// Represents the UI element for an item in the game.
/// </summary>
public partial class Item : Control
{
    /// <summary>
    /// The texture rect used to display the item's icon.
    /// </summary>
    [Export] private TextureRect _itemIcon;

    /// <summary>
    /// Sets the icon for the item UI element.
    /// </summary>
    /// <param name="icon">The icon texture to display.</param>
    public void SetIcon(Texture2D icon)
    {
        _itemIcon.Texture = icon;
    }
}