using System.Collections.Generic;
using System.Linq;
using DungeonAdventure.Characters.Views;
using DungeonAdventure.Items;
using DungeonAdventure.Utils;
using Godot;
using ItemView = DungeonAdventure.Items.View.ItemView;

namespace DungeonAdventure.World.Rooms.PillarAbstraction.Support;

/// <summary>
/// Represents a room of the pillar of abstraction where nodes are given a simplified material appearance.
/// </summary>
public partial class AbstractionPillarRoom : Room
{
    /// <summary>
    /// The material applied to player characters.
    /// </summary>
    [Export] private Material _playerMaterial;
    
    /// <summary>
    /// The material applied to enemy characters.
    /// </summary>
    [Export] private Material _enemyMaterial;
    
    /// <summary>
    /// The material applied to item objects.
    /// </summary>
    [Export] private Material _itemMaterial;
    
    /// <summary>
    /// The material applied to other nodes.
    /// </summary>
    [Export] private Material _otherMaterial;

    /// <summary>
    /// The tile map of the room.
    /// </summary>
    [Export] private TileMap _tileMap;
    
    /// <summary>
    /// Called when the node is added to the scene.
    /// </summary>
    public override void _Ready()
    {
    }

    /// <summary>
    /// Applies the abstraction effect to the specified root node and its children.
    /// </summary>
    /// <param name="root">The root node to apply the abstraction to.</param>
    private void ApplyAbstraction(Node root)
    {
        if (root == null)
            return;

        if (ProcessNode(root))
            return;
        
        for (int i = 0; i < root.GetChildCount(); i++)
        {
            ApplyAbstraction(root.GetChild(i));
        }
    }

    /// <summary>
    /// Processes a node to apply the appropriate material based on its type.
    /// </summary>
    /// <param name="node">The node to process.</param>
    /// <returns>True if the node was processed, otherwise false.</returns>
    private bool ProcessNode(Node node)
    {
        if (node == _tileMap)
            return false;
        
        if (node is CharacterView character)
        {
            MakeTreeAbstract(node, character.IsPlayer ? _playerMaterial : _enemyMaterial);
            return true;
        }

        if (node is ItemView)
        {
            MakeTreeAbstract(node, _itemMaterial);
            return true;
        }

        if (node is CanvasItem canvasItem)
        {
            MakeNodeAbstract(canvasItem, _otherMaterial);
        }

        return false;
    }
    
    /// <summary>
    /// Applies the abstraction material to the specified root node and its descendant canvas items.
    /// </summary>
    /// <param name="root">The root node to apply the material to.</param>
    /// <param name="material">The material to apply.</param>
    private void MakeTreeAbstract(Node root, Material material)
    {
        foreach (CanvasItem node in root.FindNodesDown<CanvasItem>())
        {
            if (node is TileMap)
                continue;
            
            MakeNodeAbstract(node, material);
        }
    }
    
    /// <summary>
    /// Applies the abstraction material to the specified canvas item node.
    /// </summary>
    /// <param name="node">The canvas item node to apply the material to.</param>
    /// <param name="material">The material to apply.</param>
    private void MakeNodeAbstract(CanvasItem node, Material material)
    {
        for (int i = 0; i < node.GetChildCount(); i++)
        {
            if (node.GetChild(i) is AbstractionMaterialApplicator)
                return;
        }
        
        AbstractionMaterialApplicator applicator = new();
        applicator.Material = material;

        node.AddChild(applicator);
    }

    /// <summary>
    /// Undoes the abstraction effect on the specified node and its descendants.
    /// </summary>
    /// <param name="node">The node to undo the abstraction on.</param>
    private void UndoAbstraction(Node node)
    {
        List<AbstractionMaterialApplicator> applicators = node.FindNodesDown<AbstractionMaterialApplicator>().ToList();

        foreach (AbstractionMaterialApplicator applicator in applicators)
        {
            applicator.QueueFree();
        }
    }
    
    /// <summary>
    /// Called when a new node is added to the tree. Applies abstraction to the new node.
    /// </summary>
    /// <param name="node">The node that was added.</param>
    private void OnNodeAdded(Node node)
    {
        if (node is AbstractionMaterialApplicator || node.FindRoom() != this)
            return;
        
        Callable.From(() => ApplyAbstraction(this)).CallDeferred();
    }

    /// <summary>
    /// Called when the player exits the room. Undoes the abstraction effect on the player.
    /// </summary>
    /// <param name="player">The player character.</param>
    public override void OnPlayerExited(CharacterView player)
    {
        UndoAbstraction(player);
    }

    /// <summary>
    /// Pauses the room, stopping the application of abstraction effects.
    /// </summary>
    public override void Pause()
    {
        base.Pause();
        
        ChildEnteredTree -= OnNodeAdded;
    }

    /// <summary>
    /// Resumes the room, applying the abstraction effects to the room and any newly added nodes.
    /// </summary>
    public override void Resume()
    {
        base.Resume();

        ApplyAbstraction(this);
        ChildEnteredTree += OnNodeAdded;
    }
}