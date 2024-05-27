namespace DungeonAdventure;

/// <summary>
/// Represents an interface for objects that can be paused and resumed.
/// </summary>
public interface IPausable
{
    /// <summary>
    /// Pauses the object.
    /// </summary>
    public void Pause();
    
    /// <summary>
    /// Resumes the object.
    /// </summary>
    public void Resume();
}