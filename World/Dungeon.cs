using Godot;
using System;

namespace DungeonAdventure.World;

public partial class Dungeon : Node2D
{
    private int width = 50;
    private int height = 50;
    private int[,] map;

    public override void _Ready()
    {
        GenerateDungeon();
        QueueRedraw();
    }

    private void GenerateDungeon()
    {
        map = new int[width, height];
        Random random = new Random();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (x == 0 || y == 0 || x == width - 1 || y == height - 1)
                {
                    map[x, y] = 0;
                }
                else
                {
                    map[x, y] = random.Next(0, 100) < 45 ? 0 : 1;
                }
            }
        }
    }

    public override void _Draw()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector2 position = new Vector2(x * 16, y * 16); // Assuming 16x16 tiles
                if (map[x, y] == 1)
                {
                    DrawRect(new Rect2(position, new Vector2(16, 16)), Colors.DarkGray, false);
                }
                else
                {
                    DrawRect(new Rect2(position, new Vector2(16, 16)), Colors.Black, false);
                }
            }
        }
    }

    public void Regenerate()
    {
        GenerateDungeon();
        QueueRedraw();
    }
}