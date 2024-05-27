using Godot;

namespace DungeonAdventure.Utils;

/// <summary>
/// Provides the current time in milliseconds.
/// </summary>
public class TimeProvider : ITimeProvider
{
    /// <summary>
    /// Gets the current time in milliseconds.
    /// </summary>
    /// <returns>The current time in milliseconds.</returns>
    public ulong GetTicksMs()
    {
        return Time.GetTicksMsec();
    }
}