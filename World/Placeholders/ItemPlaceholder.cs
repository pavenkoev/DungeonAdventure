using Godot;

namespace DungeonAdventure.World.Placeholders;

public partial class ItemPlaceholder : Placeholder
{
    [Export] public ItemPlaceholderItemPool ItemPool { get; private set; }
    [Export] public float Probability = 0.5f;

}