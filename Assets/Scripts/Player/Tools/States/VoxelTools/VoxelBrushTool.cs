using Godot;

public class VoxelBrushTool : BrushTool
{
    protected override void BreakAction()
    {
        GameManager.CommandManager.ExecuteCommand(new BreakVoxelCommand(ctx.VoxelPosition));
    }

    protected override void PlaceAction()
    {
        Vector3I nextVoxel = ctx.GetNextVoxel();

        if ((!ctx.IsVoxelInPlayer(nextVoxel) || ctx.ignorePlayerCheck) && GameManager.DataManager.ProjectData.SelectedVoxelData != null)
        {
            GameManager.CommandManager.ExecuteCommand(new PlaceVoxelCommand(nextVoxel));
        }
    }
}
