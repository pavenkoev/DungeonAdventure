using DungeonAdventure.Characters.Controllers;
using DungeonAdventure.Characters.Effects;
using DungeonAdventure.Characters.Views;
using DungeonAdventure.Items;
using DungeonAdventure.Items.View;
using Godot;
using ItemView = DungeonAdventure.Items.View.ItemView;

namespace DungeonAdventure.World.Rooms.PillarInheritance.Support;

/// <summary>
/// Represents a room of the pillar of inheritance.
/// </summary>
public partial class InheritancePillarRoom : Room
{
    private InheritanceEffect _effect;
    private CharacterController _originalController;
    private ItemView _bodyItem;
    private ItemView _weaponItem;

    [Export] private Node2D _bodyItemPosition;
    [Export] private Node2D _weaponItemPosition;
    
    /// <summary>
    /// The scene for the item object.
    /// </summary>
    [Export] private PackedScene _itemObjectScene { get; set; }
    
    /// <summary>
    /// Called when the player enters the room. Sets up the inheritance effect and items.
    /// </summary>
    /// <param name="player">The player character.</param>
    public override void OnPlayerEntered(CharacterView player)
    {
        _bodyItem = _itemObjectScene.Instantiate<ItemView>();
        _bodyItem.Item = new CharacterBodyItem();
        _bodyItem.Visual = MakeBodyItemVisual(player.Model.VisualName);
        AddChild(_bodyItem);
        _bodyItem.Position = _bodyItemPosition.Position;
        
        _weaponItem = _itemObjectScene.Instantiate<ItemView>();
        _weaponItem.Item = new CharacterWeaponItem();
        _weaponItem.Visual = MakeWeaponItemVisual(player.Model.WeaponName);
        AddChild(_weaponItem);
        _weaponItem.Position = _weaponItemPosition.Position;
        
        _effect = new();
        player.AddEffect(_effect);
        
        _originalController = player.Controller;
        player.Controller = new InheritanceRoomPlayerController(player, player.Model, _effect);
    }

    /// <summary>
    /// Creates an ItemVisual for a body item based on the specified visual name.
    /// </summary>
    /// <param name="visualName">The name of the visual to load.</param>
    /// <returns>An ItemVisual configured with the specified visual.</returns>
    private ItemVisual MakeBodyItemVisual(string visualName)
    {
        string scenePath = $"res://Characters/Visual/{visualName.ToLower()}.tscn";
        PackedScene scene = GD.Load<PackedScene>(scenePath);

        ItemVisual visual = new();
        visual.Visual = scene;
        return visual;
    }
    
    /// <summary>
    /// Creates an ItemVisual for a weapon item based on the specified weapon name.
    /// </summary>
    /// <param name="weaponName">The name of the weapon to load.</param>
    /// <returns>An ItemVisual configured with the specified weapon visual.</returns>
    private ItemVisual MakeWeaponItemVisual(string weaponName)
    {
        string scenePath = $"res://Weapons/{weaponName.ToLower()}.tscn";
        PackedScene scene = GD.Load<PackedScene>(scenePath);

        ItemVisual visual = new();
        visual.Visual = scene;
        return visual;
    }
    
    /// <summary>
    /// Called when the player exits the room. Restores the player's original controller and removes the effect and items.
    /// </summary>
    /// <param name="player">The player character.</param>
    public override void OnPlayerExited(CharacterView player)
    {
        player.Controller = _originalController;
        _originalController = null;
        
        player.RemoveEffect(_effect);
        _effect.QueueFree();
        _effect = null;

        if (IsInstanceValid(_bodyItem))
            _bodyItem.QueueFree();
        _bodyItem = null;
        
        if (IsInstanceValid(_weaponItem))
            _weaponItem.QueueFree();
        _weaponItem = null;
    }
}