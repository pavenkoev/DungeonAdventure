using Godot;
using System;

public class ItemIcon : TextureRect
{
    [Export]
    public Texture IconTexture { get; set; }

    [Export]
    public string ItemName { get; set; }

    [Export]
    public int MaxUses { get; set; } = 1;

    private int currentUses;

    public override void _Ready()
    {
        Texture = IconTexture;
        currentUses = MaxUses;
    }

    public void UseItem()
    {
        if (currentUses > 0)
        {
            GD.Print("Using " + ItemName);
            // Implement item use logic here
            currentUses--;
            GD.Print("Remaining uses: " + currentUses);
            if (currentUses <= 0)
            {
                GD.Print(ItemName + " depleted.");
                QueueFree(); // Remove the item icon from the scene
            }
        }
        else
        {
            GD.Print(ItemName + " has no uses left.");
        }
    }

    public void RefillUses()
    {
        currentUses = MaxUses;
        GD.Print("Refilled " + ItemName + " uses.");
    }
}
