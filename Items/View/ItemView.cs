using DungeonAdventure.Characters.Views;
using DungeonAdventure.Utils;
using Godot;

namespace DungeonAdventure.Items.View;

/// <summary>
/// Represents an item object in the game world that can be picked up by characters.
/// </summary>
[Tool]
public partial class ItemView : Node2D
{
    /// <summary>
    /// The item associated with this item object.
    /// </summary>
    private Item _item;

    /// <summary>
    /// The item visual associated with this item object.
    /// </summary>
    private ItemVisual _visual;
    
    /// <summary>
    /// Gets or sets the item associated with this item object.
    /// </summary>
    [Export] public Item Item { get => _item; set => SetItem(value); }
    
    /// <summary>
    /// Gets or sets the item visual associated with this item object.
    /// </summary>
    [Export] public ItemVisual Visual { get => _visual; set => SetItemVisual(value); }

    private Node2D _visualContainer;
    private Area2D _pickupArea;

    /// <summary>
    /// Called when the node is added to the scene. Initializes the item object.
    /// </summary>
    public override void _Ready()
    {
        _visualContainer = GetNode<Node2D>("%VisualContainer");
        _pickupArea = GetNode<Area2D>("%PickupArea");

        _pickupArea.BodyEntered += OnPickupAreaCollision;
        
        SetItemVisual(_visual);
    }
    
    /// <summary>
    /// Sets the item and updates the visual representation.
    /// </summary>
    /// <param name="value">The item to set.</param>
    private void SetItem(Item value)
    {
        _item = value;
        Visual = value.LoadVisual();
    }
    
    /// <summary>
    /// Sets the item visual representation.
    /// </summary>
    /// <param name="visual">The item visual to set.</param>
    private void SetItemVisual(ItemVisual visual)
    {
        _visual = visual;
        
        if (_visualContainer == null)
            return;

        foreach (Node node in _visualContainer.GetChildren())
        {
            node.QueueFree();
        }

        if (_item != null && _visual != null)
        {
            Node node = _visual.Visual.Instantiate<Node>();
            _visualContainer.AddChild(node);
        }
    }

    /// <summary>
    /// Called when a character enters the pickup area. Attempts to pick up the item.
    /// </summary>
    /// <param name="body">The body that entered the pickup area.</param>
    private void OnPickupAreaCollision(Node2D body)
    {
        CharacterView character = body.FindCharacter();
        if (character == null)
            return;

        if (character.CanPickupItem(_item))
        {
            character.PickupItem(_item);
            QueueFree();
        }
    }
}