using DungeonAdventure.Characters.Views;
using DungeonAdventure.Utils;
using DungeonAdventure.Weapons.Models;
using DungeonAdventure.Weapons.View;
using DungeonAdventure.World;
using Godot;

namespace DungeonAdventure.Weapons.Controllers;

public class BowController
{
    private WeaponModel _model;
    private BowView _view;
    private float _damage = 0;
    
    public BowController(BowView view, WeaponModel model)
    {
        _model = model;
        _view = view;
    }
    
    public void Attack(float damage)
    {
        if (!_model.CanAttack())
            return;

        _damage = damage;
        _model.SetLastAttackTime();
        
        _view.StartAttackAnimation();
    }

    public void ShootArrow()
    {
        Arrow arrow = _view.ArrowScene.Instantiate<Arrow>();
        arrow.Initialize(_view, _damage);
        
        Room room = _view.FindRoom();
        room.AddChild(arrow);
        
        arrow.GlobalTransform = _view.ArrowSpawnTransform;
        
        _view.PlayAttackSound();
    }

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