using DungeonAdventure.Characters.Controllers;
using DungeonAdventure.Characters.Effects;
using DungeonAdventure.Characters.Views;
using DungeonAdventure.Items;
using Godot;

namespace DungeonAdventure.World.Rooms.PillarInheritance.Support;

/// <summary>
/// Represents a room of the pillar of inheritance.
/// </summary>
public partial class InheritancePillarRoom : Room
{
    private InheritanceEffect _effect;
    private CharacterController _originalController;
    private ItemObject _bodyItem;
    private ItemObject _weaponItem;

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
        _bodyItem = _itemObjectScene.Instantiate<ItemObject>();
        _bodyItem.Item = new CharacterBodyItem(player.Model.VisualName);
        AddChild(_bodyItem);
        _bodyItem.Position = _bodyItemPosition.Position;
        
        _weaponItem = _itemObjectScene.Instantiate<ItemObject>();
        _weaponItem.Item = new CharacterWeaponItem(player.Model.WeaponName);
        AddChild(_weaponItem);
        _weaponItem.Position = _weaponItemPosition.Position;
        
        _effect = new();
        player.AddEffect(_effect);
        
        _originalController = player.Controller;
        player.Controller = new InheritanceRoomPlayerController(player, player.Model, _effect);
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