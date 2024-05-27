using DungeonAdventure.Weapons.Models;
using GdUnit4;

namespace DungeonAdventure.Tests;

[TestSuite]
public class WeaponTests
{
    private TimeProviderMock _timeProvider;
    private WeaponModel _model;

    [BeforeTest]
    public void Setup()
    {
        _timeProvider = new TimeProviderMock(10);
        _model = new(1, 100, _timeProvider);
    }

    [TestCase]
    public void NewWeaponCanAttack()
    {
        Assertions.AssertThat(_model.CanAttack()).IsTrue();
    }

    [TestCase]
    public void WeaponCantAttackRightAfterAttacking()
    {
        _model.SetLastAttackTime();
        Assertions.AssertThat(_model.CanAttack()).IsFalse();
    }

    [TestCase]
    public void WeaponCantAttackWhenNotEnoughTimePassed()
    {
        _model.SetLastAttackTime();
        _timeProvider.Time += _model.AttackRate / 2;
        Assertions.AssertThat(_model.CanAttack()).IsFalse();
    }
    
    [TestCase]
    public void WeaponCanAttackWhenEnoughTimePassed()
    {
        _model.SetLastAttackTime();
        _timeProvider.Time += _model.AttackRate * 1.1;
        Assertions.AssertThat(_model.CanAttack()).IsTrue();
    }
}