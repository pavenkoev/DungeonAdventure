using DungeonAdventure.Characters;
using DungeonAdventure.Characters.Views;
using DungeonAdventure.Utils;
using Godot;

namespace DungeonAdventure.World.Rooms.Exit.Support;

public partial class Exit : Node2D
{
    [Export] private ExitDoor[] _doors;
    [Export] private Area2D _exitArea;

    private int _openDoorsCount = 0;

    public override void _Ready()
    {
        _exitArea.BodyEntered += OnExitAreaBodyEntered;
    }
    public void OpenNextDoor()
    {
        if (_openDoorsCount >= _doors.Length)
            return;
        
        _doors[_openDoorsCount].Open();
        _openDoorsCount++;
    }

    private void OnExitAreaBodyEntered(Node2D body)
    {
        CharacterView character = body.FindCharacter();
        if (character == null)
            return;
        
        GD.Print("GAME WON!!!");
        GetTree().Quit();
    }
}