using Godot;

namespace DungeonAdventure.Utils;

/// <summary>
/// Provides extension methods for <see cref="AudioStreamPlayer2D"/> to play random sounds.
/// </summary>
public static class AudioStreamPlayerExtensions
{
    /// <summary>
    /// Plays a random sound from the provided array of audio streams.
    /// </summary>
    /// <param name="player">The audio stream player.</param>
    /// <param name="clips">The array of audio streams to choose from.</param>
    public static void PlayRandomSound(this AudioStreamPlayer2D player, AudioStream[] clips)
    {
        if (player == null || clips == null || clips.Length == 0)
        {
            return;
        }

        AudioStreamPlayback playback = player.GetStreamPlayback();
        int index = (int)(GD.Randi() % clips.Length);

        if (playback is AudioStreamPlaybackPolyphonic playbackPolyphonic)
            playbackPolyphonic.PlayStream(clips[index]);
        else
        {
            player.Stream = clips[index];
            player.Play();
        }
    }
}