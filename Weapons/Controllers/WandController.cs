using DungeonAdventure.Characters.Views;
using DungeonAdventure.Utils;
using DungeonAdventure.Weapons.Models;
using DungeonAdventure.Weapons.View;
using DungeonAdventure.World;
using Godot;

namespace DungeonAdventure.Weapons.Controllers;

/// <summary>
/// Controls the behavior of a wand weapon.
/// </summary>
public class WandController
{
    private WeaponModel _model;
    private WandView _view;
    private float _damage = 0;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="WandController"/> class.
    /// </summary>
    /// <param name="view">The view component of the wand.</param>
    /// <param name="model">The model component of the wand.</param>
    public WandController(WandView view, WeaponModel model)
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
        if (!_model.CanAttack() || _view.IsAttacking())
            return;

        _damage = damage;
        _model.SetLastAttackTime();
        
        _view.StartAttackAnimation();
    }

    /// <summary>
    /// Spawns a spell from the wand.
    /// </summary>
    public void SpawnSpell()
    {
        Spell spell = _view.SpellScene.Instantiate<View.Spell>();
        spell.Initialize(_view, _damage);

        Room room = _view.FindRoom();
        room.AddChild(spell);
        
        spell.GlobalTransform = _view.SpellSpawnTransform;
        
        _view.PlayAttackSound();
    }
    
    /// <summary>
    /// Handles the collision of the spell with another body.
    /// </summary>
    /// <param name="body">The body that the spell collides with.</param>
    /// <param name="damage">The amount of damage to apply on collision.</param>
    /// <returns>True if the collision was handled, otherwise false.</returns>
    public bool OnSpellCollision(Node2D body, float damage)
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