using DungeonAdventure.Characters;
using DungeonAdventure.Utils;
using DungeonAdventure.World;
using Godot;

namespace DungeonAdventure.Weapons;

public partial class Wand : Weapon
{
    [Export] private AnimationPlayer _animationPlayer;
    
    [Export] private PackedScene _spellScene;
    [Export] private Node2D _spellSpawnPosition;
    
    [Export] private AudioStreamPlayer2D _audioPlayer;
    [Export] private AudioStream[] _attackSounds;
    
    private float _damage = -1;

    private const string CastAnimationName = "cast";
    
    public override void Attach(Character character)
    {
        ClearIgnoredBodies();
        AddIgnoredBodies(character.HitArea);
        AddIgnoredBodies(character.Collision);
    }

    public override void Attack(float damage)
    {
        if (!CanAttack())
            return;

        _damage = damage;

        SetLastAttackTime();
        
        _animationPlayer.Play(CastAnimationName);
    }

    private void SpawnSpell()
    {
        Spell spell = _spellScene.Instantiate<Spell>();
        spell.Initialize(this);

        Room room = this.FindRoom();
        room.AddChild(spell);
        
        spell.GlobalTransform = _spellSpawnPosition.GlobalTransform;
        
        PlayAttackSound();
    }
    
    public bool OnSpellCollision(Node2D body)
    {
        if (IsBodyIgnored(body))
            return false;

        Character character = body.FindCharacter();
        if (character != null)
        {
            GD.Print("HIT: " + character.Name);
            character.ApplyDamage(_damage);
        }

        return true;
    }
    
    private void PlayAttackSound()
    {
        _audioPlayer.PlayRandomSound(_attackSounds);
    }
}