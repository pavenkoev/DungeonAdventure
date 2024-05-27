using Godot;

namespace DungeonAdventure.World.Rooms.Exit.Support;

/// <summary>
/// Represents an exit room in the dungeon.
/// </summary>
public partial class ExitRoom : Room
{
    /// <summary>
    /// The exit associated with this exit room.
    /// </summary>
    [Export] private Exit _exit;
    
    /// <summary>
    /// Opens the next door in the exit sequence.
    /// </summary>
    public void OpenNextDoor()
    {
        _exit.OpenNextDoor();
    }
}