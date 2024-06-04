using Godot;
using System;

public partial class MainMenu : Control
{
	// Config file, move it into a singleton
	private ConfigFile settingsFile = new ConfigFile();

	private int vsync = 0;

	// Audio settings represented as a Vector3
	private Vector3 audio = new Vector3(70.0f, 70.0f, 70.0f);

	private OptionButton resolutionOptionButton;
	private Control optionContainer;
	private Control mainContainer;

	public override void _Ready()
	{
		resolutionOptionButton = GetNode<OptionButton>("%Resolution_Optionbutton");
		optionContainer = GetNode<Control>("%OptionContainer");
		mainContainer = GetNode<Control>("%MainContainer");

		LoadSettings();
		resolutionOptionButton.Select(CheckResolution(DisplayServer.ScreenGetSize()));
	}

	private Vector2I GetResolution(int index)
	{
		var resolutionArr = resolutionOptionButton.GetItemText(index).Split("x");
		return new Vector2I(int.Parse(resolutionArr[0]), int.Parse(resolutionArr[1]));
	}

	private int CheckResolution(Vector2I resolution)
	{
		for (int i = 0; i < resolutionOptionButton.ItemCount; i++)
		{
			if (GetResolution(i) == resolution)
			{
				return i;
			}
		}
		return -1;
	}

	private void FirstTime()
	{
		DisplayServer.WindowSetSize(DisplayServer.ScreenGetSize());
		DisplayServer.WindowSetMode(DisplayServer.WindowMode.Maximized);
		//DisplayServer.WindowSetVsyncMode((DisplayServer.VsyncMode)vsync);
		resolutionOptionButton.Select(CheckResolution(DisplayServer.ScreenGetSize()));
		
		// -- Video
		settingsFile.SetValue("VIDEO", "Resolution", GetResolution(resolutionOptionButton.Selected));
		settingsFile.SetValue("VIDEO", "vsync", vsync);
		settingsFile.SetValue("VIDEO", "Window Mode", GetResolution(resolutionOptionButton.Selected));
		settingsFile.SetValue("VIDEO", "Difficulty", GetResolution(resolutionOptionButton.Selected));
		settingsFile.SetValue("VIDEO", "Color blind", GetResolution(resolutionOptionButton.Selected));
		
		// -- audio
		settingsFile.SetValue("audio", "General", audio.X);
		settingsFile.SetValue("audio", "Music", audio.Y);
		settingsFile.SetValue("audio", "SFX", audio.Z);

		settingsFile.Save("res://settings.cfg");
	}

	private void LoadSettings()
	{
		if (settingsFile.Load("res://settings.cfg") != Error.Ok)
		{
			FirstTime();
		}
	}

	private void SaveSettings()
	{
		// -- Video
		settingsFile.SetValue("VIDEO", "Resolution", GetResolution(resolutionOptionButton.Selected));
		settingsFile.SetValue("VIDEO", "vsync", vsync);
		settingsFile.SetValue("VIDEO", "Window Mode", GetResolution(resolutionOptionButton.Selected));
		settingsFile.SetValue("VIDEO", "Difficulty", GetResolution(resolutionOptionButton.Selected));
		settingsFile.SetValue("VIDEO", "Color blind", GetResolution(resolutionOptionButton.Selected));
		
		// -- audio
		settingsFile.SetValue("audio", "General", audio.X);
		settingsFile.SetValue("audio", "Music", audio.Y);
		settingsFile.SetValue("audio", "SFX", audio.Z);

		settingsFile.Save("res://settings.cfg");
	}

	private void OnStartButtonPressed()
	{
//		GetTree().ChangeSceneToFile("res://main.tscn");
		GetTree().ChangeSceneToFile("res://UI/SelectionMenu/CharacterSelection.tscn");
		
	}

	private void OnOptionButtonPressed()
	{
		optionContainer.Visible = true;
		mainContainer.Visible = false;
	}

	private void OnExitButtonPressed()
	{
		GetTree().Quit();
	}

	// -- VIDEO TAB --

	private void OnResolutionOptionButtonItemSelected(int index)
	{
		DisplayServer.WindowSetSize(GetResolution(index));
	}

	private void OnRoomModeOptionButtonItemSelected(int index)
	{
		// Replace with function body.
	}

	private void OnPresetHSliderValueChanged(float value)
	{
		// Replace with function body.
	}

	// -- audio TAB --

		private void OnGeneralHScrollBarValueChanged(double value)
	{
		audio = new Vector3((float)value, audio.Y, audio.Z);
	}

	private void OnMusicHScrollBarValueChanged(double  value)
	{
		audio = new Vector3(audio.X, (float)value, audio.Z);
	}

	private void OnSfxHScrollBarValueChanged(double value)
	{
		audio = new Vector3(audio.X, audio.Y, (float)value);
	}

	// -- Save and Exit buttons --

	private void OnReturnButtonPressed()
	{
		mainContainer.Visible = true;
		optionContainer.Visible = false;
	}

	private void OnApplyButtonPressed()
	{
		mainContainer.Visible = true;
		optionContainer.Visible = false;
		SaveSettings();
	}

	private void OnCreditsButtonPressed()
	{
		OS.ShellOpen("https://github.com/pavenkoev/DungeonAdventure");
	}
}





