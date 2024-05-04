using Godot;
using System;

public class HealthBar : Control
{
    [Export]
    public float MaxHealth { get; set; } = 100f;

    private float currentHealth;

    public override void _Ready()
    {
        currentHealth = MaxHealth;
        UpdateHealthBar();
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }
        UpdateHealthBar();
    }

    public void Heal(float healAmount)
    {
        currentHealth += healAmount;
        if (currentHealth > MaxHealth)
        {
            currentHealth = MaxHealth;
        }
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        float healthPercentage = currentHealth / MaxHealth;
        GetNode<Control>("HealthBarForeground").RectScale = new Vector2(healthPercentage, 1f);
    }
}
