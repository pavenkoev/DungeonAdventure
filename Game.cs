namespace DungeonAdventure;

public class Game
{
    public static Game Instance { get; } = new();

    public string CharacterModelName { get; set; } = "Wizard";
    public bool LoadGame { get; set; } = false;
    public Difficulty Difficulty { get; set; } = Difficulty.Medium;
}