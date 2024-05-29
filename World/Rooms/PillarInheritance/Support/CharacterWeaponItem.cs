using DungeonAdventure.Characters.Views;
using Item = DungeonAdventure.Items.Item;

namespace DungeonAdventure.World.Rooms.PillarInheritance.Support;

/// <summary>
/// Represents an item that restores the character's weapon.
/// </summary>
public partial class CharacterWeaponItem : Item
{
    public override string ItemId => "";
    
    public override void Use(CharacterView character)
    {
        
    }
}