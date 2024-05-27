using DungeonAdventure.Utils;

namespace DungeonAdventure.Tests;

public class TimeProviderMock : ITimeProvider
{
    public double Time { get; set; } = 0;

    public TimeProviderMock(double time)
    {
        Time = time;
    }
    
    public ulong GetTicksMs()
    {
        return (ulong)(Time * 1000.0);
    }
}