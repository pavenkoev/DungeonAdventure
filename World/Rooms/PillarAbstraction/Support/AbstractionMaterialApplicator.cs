using Godot;

namespace DungeonAdventure.World.Rooms.PillarAbstraction.Support;

/// <summary>
/// Applies a specified material to its parent CanvasItem node when entering the scene tree
/// and restores the original material when exiting.
/// </summary>
public partial class AbstractionMaterialApplicator : Node
{
    /// <summary>
    /// The material to apply to the parent CanvasItem.
    /// </summary>
    [Export] public Material Material { get; set; }

    private Material _originalMaterial;
    
    /// <summary>
    /// Called when the node is about to enter the scene tree. Applies the abstraction material to the parent CanvasItem.
    /// </summary>
    public override void _EnterTree()
    {
        CanvasItem parent = GetParentOrNull<CanvasItem>();
        if (parent == null)
            return;

        parent.Material = Material;
    }

    /// <summary>
    /// Called when the node is about to exit the scene tree. Restores the original material to the parent CanvasItem.
    /// </summary>
    public override void _ExitTree()
    {
        CanvasItem parent = GetParentOrNull<CanvasItem>();
        if (parent == null)
            return;

        parent.Material = _originalMaterial;
    }
}