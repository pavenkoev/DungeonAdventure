using DungeonAdventure.Characters.Views;
using DungeonAdventure.Utils;
using DungeonAdventure.Weapons.Controllers;
using DungeonAdventure.Weapons.Models;
using DungeonAdventure.World;
using Godot;

namespace DungeonAdventure.Weapons.View;

public partial class BowView : WeaponView
{
    [Export] private AnimationPlayer _animationPlayer;
    
    [Export] private PackedScene _arrowScene;
    [Export] private Node2D _arrowSpawnPosition;
    
    [Export] private AudioStreamPlayer2D _audioPlayer;
    [Export] private AudioStream[] _attackSounds;
    
    private BowController _controller;

    private const string ShootAnimationName = "shoot";

    public PackedScene ArrowScene => _arrowScene;
    public Transform2D ArrowSpawnTransform=> _arrowSpawnPosition.GlobalTransform;
    
    public BowView()
    {
        Model = new WeaponModel(0.5f, 300);
        _controller = new BowController(this, Model);
    }

    protected override float GetAttackAnimationLength() => _animationPlayer.GetAnimation(ShootAnimationName).Length;
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
        _animationPlayer.Play(ShootAnimationName, customSpeed: GetAnimationSpeedMultiplier());
    }

    private void Shoot()
    {
        _controller.ShootArrow();
    }

    public bool OnArrowCollision(Node2D body, float damage)
    {
        return _controller.OnArrowCollision(body, damage);
    }
    
    public void PlayAttackSound()
    {
        _audioPlayer.PlayRandomSound(_attackSounds);
    }
}