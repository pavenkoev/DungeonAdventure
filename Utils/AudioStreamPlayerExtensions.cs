using Godot;

namespace DungeonAdventure.Utils;

public static class AudioStreamPlayerExtensions
{
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