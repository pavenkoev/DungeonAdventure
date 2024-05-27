using DungeonAdventure.Utils;
using Godot;

namespace DungeonAdventure.Weapons.Models;

public class WeaponModel
{
    private ITimeProvider _timeProvider;
    public float AttackRate { get; private set; } = 1;
    public float AttackRange { get; private set; } = 70;
    
    private ulong _lastAttackTimeMs = 0;

    private const float MsToSecondFactor = 1.0f / 1000;

    public WeaponModel(float rate, float range) : this(rate, range, new TimeProvider())
    {
        
    }
    
    public WeaponModel(float rate, float range, ITimeProvider timeProvider)
    {
        AttackRate = rate;
        AttackRange = range;
        _timeProvider = timeProvider;
    }
    
    public bool CheckAttackRate()
    {
        return (_timeProvider.GetTicksMs() - _lastAttackTimeMs) * MsToSecondFactor >= AttackRate;
    }

    public void SetLastAttackTime()
    {
        _lastAttackTimeMs = _timeProvider.GetTicksMs();
    }
    
    public virtual bool CanAttack() => CheckAttackRate();
}