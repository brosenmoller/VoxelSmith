using Godot;

public class VoxelLineTool : TwoPointsTool
{
    private const float stepLength = 0.2f;

    protected override Vector3I[] GetVoxelPositions()
    {
        return VoxelHelper.GetLineVoxels(firstPosition, secondPosition, stepLength);
    }

    protected override void BreakAction(Vector3I[] voxels)
    {
        GameManager.CommandManager.ExecuteCommand(new BreakListCommand(voxels));
    }

    protected override void PlaceAction(Vector3I[] voxels)
    {
        GameManager.CommandManager.ExecuteCommand(new PlaceListCommand(voxels));
    }
}
