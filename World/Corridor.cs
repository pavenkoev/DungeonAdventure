using System;
using Godot;

namespace DungeonAdventure.World;

public partial class Corridor : Node
{
    public void GeneratePath(Vector2 start, Vector2 end)
    {
        var tileMap = GetNode<TileMap>("res://World/Base/corridor_base.tscn");
        // logic for a straight line
        int x0 = (int)start.X, y0 = (int)start.Y;
        int x1 = (int)end.X, y1 = (int)end.Y;

        // Bresenham's line algorithm to set tiles
        int dx = Math.Abs(x1 - x0), sx = x0 < x1 ? 1 : -1;
        int dy = -Math.Abs(y1 - y0), sy = y0 < y1 ? 1 : -1;
        int err = dx + dy, e2;

        for (;;)
        {
            tileMap.SetCell(x0, y0, 1);
            if (x0 == x1 && y0 == y1) break;
            e2 = 2 * err;
            if (e2 >= dy) { err += dy; x0 += sx; }
            if (e2 <= dx) { err += dx; y0 += sy; }
        }
    }
}