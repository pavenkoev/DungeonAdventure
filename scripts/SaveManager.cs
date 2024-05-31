using Godot;
using Ardot.SaveSystems;
using DungeonAdventure.Characters;
using DungeonAdventure.Characters.Views;
using DungeonAdventure.Utils;
using DungeonAdventure.World;

namespace DungeonAdventure.scripts;

/// <summary>
/// Manages the saving and loading of game data.
/// </summary>
public class SaveManager
{
	// private string filePath = "user://Save.txt";
	private string filePath = "res://saveFile/Save.json";

	/// <summary>
	/// Manages the saving and loading of game data.
	/// </summary>
	public void SaveGame(Node rootNode)
	{
		Dungeon dungeon = rootNode.FindNodeDown<Dungeon>();
		
		SaveAccess saveAccess = SaveAccess.Open(filePath);
		saveAccess.SaveObject(dungeon);
		saveAccess.Commit();
	}

	/// <summary>
	/// Loads the game state into the specified root node.
	/// </summary>
	/// <param name="rootNode">The root node to start loading into.</param>
	public void LoadGame(Node rootNode)
	{
		Dungeon dungeon = rootNode.FindNodeDown<Dungeon>();

		if (FileAccess.FileExists(filePath))
		{
			SaveAccess saveAccess = SaveAccess.Open(filePath);
			
			dungeon.Load(saveAccess.LoadData(dungeon.GetLoadKey()));
		}
		else
		{
			GD.PrintErr("Save file not found.");
		}
	}
}











