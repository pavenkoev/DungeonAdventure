using DungeonAdventure.Characters;
using DungeonAdventure.Utils;
using DungeonAdventure.World;
using Godot;

namespace DungeonAdventure.Items;

[Tool]
[GlobalClass]

public partial class KeyItem : Item
{
    public override void Use(Character character)
    {
        ExitRoom room = character.FindRoom() as ExitRoom;
        if (room == null)
            return;
        
        room.OpenNextDoor();
    }

    public override bool CanUse(Character character)
    {
        Room room = character.FindRoom();
        return room is ExitRoom;
    }
}