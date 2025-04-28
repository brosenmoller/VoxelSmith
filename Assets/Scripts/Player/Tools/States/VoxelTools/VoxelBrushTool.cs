using Godot;

public class VoxelBrushTool : BrushTool
{
    protected override void BreakAction()
    {
        GameManager.CommandManager.ExecuteCommand(new BreakVoxelCommand(ctx.VoxelPosition));
    }

    protected override void PlaceAction()
    {
        Vector3I chosenVoxel = ctx.selectInsideEnabled ? ctx.VoxelPosition : ctx.GetNextVoxel();

        if ((!ctx.IsVoxelInPlayer(chosenVoxel) || ctx.ignorePlayerCheck) && GameManager.DataManager.ProjectData.SelectedVoxelData != null)
        {
            GameManager.CommandManager.ExecuteCommand(new PlaceVoxelCommand(chosenVoxel));
        }
    }
}
