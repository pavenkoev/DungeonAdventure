using Godot;
using Ardot.SaveSystems;
using DungeonAdventure.Characters;
using DungeonAdventure.Characters.Views;

namespace DungeonAdventure.scripts;

public partial class SaveManager : Node
{
	private string filePath = "user://Save.txt";

	public void SaveGame(Node rootNode)
	{
		SaveAccess saveAccess = SaveAccess.Open(filePath);
		saveAccess.SaveTree(rootNode);
		saveAccess.Commit();
	}

	public void LoadGame(Node rootNode)
	{

		if (FileAccess.FileExists(filePath))
		{
			SaveAccess saveAccess = SaveAccess.Open(filePath);
			saveAccess.LoadTree(rootNode);
			foreach (Node node in rootNode.GetChildren())
			{
				if (node is CharacterView character)
				{
					// CharacterInRoom(character);
				}
			}
		}
		else
		{
			GD.PrintErr("Save file not found.");
		}
	}

	// private void CharacterInRoom(CharacterView character)
	// {
	// 	if (string.IsNullOrEmpty(character.CurrentRoom))
	// 	{
	// 		return;
	// 	}
	//
	// 	Node roomNode = GetRoomNode(character.CurrentRoom);
	// 	if (roomNode != null)
	// 	{
	// 		if (character.GetParent() != roomNode)
	// 		{
	// 			character.GetParent().RemoveChild(character);
	// 			roomNode.AddChild(character);
	// 		}
	// 		character.GlobalPosition = character.GlobalPosition; // Ensure position is retained
	// 	}
	// }

	private Node GetRoomNode(string roomName)
	{
		return FindRoomNode(GetTree().Root, roomName);
	}

	private Node FindRoomNode(Node node, string roomName)
	{
		if (node.Name == roomName)
		{
			return node;
		}

		foreach (Node child in node.GetChildren())
		{
			Node foundNode = FindRoomNode(child, roomName);
			if (foundNode != null)
			{
				return foundNode;
			}
		}

		return null;
	}
	
}











