using Godot;

namespace DungeonAdventure.UI;

public partial class Item : Control
{
    [Export] private TextureRect _itemIcon;

    public void SetIcon(Texture2D icon)
    {
        _itemIcon.Texture = icon;
    }

}