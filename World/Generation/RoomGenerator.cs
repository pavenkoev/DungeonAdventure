using System;
using System.Collections.Generic;
using System.Linq;
using DungeonAdventure.Characters;
using DungeonAdventure.Items;
using DungeonAdventure.World.Placeholders;
using Godot;

namespace DungeonAdventure.World.Generation;

public class RoomGenerator
{
    private readonly MapGenerationSettings _mapGenerationSettings;
    private readonly Dictionary<RoomType, List<PackedScene>> _roomTemplatesByType = new();
    private readonly Random _random = new();

    public RoomGenerator(MapGenerationSettings settings)
    {
        _mapGenerationSettings = settings;
        
        LoadRoomTemplates(RoomType.Regular, GetRoomTemplateDirectory("Regular"));
        LoadRoomTemplates(RoomType.Start, GetRoomTemplateDirectory("Entrance"));
        LoadRoomTemplates(RoomType.Exit, GetRoomTemplateDirectory("Exit"));
        LoadRoomTemplates(RoomType.PillarAbstraction, GetRoomTemplateDirectory("PillarAbstraction"));
        LoadRoomTemplates(RoomType.PillarEncapsulation, GetRoomTemplateDirectory("PillarEncapsulation"));
        LoadRoomTemplates(RoomType.PillarPolymorphism, GetRoomTemplateDirectory("PillarPolymorphism"));
        LoadRoomTemplates(RoomType.PillarInheritance, GetRoomTemplateDirectory("PillarInheritance"));
    }

    private string GetRoomTemplateDirectory(string type)
    {
        return $"res://World/Rooms/{type}/";
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
        
        List<ItemPlaceholder> itemPlaceholders = placeholders
            .Where(p => p is ItemPlaceholder)
            .Cast<ItemPlaceholder>()
            .ToList();
        
        GenerateEnemies(room, enemyPlaceholders);
        GenerateItems(room, itemPlaceholders);
        
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
    
    private void GenerateItems(World.Room room, List<ItemPlaceholder> placeholders)
    {
        if (placeholders.Count == 0)
            return;

        foreach (ItemPlaceholder placeholder in placeholders)
        {
            if (_random.NextDouble() <= placeholder.Probability)
            {
                Item item = placeholder.ItemPool.GetRandomItem(_random);
                if (item == null)
                    continue;
                    
                ItemObject itemObject = _mapGenerationSettings.ItemObjectScene.Instantiate<ItemObject>();
                itemObject.Item = item;
            
                placeholder.GetParent().AddChild(itemObject);
                itemObject.Position = placeholder.Position;
            }
        }
    }
}