using DungeonAdventure.Characters.Views;
using Godot;

namespace DungeonAdventure.Characters.Effects;

/// <summary>
/// Represents an effect that modifies the appearance of a character and its weapon.
/// </summary>
public partial class InheritanceEffect : Effect
{
    /// <summary>
    /// Gets or sets a value indicating whether the effect affects the character's appearance.
    /// </summary>
    public bool AffectCharacter { get; set; } = true;
    
    /// <summary>
    /// Gets or sets a value indicating whether the effect affects the weapon's appearance.
    /// </summary>
    public bool AffectWeapon { get; set; } = true;

    private Color _characterOriginalModulate;
    private Color _weaponOriginalModulate;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="InheritanceEffect"/> class with an infinite duration.
    /// </summary>
    public InheritanceEffect() : base(-1) {}
    
    /// <summary>
    /// Applies the effect to the specified character, modifying its appearance and that of its weapon.
    /// </summary>
    /// <param name="delta">The elapsed time since the last frame.</param>
    /// <param name="character">The character to which the effect is applied.</param>
    /// <param name="stats">The character's stats to be modified by the effect.</param>
    public override void Apply(float delta, CharacterView character, CharacterStats stats)
    {
        Character.Modulate = AffectCharacter ? new Color(_characterOriginalModulate, 0.2f) : _characterOriginalModulate;
        Character.Weapon.Modulate = AffectWeapon ? new Color(_weaponOriginalModulate, 0.2f) : _weaponOriginalModulate;
    }

    /// <summary>
    /// Called when the effect starts. Stores the original appearance of the character and its weapon.
    /// </summary>
    protected override void OnStart()
    {
        _characterOriginalModulate = Character.Modulate;
        _weaponOriginalModulate = Character.Weapon.Modulate;
    }

    /// <summary>
    /// Called when the effect ends. Restores the original appearance of the character and its weapon.
    /// </summary>
    protected override void OnEnd()
    {
        Character.Modulate = _characterOriginalModulate;
        Character.Weapon.Modulate = _weaponOriginalModulate;
    }
}