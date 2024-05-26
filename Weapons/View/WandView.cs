using DungeonAdventure.Characters.Views;
using DungeonAdventure.Utils;
using DungeonAdventure.Weapons.Controllers;
using DungeonAdventure.Weapons.Models;
using Godot;

namespace DungeonAdventure.Weapons.View;

public partial class WandView : WeaponView
{
    [Export] private AnimationPlayer _animationPlayer;
    
    [Export] private PackedScene _spellScene;
    [Export] private Node2D _spellSpawnPosition;
    
    [Export] private AudioStreamPlayer2D _audioPlayer;
    [Export] private AudioStream[] _attackSounds;
    
    private WandController _controller;

    private const string CastAnimationName = "cast";

    public PackedScene SpellScene => _spellScene;
    public Transform2D SpellSpawnTransform => _spellSpawnPosition.GlobalTransform;
    
    public WandView()
    {
        Model = new WeaponModel(0.75f, 300);
        _controller = new WandController(this, Model);
    }
    
    protected override float GetAttackAnimationLength() => _animationPlayer.GetAnimation(CastAnimationName).Length;
    public override bool IsAttacking() => _animationPlayer.IsPlaying();
    
    public override void Attach(CharacterView character)
    {
        ClearIgnoredBodies();
        AddIgnoredBodies(character.HitArea);
        AddIgnoredBodies(character.Collision);
    }

    public override void Attack(float damage)
    {
        _controller.Attack(damage);
    }
    
    public void StartAttackAnimation()
    {
        _animationPlayer.Stop();
        _animationPlayer.Play(CastAnimationName, customSpeed: GetAnimationSpeedMultiplier());
    }

    private void SpawnSpell()
    {
        _controller.SpawnSpell();
    }
    
    public bool OnSpellCollision(Node2D body, float damage)
    {
        return _controller.OnSpellCollision(body, damage);
    }
    
    public void PlayAttackSound()
    {
        _audioPlayer.PlayRandomSound(_attackSounds);
    }
}