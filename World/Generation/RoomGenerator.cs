using System;
using System.Collections.Generic;
using System.Linq;
using DungeonAdventure.Characters;
using DungeonAdventure.Characters.Views;
using DungeonAdventure.Items;
using DungeonAdventure.World.Placeholders;
using Godot;
using Item = DungeonAdventure.Items.Item;
using ItemView = DungeonAdventure.Items.View.ItemView;

namespace DungeonAdventure.World.Generation;

/// <summary>
/// Responsible for generating rooms in the dungeon based on provided settings.
/// </summary>
public class RoomGenerator
{
    private readonly MapGenerationSettings _mapGenerationSettings;
    private readonly Dictionary<RoomType, List<PackedScene>> _roomTemplatesByType = new();
    private readonly Random _random;

    /// <summary>
    /// Initializes a new instance of the <see cref="RoomGenerator"/> class with the specified settings.
    /// </summary>
    /// <param name="settings">The map generation settings.</param>
    /// <param name="seed">Random seed</param>
    public RoomGenerator(MapGenerationSettings settings, int seed)
    {
        _random = new(seed);
        _mapGenerationSettings = settings;
        
        LoadRoomTemplates(RoomType.Regular, GetRoomTemplateDirectory("Regular"));
        LoadRoomTemplates(RoomType.Start, GetRoomTemplateDirectory("Entrance"));
        LoadRoomTemplates(RoomType.Exit, GetRoomTemplateDirectory("Exit"));
        LoadRoomTemplates(RoomType.PillarAbstraction, GetRoomTemplateDirectory("PillarAbstraction"));
        LoadRoomTemplates(RoomType.PillarEncapsulation, GetRoomTemplateDirectory("PillarEncapsulation"));
        LoadRoomTemplates(RoomType.PillarPolymorphism, GetRoomTemplateDirectory("PillarPolymorphism"));
        LoadRoomTemplates(RoomType.PillarInheritance, GetRoomTemplateDirectory("PillarInheritance"));
    }

    /// <summary>
    /// Gets the directory path for room templates of the specified type.
    /// </summary>
    /// <param name="type">The type of room templates.</param>
    /// <returns>The directory path for the room templates.</returns>
    private string GetRoomTemplateDirectory(string type)
    {
        return $"res://World/Rooms/{type}/";
    }

    /// <summary>
    /// Loads room templates from the specified directory path and associates them with the specified room type.
    /// </summary>
    /// <param name="roomType">The type of rooms to load templates for.</param>
    /// <param name="directoryPath">The directory path to load the templates from.</param>
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
    
    /// <summary>
    /// Generates a room at the specified coordinates and of the specified type.
    /// </summary>
    /// <param name="coordinates">The coordinates of the room.</param>
    /// <param name="roomType">The type of the room.</param>
    /// <returns>The generated room.</returns>
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

    /// <summary>
    /// Collects all placeholder nodes from the specified room.
    /// </summary>
    /// <param name="room">The room to collect placeholders from.</param>
    /// <returns>A list of placeholders in the room.</returns>
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

    /// <summary>
    /// Removes the specified placeholders from the room and frees them.
    /// </summary>
    /// <param name="placeholders">The placeholders to remove and free.</param>
    private void RemovePlaceholders(List<Placeholder> placeholders)
    {
        foreach (Placeholder placeholder in placeholders)
        {
            placeholder.GetParent().RemoveChild(placeholder);
            placeholder.QueueFree();
        }
    }

    /// <summary>
    /// Generates enemies in the specified room at the given placeholders.
    /// </summary>
    /// <param name="room">The room to generate enemies in.</param>
    /// <param name="placeholders">The placeholders to use for enemy generation.</param>
    private void GenerateEnemies(World.Room room, List<EnemyPlaceholder> placeholders)
    {
        if (placeholders.Count == 0)
            return;
        
        int numOfEnemies = _random.Next(1, placeholders.Count + 1);

        for (int i = 0; i < numOfEnemies; i++)
        {
            EnemyPlaceholder placeholder = placeholders[i];
            
            int enemyIndex = _random.Next(_mapGenerationSettings.Enemies.Length);
            CharacterModelFactory modelFactory = _mapGenerationSettings.Enemies[enemyIndex];

            CharacterView enemy = _mapGenerationSettings.CharacterScene.Instantiate<CharacterView>();
            enemy.ControllerFactory = new EnemyCharacterControllerFactory();
            enemy.ModelFactory = modelFactory;
            
            placeholder.GetParent().AddChild(enemy);
            enemy.Position = placeholder.Position;

        }
    }
    
    /// <summary>
    /// Generates items in the specified room at the given placeholders.
    /// </summary>
    /// <param name="room">The room to generate items in.</param>
    /// <param name="placeholders">The placeholders to use for item generation.</param>
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
                    
                ItemView itemObject = _mapGenerationSettings.ItemObjectScene.Instantiate<ItemView>();
                itemObject.Item = item;
            
                placeholder.GetParent().AddChild(itemObject);
                itemObject.Position = placeholder.Position;
            }
        }
    }
}