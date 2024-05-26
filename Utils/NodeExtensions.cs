using System.Collections.Generic;
using DungeonAdventure.Characters;
using DungeonAdventure.Characters.Views;
using DungeonAdventure.World;
using Godot;
using Godot.Collections;

namespace DungeonAdventure.Utils;

public static class NodeExtensions
{
    private const string PlayerGroup = "player";
    
    public static T FindNodeUp<T>(this Node node) where T : class
    {
        if (node == null)
            return null;

        if (node is T targetNode)
            return targetNode;

        return FindNodeUp<T>(node.GetParent());
    }

    public static T FindNodeDown<T>(this Node node) where T : class
    {
        if (node == null)
            return null;

        Queue<Node> queue = new Queue<Node>();
        queue.Enqueue(node);

        while (queue.Count > 0)
        {
            Node current = queue.Dequeue();

            if (current is T targetNode)
                return targetNode;

            for (int i = 0; i < current.GetChildCount(); i++)
            {
                queue.Enqueue(current.GetChild(i));
            }
        }

        return null;
    }

    public static Dungeon FindDungeon(this Node node) => FindNodeUp<Dungeon>(node);
    public static Room FindRoom(this Node node) => FindNodeUp<Room>(node);
    public static CharacterView FindCharacter(this Node node) => FindNodeUp<CharacterView>(node);
    
    public static CharacterView FindPlayer(this Node node)
    {
        Array<Node> nodes = node.GetTree().GetNodesInGroup(PlayerGroup);
        CharacterView character = nodes[0] as CharacterView;

        if (character != null && character.IsAlive)
            return character;
        return null;
    }
}