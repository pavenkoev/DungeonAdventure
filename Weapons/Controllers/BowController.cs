using DungeonAdventure.Characters.Views;
using DungeonAdventure.Utils;
using DungeonAdventure.Weapons.Models;
using DungeonAdventure.Weapons.View;
using DungeonAdventure.World;
using Godot;

namespace DungeonAdventure.Weapons.Controllers;

/// <summary>
/// Controls the behavior of a bow weapon.
/// </summary>
public class BowController
{
    private WeaponModel _model;
    private BowView _view;
    private float _damage = 0;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="BowController"/> class.
    /// </summary>
    /// <param name="view">The view component of the bow.</param>
    /// <param name="model">The model component of the bow.</param>
    public BowController(BowView view, WeaponModel model)
    {
        _model = model;
        _view = view;
    }
    
    /// <summary>
    /// Executes the attack action with the specified damage.
    /// </summary>
    /// <param name="damage">The amount of damage to inflict.</param>
    public void Attack(float damage)
    {
        if (!_model.CanAttack())
            return;

        _damage = damage;
        _model.SetLastAttackTime();
        
        _view.StartAttackAnimation();
    }

    /// <summary>
    /// Shoots an arrow from the bow.
    /// </summary>
    public void ShootArrow()
    {
        Arrow arrow = _view.ArrowScene.Instantiate<Arrow>();
        arrow.Initialize(_view, _damage);
        
        Room room = _view.FindRoom();
        room.AddChild(arrow);
        
        arrow.GlobalTransform = _view.ArrowSpawnTransform;
        
        _view.PlayAttackSound();
    }

    /// <summary>
    /// Handles the collision of the arrow with another body.
    /// </summary>
    /// <param name="body">The body that the arrow collides with.</param>
    /// <param name="damage">The amount of damage to apply on collision.</param>
    /// <returns>True if the collision was handled, otherwise false.</returns>
    public bool OnArrowCollision(Node2D body, float damage)
    {
        if (_view.IsBodyIgnored(body))
            return false;

        CharacterView character = body.FindCharacter();
        if (character != null)
        {
            GD.Print("HIT: " + character.Name);
            character.ApplyDamage(damage);
        }

        return true;
    }
}