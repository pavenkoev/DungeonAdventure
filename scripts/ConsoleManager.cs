using DungeonAdventure.scripts;
using Godot;

/*
 * Class to manage the console UI. To use the console for save and load in game you need to press '\'
 * delete the backspace and type 'save' and press enter to unpause the game.
 * to load press the '\' and delete the backslash then type 'load' and press enter.
 */
public partial class ConsoleManager : Node
{
	private PackedScene _consoleScene;
	private ConsoleUI _consoleUiInstance;
	private SaveManager _saveManager;

	public override void _Ready()
	{
		_saveManager = new SaveManager();

		_consoleScene = GD.Load<PackedScene>("res://scripts/console.tscn");
		if (_consoleScene != null)
		{
			GD.Print("console.tscn loaded successfully");
			_consoleUiInstance = _consoleScene.Instantiate<ConsoleUI>();
			if (_consoleUiInstance != null)
			{
				AddChild(_consoleUiInstance);
				_consoleUiInstance.Hide();
			}
			else
			{
				GD.PrintErr("Failed to instantiate console.tscn");
			}
		}
		else
		{
			GD.PrintErr("Failed to load console.tscn");
		}

		// Get command line arguments
		var args = OS.GetCmdlineArgs();
		foreach (var arg in args)
		{
			if (arg == "--save-game")
			{
				SaveGame();
				GetTree().Quit();
			}
			else if (arg == "--load-game")
			{
				LoadGame();
			}
		}
	}

	public override void _Input(InputEvent @event)
	{
		if (_consoleUiInstance == null)
		{
			GD.Print("Console instance is null");
			return;
		}

		if (@event is InputEventKey eventKey && eventKey.Pressed && eventKey.Keycode == Key.Backslash)
		{
			// Toggle console UI on '\' key press
			GD.Print("Toggling ConsoleUI visibility");
			_consoleUiInstance.ToggleVisibility();
		}
	}

	public void HandleConsoleCommand(string command)
	{
		switch (command.ToLower())
		{
			case "save":
				SaveGame();
				break;
			case "load":
				LoadGame();
				break;
			default:
				GD.Print($"Unknown command: {command}");
				break;
		}

		// Unpause the game if console is still open
		if (_consoleUiInstance.Visible)
		{
			_consoleUiInstance.ToggleVisibility();
		}
	}

	private void SaveGame()
	{
		GD.Print("Saving game...");
		_saveManager.SaveGame(GetTree().Root);
		GD.Print("Game saved.");
	}

	private void LoadGame()
	{
		GD.Print("Loading game...");
		_saveManager.LoadGame(GetTree().Root);
		GD.Print("Game loaded.");
	}
}


