using Godot;
using System;

public partial class Scroll_Label : Label
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void _on_general_h_scroll_bar_value_changed(double value)
	{
//		Text = $"General Volume: {value}";
		Text = $"{value}%";

	}

	private void _on_music_h_scroll_bar_value_changed(double value)
	{
//		Text = $"Music Volume: {value}";
			Text = $"{value}%";
	}

	private void _on_sfx_h_scroll_bar_value_changed(double value)
	{
//		Text = $"SFX Volume: {value}";
		Text = $"{value}%";
	}
}
