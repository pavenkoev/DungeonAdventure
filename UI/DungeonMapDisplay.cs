using DungeonAdventure.Utils;
using DungeonAdventure.World;
using DungeonAdventure.World.Generation;
using Godot;
using Room = DungeonAdventure.World.Generation.Room;

namespace DungeonAdventure.UI;

/// <summary>
/// Represents the display for the dungeon map in the UI.
/// Handles the rendering of the dungeon map, including player position and special rooms.
/// </summary>
public partial class DungeonMapDisplay : Control
{
    private Dungeon _dungeon;

    [Export] private Font _font;

    /// <summary>
    /// Called when the node is added to the scene.
    /// Initializes the dungeon reference.
    /// </summary>
    public override void _Ready()
    {
        _dungeon = GetTree().Root.FindNodeDown<Dungeon>();
    }

    /// <summary>
    /// Called every frame.
    /// If the control is visible, queues a redraw of the map.
    /// </summary>
    /// <param name="delta">The frame time.</param>
    public override void _Process(double delta)
    {
        if (Visible)
            QueueRedraw();
    }

    /// <summary>
    /// Called when the control needs to be drawn.
    /// Handles the rendering of the dungeon map, including player position and special rooms.
    /// </summary>
    public override void _Draw()
    {
        float paddingFactor = 0.95f;

        Map map = _dungeon.Map;

        Vector2I? playerRoom = null;
        if (_dungeon.Player != null)
            playerRoom = _dungeon.Player.FindRoom().Coordinates;

        Rect2 rect = new();

        foreach (Vector2I coord in map.Rooms.Keys)
            rect = rect.Expand(coord);

        float xCellSize = (Size.X * paddingFactor) / (rect.Size.X + 1);
        float yCellSize = (Size.Y * paddingFactor) / (rect.Size.Y + 1);
        float cellSize = Mathf.Min(xCellSize, yCellSize);

        Vector2 paddingOffset = new Vector2(
            (Size.X - (rect.Size.X + 1) * cellSize) / 2,
            (Size.Y - (rect.Size.Y + 1) * cellSize) / 2
        );

        foreach (Room room in map.Rooms.Values)
        {
            Vector2 roomPos = new Vector2(
                (room.Coordinates.X - rect.Position.X) * cellSize,
                (room.Coordinates.Y - rect.Position.Y) * cellSize
            ) + paddingOffset;

            Rect2 r = new Rect2(roomPos, new Vector2(cellSize * 0.9f, cellSize * 0.9f));
            DrawRect(r, new Color(0.3f, 0.3f, 0.3f, 0.8f));

            if (room.Coordinates == playerRoom)
            {
                DrawCircle(r.Position + r.Size / 2, cellSize / 4, new Color(0, 0.8f, 0, 0.8f));
            }

            if (room.RoomType != RoomType.Regular)
            {
                DrawString(_font, r.Position + r.Size / 2, RoomTypeToString(room.RoomType), 
                    modulate: new Color(0.8f, 0, 0, 0.8f));
            }
        }
    }
    
    /// <summary>
    /// Converts a room type to a string representation.
    /// </summary>
    /// <param name="type">The room type to convert.</param>
    /// <returns>The string representation of the room type.</returns>
    private static string RoomTypeToString(RoomType type)
    {
        return type switch
        {
            RoomType.Regular => "*",
            RoomType.Start => "i",
            RoomType.Exit => "o",
            RoomType.PillarAbstraction => "A",
            RoomType.PillarPolymorphism => "P",
            RoomType.PillarEncapsulation => "E",
            RoomType.PillarInheritance => "I"
        };
    }
}