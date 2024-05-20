using System.Collections.Generic;
using DungeonAdventure.Characters;
using DungeonAdventure.Utils;
using Godot;

namespace DungeonAdventure.Weapons;

public partial class Bow : Weapon
{
    [Export] private AnimationPlayer _animationPlayer;
    
    [Export] private PackedScene _arrowScene;
    [Export] private Node2D _arrowSpawnPosition;
    
    [Export] private AudioStreamPlayer2D _audioPlayer;
    [Export] private AudioStream[] _attackSounds;
    
    private float _damageModifier = 1;

    private const string ShootAnimationName = "shoot";
    
    public override void Attach(Character character)
    {
        ClearIgnoredBodies();
        AddIgnoredBodies(character.HitArea);
        AddIgnoredBodies(character.Collision);
    }
    
    public override void Attack(float damageModifier)
    {
        if (!CanAttack())
            return;

        _damageModifier = damageModifier;

        SetLastAttackTime();
        
        _animationPlayer.Play(ShootAnimationName);
    }

    private void Shoot()
    {
        Arrow arrow = _arrowScene.Instantiate<Arrow>();
        arrow.Initialize(this);
        AddChild(arrow);
        arrow.TopLevel = true;
        arrow.GlobalTransform = _arrowSpawnPosition.GlobalTransform;
        
        PlayAttackSound();
    }

    public bool OnArrowCollision(Node2D body)
    {
        if (IsBodyIgnored(body))
            return false;

        Character character = body.FindCharacter();
        if (character != null)
        {
            GD.Print("HIT: " + character.Name);
            character.ApplyDamage(Damage * _damageModifier);
        }

        return true;
    }
    
    private void PlayAttackSound()
    {
        _audioPlayer.PlayRandomSound(_attackSounds);
    }
}