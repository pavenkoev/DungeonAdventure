using DungeonAdventure.Characters.Views;
using DungeonAdventure.Utils;
using DungeonAdventure.Weapons.Models;
using DungeonAdventure.Weapons.View;
using DungeonAdventure.World;
using Godot;

namespace DungeonAdventure.Weapons.Controllers;

public class WandController
{
    private WeaponModel _model;
    private WandView _view;
    private float _damage = 0;
    
    public WandController(WandView view, WeaponModel model)
    {
        _model = model;
        _view = view;
    }
    
    public void Attack(float damage)
    {
        if (!_model.CanAttack() || _view.IsAttacking())
            return;

        _damage = damage;
        _model.SetLastAttackTime();
        
        _view.StartAttackAnimation();
    }

    public void SpawnSpell()
    {
        Spell spell = _view.SpellScene.Instantiate<View.Spell>();
        spell.Initialize(_view, _damage);

        Room room = _view.FindRoom();
        room.AddChild(spell);
        
        spell.GlobalTransform = _view.SpellSpawnTransform;
        
        _view.PlayAttackSound();
    }
    
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