using DungeonAdventure.Characters.Controllers;
using DungeonAdventure.Characters.Effects;
using DungeonAdventure.Characters.Models;
using DungeonAdventure.Characters.Views;
using DungeonAdventure.Items;
using Item = DungeonAdventure.Items.Item;

namespace DungeonAdventure.World.Rooms.PillarInheritance.Support;

/// <summary>
/// Custom player controller for the inheritance pillar room, managing body and weapon acquisition.
/// </summary>
public class InheritanceRoomPlayerController : PlayerController
{
    /// <summary>
    /// Gets or sets a value indicating whether the player has acquired a body.
    /// </summary>
    public bool HasBody { get; set; } = false;
    
    /// <summary>
    /// Gets or sets a value indicating whether the player has acquired a weapon.
    /// </summary>
    public bool HasWeapon { get; set; } = false;

    private InheritanceEffect _inheritanceEffect;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="InheritanceRoomPlayerController"/> class.
    /// </summary>
    /// <param name="character">The character view controlled by this controller.</param>
    /// <param name="model">The character model associated with the character view.</param>
    /// <param name="inheritanceEffect">The inheritance effect applied to the character.</param>
    public InheritanceRoomPlayerController(CharacterView character, CharacterModel model, 
        InheritanceEffect inheritanceEffect) : base(character, model)
    {
        _inheritanceEffect = inheritanceEffect;
    }

    /// <summary>
    /// Determines whether the player can attack, requiring a weapon.
    /// </summary>
    /// <returns>True if the player can attack, otherwise false.</returns>
    public override bool CanAttack()
    {
        return HasWeapon && base.CanAttack();
    }

    /// <summary>
    /// Determines whether the player can pick up the specified item.
    /// </summary>
    /// <param name="item">The item to check.</param>
    /// <returns>True if the player can pick up the item, otherwise false.</returns>
    public override bool CanPickupItem(Item item)
    {
        if (!HasBody)
        {
            return item is CharacterBodyItem;
        }

        if (!HasWeapon)
        {
            return item is CharacterWeaponItem;
        }
        
        return base.CanPickupItem(item);
    }

    /// <summary>
    /// Picks up the specified item, updating the player's state accordingly.
    /// </summary>
    /// <param name="item">The item to pick up.</param>
    public override void PickupItem(Item item)
    {
        if (item is CharacterBodyItem)
        {
            HasBody = true;
            _inheritanceEffect.AffectCharacter = false;
            return;
        }

        if (item is CharacterWeaponItem)
        {
            HasWeapon = true;
            _inheritanceEffect.AffectWeapon = false;
            return;
        }
        
        base.PickupItem(item);
    }

    /// <summary>
    /// Determines whether the player can use the specified item.
    /// </summary>
    /// <param name="item">The item to check.</param>
    /// <returns>True if the player can use the item, otherwise false.</returns>
    public override bool CanUseItem(Item item)
    {
        return HasBody && base.CanUseItem(item);
    }

    /// <summary>
    /// Applies damage to the player, reducing it to zero if the player has no body.
    /// </summary>
    /// <param name="damage">The amount of damage to apply.</param>
    public override void ApplyDamage(float damage)
    {
        if (!HasBody)
            damage = 0;
        base.ApplyDamage(damage);
    }
}