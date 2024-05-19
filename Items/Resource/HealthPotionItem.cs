using DungeonAdventure.Characters;
using Godot;

namespace DungeonAdventure.Items;

[Tool]
[GlobalClass]
public partial class HealthPotionItem : Item
{
    [Export] private int _value;
    public override void Use(Character character)
    {
        character.Heal(_value);
    }
}