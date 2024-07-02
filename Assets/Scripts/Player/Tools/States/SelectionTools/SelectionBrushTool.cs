using Godot;

public class SelectionBrushTool : BrushTool
{
    protected override void BreakAction()
    {
        GameManager.CommandManager.ExecuteCommand(new ClearSelectionListCommand(ctx.VoxelPosition));
    }

    protected override void PlaceAction()
    {
        Vector3I nextVoxel = GetNextVoxel();
        GameManager.CommandManager.ExecuteCommand(new AddSelectionListCommand(nextVoxel));
    }
}
