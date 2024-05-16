using Godot;
using Ardot.SaveSystems;

public partial class SaveManager : Node
{
	private string filePath = "saveFile/Save.txt";

	public void SaveGame(Node rootNode)
	{
		GD.Print("Saving game...");
		SaveAccess saveAccess = SaveAccess.Open(filePath);
		saveAccess.SaveTree(rootNode);
		saveAccess.Commit();
		GD.Print("Game saved.");
	}

	public void LoadGame(Node rootNode)
	{
		GD.Print("Loading game...");

		// Check if the save file exists
		if (FileAccess.FileExists(filePath))
		{
			GD.Print("Save file found. Loading...");
			SaveAccess saveAccess = SaveAccess.Open(filePath);
			saveAccess.LoadTree(rootNode);
			GD.Print("Game loaded.");
			
			InspectLoadedState(rootNode);
		}
		else
		{
			GD.PrintErr("Save file not found.");
		}
	}

	private void InspectLoadedState(Node rootNode)
	{
		// Add debug prints for key nodes or properties
		GD.Print("Inspecting loaded state...");
		foreach (Node node in rootNode.GetChildren())
		{
			GD.Print($"Node: {node.Name}, Type: {node.GetType().Name}");
		}

		// Example: Inspect a specific character node
		var character = rootNode.GetNode<CharacterBody2D>("Character");
		if (character != null)
		{
			GD.Print($"Character position: {character.Position}");
		}
	}
}



