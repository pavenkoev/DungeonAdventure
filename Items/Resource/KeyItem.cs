using DungeonAdventure.Characters;
using DungeonAdventure.Characters.Views;
using DungeonAdventure.Utils;
using DungeonAdventure.World;
using Godot;

namespace DungeonAdventure.Items;

[Tool]
[GlobalClass]

public partial class KeyItem : Item
{
    public override void Use(CharacterView character)
    {
        ExitRoom room = character.FindRoom() as ExitRoom;
        if (room == null)
            return;
        
        room.OpenNextDoor();
    }

    public override bool CanUse(CharacterView character)
    {
        Room room = character.FindRoom();
        return room is ExitRoom;
    }
}