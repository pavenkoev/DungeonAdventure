using System.Collections.Generic;
using DungeonAdventure.Characters;
using Godot;

namespace DungeonAdventure.Weapons;

public partial class Bow : Weapon
{
    [Export] private AnimationPlayer _animationPlayer;
    
    [Export] private PackedScene _arrowScene;
    [Export] private Node2D _arrowSpawnPosition;
    
    public override void Attach(Character character)
    {
        ClearIgnoredBodies();
        AddIgnoredBodies(character.HitArea);
        AddIgnoredBodies(character.Collision);
    }
    
    public override void Attack()
    {
        if (!CanAttack())
            return;

        SetLastAttackTime();
        
        _animationPlayer.Play("shoot");
    }

    private void Shoot()
    {
        Arrow arrow = _arrowScene.Instantiate<Arrow>();
        arrow.Initialize(this);
        AddChild(arrow);
        arrow.TopLevel = true;
        arrow.GlobalTransform = _arrowSpawnPosition.GlobalTransform;
    }

    public bool OnArrowCollision(Node2D body)
    {
        if (IsBodyIgnored(body))
            return false;

        Character character = LocateCharacter(body);
        if (character != null)
        {
            GD.Print("HIT: " + character.Name);
            character.ApplyDamage(Damage);
        }

        return true;
    }

    private Character LocateCharacter(Node node)
    {
        if (node == null)
            return null;

        if (node is Character)
            return (Character)node;

        return LocateCharacter(node.GetParent());
    }
}