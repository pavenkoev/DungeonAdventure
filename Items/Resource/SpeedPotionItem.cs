using DungeonAdventure.Characters;
using Godot;

namespace DungeonAdventure.Items;

[Tool]
[GlobalClass]
public partial class SpeedPotionItem : Item
{
    [Export] private float _factor;
    public override void Use(Character character)
    {
    }
}