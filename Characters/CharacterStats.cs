namespace DungeonAdventure.Characters;

/// <summary>
/// Represents the stats of a character that could be modified by effects.
/// </summary>
public class CharacterStats
{
    /// <summary>
    /// Gets or sets the healing rate of the character.
    /// </summary>
    public float HealRate { get; set; } = 0;
    
    /// <summary>
    /// Gets or sets the speed modifier for the character.
    /// </summary>
    public float SpeedModifier { get; set; } = 1;
    
    /// <summary>
    /// Gets or sets the damage modifier for the character.
    /// </summary>
    public float DamageModifier { get; set; } = 1;
}