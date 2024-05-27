using DungeonAdventure.Characters;
using DungeonAdventure.Characters.Views;
using DungeonAdventure.Utils;
using DungeonAdventure.World;
using Godot;
using ExitRoom = DungeonAdventure.World.Rooms.Exit.Support.ExitRoom;

namespace DungeonAdventure.Items;

/// <summary>
/// Represents a key item that can be used to open the next door in an exit room.
/// </summary>
[Tool]
[GlobalClass]
public partial class KeyItem : Item
{
    /// <summary>
    /// Uses the key item. If the character is in an exit room, opens the next door.
    /// </summary>
    /// <param name="character">The character on which to use the item.</param>
    public override void Use(CharacterView character)
    {
        ExitRoom room = character.FindRoom() as ExitRoom;
        if (room == null)
            return;
        
        room.OpenNextDoor();
    }

    /// <summary>
    /// Determines whether the key item can be used. The item can be used if the character is in an exit room.
    /// </summary>
    /// <param name="character">The character to check.</param>
    /// <returns>True if the character is in an exit room, otherwise false.</returns>

    public override bool CanUse(CharacterView character)
    {
        Room room = character.FindRoom();
        return room is ExitRoom;
    }
}