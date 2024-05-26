using DungeonAdventure.Characters;
using DungeonAdventure.Characters.Views;
using DungeonAdventure.Utils;
using Godot;

namespace DungeonAdventure.Items;

[Tool]
public partial class ItemObject : Node2D
{
    private Item _item;
    [Export] public Item Item { get => _item; set => SetItem(value); }

    private Node2D _visualContainer;
    private Area2D _pickupArea;

    public override void _Ready()
    {
        _visualContainer = GetNode<Node2D>("%VisualContainer");
        _pickupArea = GetNode<Area2D>("%PickupArea");

        _pickupArea.BodyEntered += OnPickupAreaCollision;
        
        SetItem(_item);
    }
    
    private void SetItem(Item value)
    {
        _item = value;

        if (_visualContainer == null)
            return;

        foreach (Node node in _visualContainer.GetChildren())
        {
            node.QueueFree();
        }

        if (_item != null && _item.Visual != null)
        {
            Node visual = _item.Visual.Instantiate<Node>();
            _visualContainer.AddChild(visual);
        }
    }

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