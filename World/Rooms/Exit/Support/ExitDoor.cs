using Godot;

namespace DungeonAdventure.World.Rooms.Exit.Support;

public partial class ExitDoor : Node2D
{
    [Export] private AnimationPlayer _animationPlayer;
    [Export] private StaticBody2D _block;

    public void Open()
    {
        _animationPlayer.Play("open");
        _block.QueueFree();
    }
}