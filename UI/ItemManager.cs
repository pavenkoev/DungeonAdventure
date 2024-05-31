using DungeonAdventure.Characters.Views;
using DungeonAdventure.Items.View;
using DungeonAdventure.Utils;
using DungeonAdventure.World;
using Godot;

namespace DungeonAdventure.UI;

/// <summary>
/// Manages the UI for displaying and using items held by the player character.
/// </summary>
public partial class ItemManager : Control
{
    private CharacterView _character;

    /// <summary>
    /// The scene used to instantiate item UI elements.
    /// </summary>
    [Export] private PackedScene _itemUIScene;

    /// <summary>
    /// Called when the node is added to the scene tree. Initializes the item manager.
    /// </summary>
    public override void _EnterTree()
    {
        GetTree().Root.FindNodeDown<Dungeon>().GameStarted += OnGameStarted;
    }

    /// <summary>
    /// Handles the initialization when the game starts.
    /// </summary>
    private void OnGameStarted()
    {
        _character = this.FindPlayer();
        
        _character.Model.ItemsChanged += OnItemsChanged;
        OnItemsChanged();
    }

    /// <summary>
    /// Updates the UI when the items held by the character change.
    /// </summary>
    private void OnItemsChanged()
    {
        foreach (Node node in GetChildren())
        {
            node.QueueFree();
        }

        foreach (Items.Item item in _character.Model.Items)
        {
            Item itemUI = _itemUIScene.Instantiate<Item>();
            ItemVisual visual = item.LoadVisual();
            
            itemUI.SetIcon(visual?.Icon);
            AddChild(itemUI);
        }
    }

    /// <summary>
    /// Handles input events to allow the player to use items.
    /// </summary>
    /// <param name="event">The input event to handle.</param>
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

    /// <summary>
    /// Attempts to use the item at the specified index in the character's inventory.
    /// </summary>
    /// <param name="index">The index of the item to use.</param>
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