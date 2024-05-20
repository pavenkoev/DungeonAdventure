using DungeonAdventure.Characters;
using DungeonAdventure.Characters.Effects;
using Godot;

namespace DungeonAdventure.Items;

[Tool]
[GlobalClass]
public partial class SpeedPotionItem : Item
{
    [Export] private float _factor = 1.2f;
    [Export] private float _duration = 5f;
    public override void Use(Character character)
    {
        character.AddEffect(new SpeedEffect(_factor, _duration));
    }
}