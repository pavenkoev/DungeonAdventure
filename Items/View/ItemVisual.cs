using Godot;

namespace DungeonAdventure.Items.View;

[Tool]
[GlobalClass]
public partial class ItemVisual : Resource
{
    /// <summary>
    /// Gets the icon texture of the item.
    /// </summary>
    [Export] public Texture2D Icon { get; set; }
    
    /// <summary>
    /// Gets the visual representation of the item.
    /// </summary>
    [Export] public PackedScene Visual { get; set; }
}