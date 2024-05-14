using System;
using System.Collections.Generic;
using Godot;

namespace DungeonAdventure.World.Generation;

public class RoomGenerator
{

    private Dictionary<RoomType, List<PackedScene>> _roomTemplatesByType = new();
    private Random _random = new();

    public RoomGenerator()
    {
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
        return new Room(roomType, coordinates, scene);
    }
}