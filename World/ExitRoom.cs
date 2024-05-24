using DungeonAdventure.World.Rooms.Exit.Support;
using Godot;

namespace DungeonAdventure.World;

public partial class ExitRoom : Room
{
    [Export] private Exit _exit;
    public void OpenNextDoor()
    {
        _exit.OpenNextDoor();
    }
}