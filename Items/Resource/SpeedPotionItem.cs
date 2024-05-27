using DungeonAdventure.Characters;
using DungeonAdventure.Characters.Effects;
using DungeonAdventure.Characters.Views;
using Godot;

namespace DungeonAdventure.Items;

/// <summary>
/// Represents a speed potion item that increases the speed of a character for a duration.
/// </summary>
[Tool]
[GlobalClass]
public partial class SpeedPotionItem : Item
{
    /// <summary>
    /// The factor by which the speed is increased.
    /// </summary>
    [Export] private float _factor = 1.2f;
    
    /// <summary>
    /// The duration for which the speed increase effect lasts.
    /// </summary>
    [Export] private float _duration = 5f;
    
    /// <summary>
    /// Uses the speed potion on the specified character.
    /// </summary>
    /// <param name="character">The character on which to use the item.</param>
    public override void Use(CharacterView character)
    {
        character.AddEffect(new SpeedEffect(_factor, _duration));
    }
}