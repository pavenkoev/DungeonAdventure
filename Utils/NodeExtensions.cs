using DungeonAdventure.Characters;
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

    public static Dungeon FindDungeon(this Node node) => FindNodeUp<Dungeon>(node);
    public static Room FindRoom(this Node node) => FindNodeUp<Room>(node);
    public static Character FindCharacter(this Node node) => FindNodeUp<Character>(node);
    
    public static Character FindPlayer(this Node node)
    {
        Array<Node> nodes = node.GetTree().GetNodesInGroup(PlayerGroup);
        Character character = nodes[0] as Character;

        if (character != null && character.IsAlive)
            return character;
        return null;
    }
}