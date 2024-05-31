using DungeonAdventure.Utils;
using Godot;

namespace DungeonAdventure.Weapons.Models;

/// <summary>
/// Represents the model for a weapon.
/// </summary>
public class WeaponModel
{
    private ITimeProvider _timeProvider;

    private float _attackRate = 1;
    
    /// <summary>
    /// Gets the rate at which the weapon can attack.
    /// </summary>
    public float AttackRate 
    { 
        get => _attackRate;
        set => _attackRate = (float)Mathf.Clamp(value, 0.01, value);
    }
    
    /// <summary>
    /// Gets the range of the weapon's attack.
    /// </summary>
    public float AttackRange { get; private set; } = 70;
    
    private ulong _lastAttackTimeMs = 0;

    private const float MsToSecondFactor = 1.0f / 1000;

    /// <summary>
    /// Initializes a new instance of the <see cref="WeaponModel"/> class with the specified rate and range.
    /// </summary>
    /// <param name="rate">The rate at which the weapon can attack.</param>
    /// <param name="range">The range of the weapon's attack.</param>
    public WeaponModel(float rate, float range) : this(rate, range, new TimeProvider())
    {
        
    }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="WeaponModel"/> class with the specified rate, range, and time provider.
    /// </summary>
    /// <param name="rate">The rate at which the weapon can attack.</param>
    /// <param name="range">The range of the weapon's attack.</param>
    /// <param name="timeProvider">The time provider to use for attack timing.</param>
    public WeaponModel(float rate, float range, ITimeProvider timeProvider)
    {
        AttackRate = rate;
        AttackRange = range;
        _timeProvider = timeProvider;
    }
    
    /// <summary>
    /// Checks if the weapon can attack based on the attack rate.
    /// </summary>
    /// <returns>True if the weapon can attack, otherwise false.</returns>
    public bool CheckAttackRate()
    {
        return (_timeProvider.GetTicksMs() - _lastAttackTimeMs) * MsToSecondFactor >= AttackRate;
    }

    /// <summary>
    /// Sets the last attack time to the current time.
    /// </summary>
    public void SetLastAttackTime()
    {
        _lastAttackTimeMs = _timeProvider.GetTicksMs();
    }
    
    /// <summary>
    /// Determines whether the weapon can attack.
    /// </summary>
    /// <returns>True if the weapon can attack, otherwise false.</returns>
    public virtual bool CanAttack() => CheckAttackRate();
}