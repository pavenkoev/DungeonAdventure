using Godot;

namespace DungeonAdventure.Weapons.Models;

public class WeaponModel
{
    public float AttackRate { get; private set; } = 1;
    public float AttackRange { get; private set; } = 70;
    
    private ulong _lastAttackTimeMs = 0;

    private const float MsToSecondFactor = 1.0f / 1000;

    public WeaponModel(float rate, float range)
    {
        AttackRate = rate;
        AttackRange = range;
    }
    
    public bool CheckAttackRate()
    {
        return (Time.GetTicksMsec() - _lastAttackTimeMs) * MsToSecondFactor >= AttackRate;
    }

    public void SetLastAttackTime()
    {
        _lastAttackTimeMs = Time.GetTicksMsec();
    }
    
    public virtual bool CanAttack() => CheckAttackRate();
}