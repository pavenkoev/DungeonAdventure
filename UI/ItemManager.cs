using DungeonAdventure.Characters;
using Godot;

namespace DungeonAdventure.UI;

public partial class ItemManager : Control
{
    [Export] private Character _character;

    [Export] private PackedScene _itemUIScene;

    public override void _Ready()
    {
        _character.ItemsChanged += OnItemsChanged;
        OnItemsChanged();
    }

    private void OnItemsChanged()
    {
        foreach (Node node in GetChildren())
        {
            node.QueueFree();
        }

        foreach (Items.Item item in _character.Items)
        {
            Item itemUI = _itemUIScene.Instantiate<Item>();
            itemUI.SetIcon(item.Icon);
            AddChild(itemUI);
        }
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (!@event.IsActionType())
            return;

        for (int i = 1; i <= 9; i++)
        {
            string name = $"use_item_{i}";
            if (Input.IsActionJustPressed(name))
            {
                TryUseItem(i);
                return;
            }
        }
    }

    private void TryUseItem(int index)
    {
        index -= 1;
        if (index >= _character.Items.Count)
            return;

        Items.Item item = _character.Items[index];
        if (_character.CanUseItem(item))
            _character.UseItem(item);
    }
}