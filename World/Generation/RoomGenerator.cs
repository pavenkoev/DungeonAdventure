using System;
using System.Collections.Generic;
using System.Linq;
using DungeonAdventure.Characters;
using DungeonAdventure.World.Placeholders;
using Godot;

namespace DungeonAdventure.World.Generation;

public class RoomGenerator
{
    private MapGenerationSettings _mapGenerationSettings;
    private Dictionary<RoomType, List<PackedScene>> _roomTemplatesByType = new();
    private Random _random = new();

    public RoomGenerator(MapGenerationSettings settings)
    {
        _mapGenerationSettings = settings;
        
        LoadRoomTemplates(RoomType.Regular, "res://World/Rooms/Regular/");
        LoadRoomTemplates(RoomType.Start, "res://World/Rooms/Entrance/");
        LoadRoomTemplates(RoomType.Exit, "res://World/Rooms/Exit/");
        LoadRoomTemplates(RoomType.PillarAbstraction, "res://World/Rooms/PillarAbstraction/");
        LoadRoomTemplates(RoomType.PillarEncapsulation, "res://World/Rooms/PillarEncapsulation/");
        LoadRoomTemplates(RoomType.PillarPolymorphism, "res://World/Rooms/PillarPolymorphism/");
        LoadRoomTemplates(RoomType.PillarInheritance, "res://World/Rooms/PillarInheritance/");
    }

    private void LoadRoomTemplates(RoomType roomType, string directoryPath)
    {
        List<PackedScene> scenes = new();
        DirAccess directory = DirAccess.Open(directoryPath);

        string[] files = directory.GetFiles();
        foreach (string file in files)
        {
            string sceneFilePath = directoryPath + file;
            PackedScene scene = GD.Load<PackedScene>(sceneFilePath);
            
            if (scene != null) 
                scenes.Add(scene);
        }

        _roomTemplatesByType[roomType] = scenes;
    }
    public Room GenerateRoom(Vector2I coordinates, RoomType roomType)
    {
        List<PackedScene> scenes = _roomTemplatesByType[roomType];
        PackedScene scene = scenes[_random.Next(scenes.Count)];

        World.Room room = scene.Instantiate<World.Room>();

        List<Placeholder> placeholders = CollectPlaceholders(room);

        List<EnemyPlaceholder> enemyPlaceholders = placeholders
            .Where(p => p is EnemyPlaceholder)
            .Cast<EnemyPlaceholder>()
            .ToList();
        
        GenerateEnemies(room, enemyPlaceholders);
        
        RemovePlaceholders(placeholders);
        
        return new Room(roomType, coordinates, room);
    }

    private List<Placeholder> CollectPlaceholders(World.Room room)
    {
        List<Placeholder> placeholders = new();
        for (int i = 0; i < room.GetChildCount(); i++)
        {
            Node node = room.GetChild(i);
            if (node is Placeholder placeholder)
                placeholders.Add(placeholder);
        }
        
        return placeholders;
    }

    private void RemovePlaceholders(List<Placeholder> placeholders)
    {
        foreach (Placeholder placeholder in placeholders)
        {
            placeholder.GetParent().RemoveChild(placeholder);
            placeholder.QueueFree();
        }
    }

    private void GenerateEnemies(World.Room room, List<EnemyPlaceholder> placeholders)
    {
        if (placeholders.Count == 0)
            return;
        
        int numOfEnemies = _random.Next(1, placeholders.Count + 1);

        for (int i = 0; i < numOfEnemies; i++)
        {
            EnemyPlaceholder placeholder = placeholders[i];
            
            int enemyIndex = _random.Next(_mapGenerationSettings.Enemies.Length);
            PackedScene enemyScene = _mapGenerationSettings.Enemies[enemyIndex];

            Character enemy = enemyScene.Instantiate<Character>();
            placeholder.GetParent().AddChild(enemy);
            enemy.Position = placeholder.Position;

        }
    }
}