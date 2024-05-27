using Godot;

namespace DungeonAdventure.Utils;

public class TimeProvider : ITimeProvider
{
    public ulong GetTicksMs()
    {
        return Time.GetTicksMsec();
    }
}