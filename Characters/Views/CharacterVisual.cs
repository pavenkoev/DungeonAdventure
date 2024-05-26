using Godot;

namespace DungeonAdventure.Characters.Views;

public partial class CharacterVisual : Node2D
{
    [Export] public Sprite2D Sprite { get; private set; }
    [Export] public AnimationPlayer AnimationPlayer { get; private set; }
}