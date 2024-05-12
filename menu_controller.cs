using Godot;
using System;

public class SettingsControl : Control
{
	private ConfigFile settingsFile = new ConfigFile();
	private int vsync = 0;
	private Vector3 audio = new Vector3(70f, 70f, 70f);

	private OptionButton resolutionOptionButton;
	private Control optionContainer;
	private Control mainContainer;

	public override void _Ready()
	{
		resolutionOptionButton = GetNode<OptionButton>("%Resolution_OptionButton");
		optionContainer = GetNode<Control>("%OptionContainer");
		mainContainer = GetNode<Control>("%MainContainer");

		LoadSettings();
		int index = CheckResolution(OS.WindowSize);
		resolutionOptionButton.Select(index);
	}

	private Vector2i GetResolution(int index)
	{
		var resolutionText = resolutionOptionButton.GetItemText(index);
		var parts = resolutionText.Split('x');
		return new Vector2i(Convert.ToInt32(parts[0]), Convert.ToInt32(parts[1]));
	}

	private int CheckResolution(Vector2i resolution)
	{
		for (int i = 0; i < resolutionOptionButton.GetItemCount(); i++)
		{
			if (GetResolution(i) == resolution)
				return i;
		}
		return -1; // Not found
	}

	private void FirstTime()
	{
		OS.WindowSize = OS.GetScreenSize();
		OS.WindowMaximized = true;
		OS.SetVsyncMode((VsyncMode)vsync);
		int index = CheckResolution(OS.WindowSize);
		resolutionOptionButton.Select(index);

		// Save initial settings
		SaveSettings();
	}

	private void LoadSettings()
	{
		var result = settingsFile.Load("res://settings.cfg");
		if (result != Error.Ok)
			FirstTime();
	}

	private void SaveSettings()
	{
		settingsFile.SetValue("VIDEO", "Resolution", GetResolution(resolutionOptionButton.SelectedIndex).ToString());
		settingsFile.SetValue("VIDEO", "vsync", vsync);
		settingsFile.SetValue("AUDIO", "General", audio.x);
		settingsFile.SetValue("AUDIO", "Music", audio.y);
		settingsFile.SetValue("AUDIO", "SFX", audio.z);

		settingsFile.Save("res://settings.cfg");
	}

	private void OnStartButtonPressed()
	{
		// Implementation needed
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

	private void OnResolutionOptionButtonItemSelected(int index)
	{
		OS.WindowSize = GetResolution(index);
	}

	private void OnWindowModeOptionButtonItemSelected(int index)
	{
		// Placeholder for implementation
	}

	private void OnPresetHSliderValueChanged(float value)
	{
		// Placeholder for implementation
	}

	private void OnGeneralHScrollBarValueChanged(float value)
	{
		audio.x = value;
	}

	private void OnMusicHScrollBarValueChanged(float value)
	{
		audio.y = value;
	}

	private void OnSfxHScrollBarValueChanged(float value)
	{
		audio.z = value;
	}

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

	private void OnVsyncOptionButtonItemSelected(int index)
	{
		vsync = index;
		OS.SetVsyncMode((VsyncMode)vsync);
	}
}
