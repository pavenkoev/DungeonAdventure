using DungeonAdventure.Characters.Views;
using DungeonAdventure.Utils;
using Godot;

namespace DungeonAdventure.UI;

public partial class ItemManager : Control
{
    private CharacterView _character;

    [Export] private PackedScene _itemUIScene;

    public override void _Ready()
    {
        _character = this.FindPlayer();
        
        _character.Model.ItemsChanged += OnItemsChanged;
        OnItemsChanged();
    }

    private void OnItemsChanged()
    {
        foreach (Node node in GetChildren())
        {
            node.QueueFree();
        }

        foreach (Items.Item item in _character.Model.Items)
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
        if (index >= _character.Model.Items.Count)
            return;

        Items.Item item = _character.Model.Items[index];
        if (_character.CanUseItem(item))
            _character.UseItem(item);
    }
}