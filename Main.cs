using DungeonAdventure.Characters;
using DungeonAdventure.Characters.Controllers;
using DungeonAdventure.Characters.Models;
using DungeonAdventure.Characters.Views;
using DungeonAdventure.scripts;
using DungeonAdventure.Utils;
using Godot;

namespace DungeonAdventure;

public partial class Main : Node2D
{
    [Export] private CharacterView _player;
    
    public override void _EnterTree()
    {
        if (!Game.Instance.LoadGame)
        {
            CharacterData data = new();
            CharacterModel model = data.LoadCharacter(Game.Instance.CharacterModelName);

            _player.ModelFactory = null;
            _player.ControllerFactory = null;
            _player.Model = model;
            _player.Controller = new PlayerController(_player, model);
        }
    }

    public override void _Ready()
    {
        if (Game.Instance.LoadGame)
        {
            SaveManager saveManager = new();
            saveManager.LoadGame(GetTree().Root);
        }
    }
}