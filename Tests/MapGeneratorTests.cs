using System.Linq;
using DungeonAdventure.World;
using DungeonAdventure.World.Generation;
using GdUnit4;
using Godot;
using Room = DungeonAdventure.World.Generation.Room;

namespace DungeonAdventure.Tests;

[TestSuite]
public class MapGeneratorTests
{
    private static readonly RoomType[] SpecialRoomTypes =
    {
        RoomType.Start, RoomType.Exit,
        RoomType.PillarAbstraction, RoomType.PillarEncapsulation,
        RoomType.PillarInheritance, RoomType.PillarPolymorphism
    };
    
    private MapGenerator _generator;

    [BeforeTest]
    public void Setup()
    {
        _generator = new();
    }
    
    private static MapGenerationSettings GetMapGenerationSettings()
    {
        return GD.Load<MapGenerationSettings>("res://map_generation_settings.tres");
    }

    private void EnsureRoomTypes(Map map)
    {
        Assertions.AssertThat(map.StartingRoom.RoomType == RoomType.Start).IsTrue();

        // check that there is only one room of each type
        foreach (RoomType type in SpecialRoomTypes)
        {
            Assertions.AssertThat(map.Rooms.Values.Count(r => r.RoomType == type)).IsEqual(1);
        }

        // check that all other rooms are regular rooms
        Assertions.AssertThat(map.Rooms.Values.Count(r => r.RoomType == RoomType.Regular))
            .IsEqual(map.Rooms.Count - SpecialRoomTypes.Length);
    }

    private void DestroyMap(Map map)
    {
        foreach (Room room in map.Rooms.Values)
        {
            room.Node.QueueFree();
        }
    }

    [TestCase]
    public void Generate20Rooms()
    {
        MapGenerationSettings settings = GetMapGenerationSettings();
        settings.NumberOfRooms = 20;

        Map map = _generator.Generate(settings);

        Assertions.AssertThat(map.Rooms.Count).IsEqual(settings.NumberOfRooms);
        EnsureRoomTypes(map);
        
        DestroyMap(map);
    }
    
    [TestCase]
    public void GenerateLessRooms()
    {
        MapGenerationSettings settings = GetMapGenerationSettings();
        settings.NumberOfRooms = 3;

        Map map = _generator.Generate(settings);

        Assertions.AssertThat(map.Rooms.Count).IsEqual(SpecialRoomTypes.Length);
        EnsureRoomTypes(map);
        
        DestroyMap(map);
    }
}