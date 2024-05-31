using DungeonAdventure.Characters.Effects;
using DungeonAdventure.Characters.Views;
using Godot;

namespace DungeonAdventure.Items;

/// <summary>
/// Represents a attack rate potion item that increases the attack rate of a character for a duration.
/// </summary>
[Tool]
[GlobalClass]
public partial class AttackRatePotionItem : Item
{
    /// <summary>
    /// The factor by which the attack rate is increased.
    /// </summary>
    [Export] private float _factor = 0.5f;
    
    /// <summary>
    /// The duration for which the attack rate increase effect lasts.
    /// </summary>
    [Export] private float _duration = 5f;
    
    public override string ItemId => "attack_rate_potion";
    
    /// <summary>
    /// Uses the attack rate potion on the specified character.
    /// </summary>
    /// <param name="character">The character on which to use the item.</param>
    public override void Use(CharacterView character)
    {
        character.AddEffect(new AttackRateEffect(_factor, _duration));
    }
}