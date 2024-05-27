using System.Collections.Generic;
using DungeonAdventure.Characters;
using DungeonAdventure.Characters.Views;
using DungeonAdventure.World;
using Godot;
using Godot.Collections;

namespace DungeonAdventure.Utils;

/// <summary>
/// Provides extension methods for working with Godot nodes.
/// </summary>
public static class NodeExtensions
{
    private const string PlayerGroup = "player";
    
    /// <summary>
    /// Finds the first node of the specified type by traversing up the scene tree.
    /// </summary>
    /// <typeparam name="T">The type of node to find.</typeparam>
    /// <param name="node">The starting node.</param>
    /// <returns>The found node or null if not found.</returns>
    public static T FindNodeUp<T>(this Node node) where T : class
    {
        if (node == null)
            return null;

        if (node is T targetNode)
            return targetNode;

        return FindNodeUp<T>(node.GetParent());
    }

    /// <summary>
    /// Finds the first node of the specified type by traversing down the scene tree.
    /// </summary>
    /// <typeparam name="T">The type of node to find.</typeparam>
    /// <param name="node">The starting node.</param>
    /// <returns>The found node or null if not found.</returns>
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

    /// <summary>
    /// Finds all nodes of the specified type by traversing down the scene tree.
    /// </summary>
    /// <typeparam name="T">The type of nodes to find.</typeparam>
    /// <param name="node">The starting node.</param>
    /// <param name="includeRoot">Whether to include the root node in the search.</param>
    /// <returns>An enumerable of found nodes.</returns>
    public static IEnumerable<T> FindNodesDown<T>(this Node node, bool includeRoot = true) where T : class
    {
        if (node == null)
            yield break;
        
        if (includeRoot && node is T targetNode)
            yield return targetNode;

        for (int i = 0; i < node.GetChildCount(); i++)
        {
            foreach (T n in node.GetChild(i).FindNodesDown<T>(true))
                yield return n;
        }
    }

    /// <summary>
    /// Finds the first dungeon node by traversing up the scene tree.
    /// </summary>
    /// <param name="node">The starting node.</param>
    /// <returns>The found dungeon node or null if not found.</returns>
    public static Dungeon FindDungeon(this Node node) => FindNodeUp<Dungeon>(node);
    
    /// <summary>
    /// Finds the first room node by traversing up the scene tree.
    /// </summary>
    /// <param name="node">The starting node.</param>
    /// <returns>The found room node or null if not found.</returns>
    public static Room FindRoom(this Node node) => FindNodeUp<Room>(node);
    
    /// <summary>
    /// Finds the first character view node by traversing up the scene tree.
    /// </summary>
    /// <param name="node">The starting node.</param>
    /// <returns>The found character view node or null if not found.</returns>
    public static CharacterView FindCharacter(this Node node) => FindNodeUp<CharacterView>(node);
    
    /// <summary>
    /// Finds the player character view node.
    /// </summary>
    /// <param name="node">The starting node.</param>
    /// <returns>The found player character view node or null if not found.</returns>
    public static CharacterView FindPlayer(this Node node)
    {
        Array<Node> nodes = node.GetTree().GetNodesInGroup(PlayerGroup);
        CharacterView character = nodes[0] as CharacterView;

        if (character != null && character.IsAlive)
            return character;
        return null;
    }
}