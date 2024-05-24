using DungeonAdventure.Characters;
using DungeonAdventure.Characters.Effects;
using Godot;

namespace DungeonAdventure.Items;

[Tool]
[GlobalClass]
public partial class HealthPotionItem : Item
{
    [Export] private int _value;
    public override void Use(Character character)
    {
        character.Heal(_value, 1);
    }
}