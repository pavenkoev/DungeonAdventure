namespace DungeonAdventure.scripts;
using Godot;

public partial class ConsoleUI : Control
{
	private LineEdit _lineEdit;

	public override void _Ready()
	{
		GD.Print("ConsoleUI _Ready");

		_lineEdit = GetNode<LineEdit>("LineEdit");
		_lineEdit.Connect("text_submitted", new Callable(this, nameof(OnTextSubmitted)));

		// Set PauseMode for console
		ProcessMode = ProcessModeEnum.Always;
	}

	private void OnTextSubmitted(string text)
	{
		// Process the command
		ConsoleManager.Instance.HandleConsoleCommand(text);
		_lineEdit.Clear();
	}

	public void ToggleVisibility()
	{
		Visible = !Visible;
		if (Visible)
		{
			GD.Print("ConsoleUI is now visible");
			_lineEdit.GrabFocus();
			GetTree().Paused = true; // Pause the game
		}
		else
		{
			GD.Print("ConsoleUI is now hidden");
			GetTree().Paused = false; // Unpause the game
		}
	}
}










