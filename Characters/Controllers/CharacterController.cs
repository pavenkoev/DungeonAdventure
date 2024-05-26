using DungeonAdventure.Characters.Effects;
using DungeonAdventure.Characters.Indicators;
using DungeonAdventure.Characters.Models;
using DungeonAdventure.Characters.Views;
using DungeonAdventure.Items;
using Godot;

namespace DungeonAdventure.Characters.Controllers;

public abstract class CharacterController
{
    protected CharacterView View;
    protected CharacterModel Model;
    
    public IndicatorManager IndicatorManager { get; set; }

    private CharacterStats _stats = new();
    
    public CharacterController(CharacterView view, CharacterModel model)
    {
        View = view;
        Model = model;
    }

    public virtual bool IsPlayer => false;
    
    public virtual void Process(double delta)
    {
        if (!Model.IsAlive)
            return;
        
        _stats = View.CollectEffects((float)delta);
		
        Vector2 direction = GetMoveDirection();

        if (_stats.HealRate != 0)
            Model.Heal(_stats.HealRate);
		
        View.Velocity = direction * Model.Speed * _stats.SpeedModifier;
		
        View.UpdateAnimation(View.Velocity);
		
        View.MoveAndSlide();

        ProcessAttack(_stats.DamageModifier);
    }
    public virtual void PhysicsProcess(double delta) {}
    public abstract Vector2 GetMoveDirection();
    public abstract Vector2? GetAttackDirection();

    public void ApplyDamage(float damage)
    {
        if (!Model.IsAlive)
            return;

        if (damage <= 0)
        {
            IndicatorManager?.AddIndicator($"MISS", new Color(0.8f, 0.8f, 0.8f));
            return;
        }

        if (Model.RandomizeBlock())
        {
            IndicatorManager?.AddIndicator($"BLOCK", new Color(0.8f, 0.8f, 0.8f));
            return;
        }
		
        IndicatorManager?.AddIndicator($"-{(int)damage}", new Color(0.8f, 0, 0));
		
        View.PlayHitSound();
		
        Model.ApplyDamage(damage);
    }

    public void Heal(float value, float duration)
    {
        if (!Model.IsAlive)
            return;
		
        IndicatorManager?.AddIndicator($"+{(int)value}", new Color(0, 0.8f, 0));
		
        if (duration <= 0)
            Model.Heal(value);
        else
            View.AddEffect(new HealEffect(value, duration));
    }
    
    private void ProcessAttack(float damageModifier)
    {
        Vector2? attackDirection = GetAttackDirection();
		

        if (attackDirection.HasValue)
        {
            View.Weapon.SetWeaponAttackSide(attackDirection.Value);

            if (View.Weapon.CanAttack())
            {
                float damage = -1;
                if (!Model.RandomizeMiss())
                    damage = Model.RandomizeDamage() * damageModifier;
                View.Weapon.Attack(damage);
            }
        }
    }

    public void PickupItem(Item item)
    {
        Model.AddItem(item);
    }

    public void UseItem(Item item)
    {
        item.Use(View);
        Model.RemoveItem(item);
    }
}