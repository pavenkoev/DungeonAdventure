using Godot;

namespace DungeonAdventure;

public partial class Character : CharacterBody2D
{
    [Export] protected float _speed = 80.0f;
    [Export] protected float _health = 100.0f;
    
    [Export] protected AnimatedSprite2D _sprite;
    
    [Export] protected Sword _weapon;
    
    [Export] protected Node2D _weaponPivotUp;
    [Export] protected Node2D _weaponPivotDown;
    [Export] protected Node2D _weaponPivotLeft;
    [Export] protected Node2D _weaponPivotRight;
    
    public override void _Ready()
    {
        _weapon.Attach(this);
    }
    
    protected void UpdateAnimation(Vector2 velocity)
    {
        if (!velocity.IsZeroApprox())
        {
            _sprite.Play("run");
            _sprite.FlipH = velocity.X < 0;
        }
        else
        {
            _sprite.Play("idle");
        }
    }
    
    public void ApplyDamage(float damage)
    {
        _health -= damage;
        GD.Print("health: " + _health);
        if (_health <= 0)
        {
            QueueFree();
        }
    }

    protected void SetWeaponAttackSide(AttackSide side)
    {
        switch (side)
        {
            case AttackSide.Up:
                _weapon.Reparent(_weaponPivotUp, false);
                break;
            case AttackSide.Down:
                _weapon.Reparent(_weaponPivotDown, false);
                break;
            case AttackSide.Left:
                _weapon.Reparent(_weaponPivotLeft, false);
                break;
            case AttackSide.Right:
                _weapon.Reparent(_weaponPivotRight, false);
                break;
        }
    }
}