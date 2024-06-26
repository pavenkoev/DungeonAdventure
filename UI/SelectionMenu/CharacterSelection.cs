using Godot;
using System;
using DungeonAdventure;
using DungeonAdventure.Characters.Models;
using DungeonAdventure.Characters;

public partial class CharacterSelection : Node2D
{
	private CharacterModelFactoryManual _characterFactory;
	private Node _characterDisplayNode;

	[Export] private OptionButton _difficultyButton;
	
	public override void _Ready()
	{
		// Assuming you have nodes for displaying character infor
		_characterDisplayNode = GetNode("CharacterDisplay"); // CChange the path to your character display node
	}
	private void _on_option_button_pressed()
	{
		// Replace with function body.
	}

	private void _on_rogue_button_pressed()
	{
		StartGame("rogue");
	}

	private void _on_knight_button_pressed()
	{
		StartGame("knight");
	}

	private void _on_wizard_button_pressed()
	{
		StartGame("wizard");
	}

	private void StartGame(string character)
	{
		Game.Instance.CharacterModelName = character;
		Game.Instance.Difficulty = GetDifficulty();
		TransitionToMainScene();
	}

	private Difficulty GetDifficulty()
	{
		if (_difficultyButton.Selected < 0)
			return Difficulty.Medium;

		switch (_difficultyButton.GetItemText(_difficultyButton.Selected).ToLower())
		{
			case "easy":
				return Difficulty.Easy;
			case "hard":
				return Difficulty.Hard;
		}

		return Difficulty.Medium;
	}

	private void DisplayCharacter(CharacterModel characterModel)
	{
		// Logic to display the character on the screen
		// This could involve updating sprites, labels, etc.
		// Example: _characterDisplayNode.Call("UpdateCharacterDisplay", characterModel);
	}

	private void TransitionToMainScene()
	{
		// Transition to the main game scene
		GetTree().ChangeSceneToFile("res://main.tscn");
	}
}


