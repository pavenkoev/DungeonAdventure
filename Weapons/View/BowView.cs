using DungeonAdventure.Characters.Views;
using DungeonAdventure.Utils;
using DungeonAdventure.Weapons.Controllers;
using DungeonAdventure.Weapons.Models;
using DungeonAdventure.World;
using Godot;

namespace DungeonAdventure.Weapons.View;

/// <summary>
/// Represents the view for a bow weapon.
/// </summary>
public partial class BowView : WeaponView
{
    [Export] private AnimationPlayer _animationPlayer;
    
    [Export] private PackedScene _arrowScene;
    [Export] private Node2D _arrowSpawnPosition;
    
    [Export] private AudioStreamPlayer2D _audioPlayer;
    [Export] private AudioStream[] _attackSounds;
    
    private BowController _controller;

    private const string ShootAnimationName = "shoot";

    /// <summary>
    /// Gets the arrow scene for the bow.
    /// </summary>
    public PackedScene ArrowScene => _arrowScene;
    
    /// <summary>
    /// Gets the transform for spawning arrows.
    /// </summary>
    public Transform2D ArrowSpawnTransform=> _arrowSpawnPosition.GlobalTransform;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="BowView"/> class.
    /// </summary>
    public BowView()
    {
        Model = new WeaponModel(0.5f, 300);
        _controller = new BowController(this, Model);
    }

    /// <summary>
    /// Gets the length of the attack animation.
    /// </summary>
    /// <returns>The length of the attack animation.</returns>
    protected override float GetAttackAnimationLength() => _animationPlayer.GetAnimation(ShootAnimationName).Length;
    
    /// <summary>
    /// Determines whether the weapon is currently attacking.
    /// </summary>
    /// <returns>True if the weapon is attacking, otherwise false.</returns>
    public override bool IsAttacking() => _animationPlayer.IsPlaying();

    /// <summary>
    /// Attaches the bow to a character.
    /// </summary>
    /// <param name="character">The character to attach the bow to.</param>
    public override void Attach(CharacterView character)
    {
        ClearIgnoredBodies();
        AddIgnoredBody(character.HitArea);
        AddIgnoredBody(character.Collision);
    }
    
    /// <summary>
    /// Executes the attack action with the specified damage.
    /// </summary>
    /// <param name="damage">The amount of damage to inflict.</param>
    public override void Attack(float damage)
    {
        _controller.Attack(damage);
    }

    /// <summary>
    /// Starts the attack animation.
    /// </summary>
    public void StartAttackAnimation()
    {
        _animationPlayer.Stop();
        _animationPlayer.Play(ShootAnimationName, customSpeed: GetAnimationSpeedMultiplier());
    }

    /// <summary>
    /// Shoots an arrow from the bow.
    /// </summary>
    private void Shoot()
    {
        _controller.ShootArrow();
    }

    /// <summary>
    /// Handles the collision of the arrow with another body.
    /// </summary>
    /// <param name="body">The body that the arrow collides with.</param>
    /// <param name="damage">The amount of damage to apply on collision.</param>
    /// <returns>True if the collision was handled, otherwise false.</returns>
    public bool OnArrowCollision(Node2D body, float damage)
    {
        return _controller.OnArrowCollision(body, damage);
    }
    
    /// <summary>
    /// Plays an attack sound.
    /// </summary>
    public void PlayAttackSound()
    {
        _audioPlayer.PlayRandomSound(_attackSounds);
    }
}