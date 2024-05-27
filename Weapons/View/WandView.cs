using DungeonAdventure.Characters.Views;
using DungeonAdventure.Utils;
using DungeonAdventure.Weapons.Controllers;
using DungeonAdventure.Weapons.Models;
using Godot;

namespace DungeonAdventure.Weapons.View;

/// <summary>
/// Represents the view for a wand weapon.
/// </summary>
public partial class WandView : WeaponView
{
    [Export] private AnimationPlayer _animationPlayer;
    
    [Export] private PackedScene _spellScene;
    [Export] private Node2D _spellSpawnPosition;
    
    [Export] private AudioStreamPlayer2D _audioPlayer;
    [Export] private AudioStream[] _attackSounds;
    
    private WandController _controller;

    private const string CastAnimationName = "cast";

    /// <summary>
    /// Gets the spell scene for the wand.
    /// </summary>
    public PackedScene SpellScene => _spellScene;
    
    /// <summary>
    /// Gets the transform for spawning spells.
    /// </summary>
    public Transform2D SpellSpawnTransform => _spellSpawnPosition.GlobalTransform;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="WandView"/> class.
    /// </summary>
    public WandView()
    {
        Model = new WeaponModel(0.75f, 300);
        _controller = new WandController(this, Model);
    }
    
    /// <summary>
    /// Gets the length of the attack animation.
    /// </summary>
    /// <returns>The length of the attack animation.</returns>
    protected override float GetAttackAnimationLength() => _animationPlayer.GetAnimation(CastAnimationName).Length;
    
    /// <summary>
    /// Determines whether the weapon is currently attacking.
    /// </summary>
    /// <returns>True if the weapon is attacking, otherwise false.</returns>
    public override bool IsAttacking() => _animationPlayer.IsPlaying();
    
    /// <summary>
    /// Attaches the wand to a character.
    /// </summary>
    /// <param name="character">The character to attach the wand to.</param>
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
        _animationPlayer.Play(CastAnimationName, customSpeed: GetAnimationSpeedMultiplier());
    }

    /// <summary>
    /// Spawns a spell from the wand.
    /// </summary>
    private void SpawnSpell()
    {
        _controller.SpawnSpell();
    }
    
    /// <summary>
    /// Handles the collision of the spell with another body.
    /// </summary>
    /// <param name="body">The body that the spell collides with.</param>
    /// <param name="damage">The amount of damage to apply on collision.</param>
    /// <returns>True if the collision was handled, otherwise false.</returns>
    public bool OnSpellCollision(Node2D body, float damage)
    {
        return _controller.OnSpellCollision(body, damage);
    }
    
    /// <summary>
    /// Plays an attack sound.
    /// </summary>
    public void PlayAttackSound()
    {
        _audioPlayer.PlayRandomSound(_attackSounds);
    }
}