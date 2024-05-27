using DungeonAdventure.Characters;
using DungeonAdventure.Characters.Effects;
using DungeonAdventure.Characters.Views;
using Godot;

namespace DungeonAdventure.Items;

/// <summary>
/// Represents a damage potion item that increases the damage dealt by a character for a duration.
/// </summary>
[Tool]
[GlobalClass]
public partial class DamagePotionItem : Item
{
    /// <summary>
    /// The factor by which the damage is increased.
    /// </summary>
    [Export] private float _factor = 1.2f;
    
    /// <summary>
    /// The duration for which the damage increase effect lasts.
    /// </summary>
    [Export] private float _duration = 5f;
    
    /// <summary>
    /// Uses the damage potion on the specified character.
    /// </summary>
    /// <param name="character">The character on which to use the item.</param>
    public override void Use(CharacterView character)
    {
        character.AddEffect(new DamageEffect(_factor, _duration));
    }
}