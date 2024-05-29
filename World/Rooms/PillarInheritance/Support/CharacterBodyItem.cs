using DungeonAdventure.Characters.Views;
using Item = DungeonAdventure.Items.Item;

namespace DungeonAdventure.World.Rooms.PillarInheritance.Support;

/// <summary>
/// Represents an item that restores the character's body.
/// </summary>
public partial class CharacterBodyItem : Item
{
    public override string ItemId => "";

    public override void Use(CharacterView character)
    {
        
    }
}