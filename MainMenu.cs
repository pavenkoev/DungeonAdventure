using Godot;
using System;

partial class MainMenu : Control
{
	public override void _Ready()
	{
				GD.Print("Hello from C# to Godot :)");

		GetNode<Button>("NewGameButton").Connect("pressed", new Callable(this, nameof(OnNewGamePressed)));
		GetNode<Button>("ContinueGameButton").Connect("pressed", new Callable(this, nameof(OnContinueGamePressed)));
		GetNode<Button>("OptionsButton").Connect("pressed", new Callable(this, nameof(OnOptionsPressed)));
		GetNode<Button>("CreditsButton").Connect("pressed", new Callable(this, nameof(OnCreditsPressed)));
		GetNode<Button>("QuitButton").Connect("pressed", new Callable(this, nameof(OnQuitPressed)));
	}


	private void OnNewGamePressed()
	{
		GD.Print("New Game Started");
		// Implement loading new game scene
	}

	private void OnContinueGamePressed()
	{
		GD.Print("Continue Game");
		// Implement loading existing game
	}

	private void OnOptionsPressed()
	{
		GD.Print("Options");
		// Implement options logic
	}

	private void OnCreditsPressed()
	{
		GD.Print("Credits");
		// Implement showing credits
	}
	private void OnQuitPressed()
	{
		GD.Print("Attempting to quit...");  // Debug statement
		GetTree().Quit();gi
	}



}