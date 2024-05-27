namespace DungeonAdventure.Utils;

/// <summary>
/// Provides an interface for getting the current time in milliseconds.
/// </summary>
public interface ITimeProvider
{
    /// <summary>
    /// Gets the current time in milliseconds.
    /// </summary>
    /// <returns>The current time in milliseconds.</returns>
    public ulong GetTicksMs();
}