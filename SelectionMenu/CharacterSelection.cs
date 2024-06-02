using Godot;
using System;
using DungeonAdventure.Characters.Models;
using DungeonAdventure.Characters;

public partial class CharacterSelection : Node2D
{
	private CharacterModelFactoryManual _characterFactory;
	private Node _characterDisplayNode;

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

		TransitionToMainScene();
	}

	private void _on_knight_button_pressed()
	{

		TransitionToMainScene();
	}

	private void _on_wizard_button_pressed()
	{
		TransitionToMainScene();
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


