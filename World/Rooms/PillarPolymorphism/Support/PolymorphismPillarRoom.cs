using System;
using System.Linq;
using DungeonAdventure.Characters.Effects;
using DungeonAdventure.Characters.Views;
using DungeonAdventure.Items;
using DungeonAdventure.Utils;
using Godot;

namespace DungeonAdventure.World.Rooms.PillarPolymorphism.Support;

/// <summary>
/// Represents a room of the pillar of polymorphism where items and characters change periodically.
/// </summary>
public partial class PolymorphismPillarRoom : Room
{
    /// <summary>
    /// The period between changes in seconds.
    /// </summary>
    [Export] private float _changePeriod = 3f;

    /// <summary>
    /// The possible items that can appear in the room.
    /// </summary>
    [Export] private Item[] _possibleItems = { };

    /// <summary>
    /// The possible character visuals that can appear in the room.
    /// </summary>
    [Export] private string[] _possibleCharacterVisuals =
    {
        "Knight", "Rogue", "Wizard",
        "Skeleton", "Orc", "Ghost"
    };
    
    /// <summary>
    /// The possible weapons that can appear in the room.
    /// </summary>
    [Export] private string[] _possibleWeapons =
    {
        "Sword", "Bow", "Wand"
    };

    /// <summary>
    /// The scene for the polymorphism effect.
    /// </summary>
    [Export] private PackedScene _polymorphismEffectScene;
    
    private Timer _changeTimer;
    private Random _random = new();

    private string _playerOriginalVisual = "";
    private string _playerOriginalWeapon = "";
    
    /// <summary>
    /// Called when the node is added to the scene. Initializes the room.
    /// </summary>
    public override void _Ready()
    {
        _changeTimer = new();
        AddChild(_changeTimer);
        _changeTimer.OneShot = false;
        _changeTimer.Timeout += OnChangeTimer;
        _changeTimer.Start(_changePeriod);
    }

    /// <summary>
    /// Called when the change timer times out. Triggers changes in items and characters.
    /// </summary>
    private void OnChangeTimer()
    {
        ChangeItems();
        ChangeCharacters();
    }

    /// <summary>
    /// Changes the items in the room.
    /// </summary>
    private void ChangeItems()
    {
        foreach (ItemObject itemObject in this.FindNodesDown<ItemObject>())
        {
            ChangeItem(itemObject);
        }
    }

    /// <summary>
    /// Changes a specific item in the room.
    /// </summary>
    /// <param name="itemObject">The item object to change.</param>
    private void ChangeItem(ItemObject itemObject)
    {
        if (_possibleItems.Contains(itemObject.Item))
        {
            Item nextItem = _possibleItems[_random.Next(_possibleItems.Length)];
            itemObject.Item = nextItem;
            
            PolymorphismEffect effect = _polymorphismEffectScene.Instantiate<PolymorphismEffect>();
            itemObject.AddChild(effect);
        }
    }

    /// <summary>
    /// Changes the characters in the room.
    /// </summary>
    private void ChangeCharacters()
    {
        foreach (CharacterView character in this.FindNodesDown<CharacterView>())
        {
            ChangeCharacter(character);
        }
    }

    /// <summary>
    /// Changes a specific character in the room.
    /// </summary>
    /// <param name="character">The character to change.</param>
    private void ChangeCharacter(CharacterView character)
    {
        if (!character.IsAlive)
            return;
        
        string nextVisual = _possibleCharacterVisuals[_random.Next(_possibleCharacterVisuals.Length)];
        string nextWeapon = _possibleWeapons[_random.Next(_possibleWeapons.Length)];

        character.UpdateVisual(nextVisual);
        character.UpdateWeapon(nextWeapon);

        PolymorphismEffect effect = _polymorphismEffectScene.Instantiate<PolymorphismEffect>();
        effect.Scale = Vector2.One * 2;
        character.AddEffect(effect);
    }

    /// <summary>
    /// Called when the player enters the room. Stores the player's original visual and weapon.
    /// </summary>
    /// <param name="player">The player character.</param>
    public override void OnPlayerEntered(CharacterView player)
    {
        _playerOriginalVisual = player.Model.VisualName;
        _playerOriginalWeapon = player.Model.WeaponName;
    }

    /// <summary>
    /// Called when the player exits the room. Restores the player's original visual and weapon.
    /// </summary>
    /// <param name="player">The player character.</param>
    public override void OnPlayerExited(CharacterView player)
    {
        player.UpdateVisual(_playerOriginalVisual);
        player.UpdateWeapon(_playerOriginalWeapon);
    }

    /// <summary>
    /// Pauses the room, stopping the change timer.
    /// </summary>
    public override void Pause()
    {
        base.Pause();
        
        _changeTimer.Stop();
    }

    /// <summary>
    /// Resumes the room, starting the change timer.
    /// </summary>
    public override void Resume()
    {
        base.Resume();
        
        _changeTimer.Start(_changePeriod);
    }
}