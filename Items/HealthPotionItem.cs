using DungeonAdventure.Characters.Views;
using Godot;

namespace DungeonAdventure.Items;

/// <summary>
/// Represents a health potion item that heals a character.
/// </summary>
[Tool]
[GlobalClass]
public partial class HealthPotionItem : Item
{
    /// <summary>
    /// The amount of health restored by the potion.
    /// </summary>
    [Export] private int _value;

    /// <summary>
    /// The id of the item to locate item's visual.
    /// </summary>
    public override string ItemId => "health_potion";

    /// <summary>
    /// Uses the health potion on the specified character.
    /// </summary>
    /// <param name="character">The character on which to use the item.</param>
    public override void Use(CharacterView character)
    {
        character.Heal(_value, 1);
    }
}