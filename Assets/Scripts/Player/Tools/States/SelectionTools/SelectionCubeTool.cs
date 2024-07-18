using Godot;

public class SelectionCubeTool : TwoPointsTool
{
    protected override Vector3I[] GetVoxelPositions()
    {
        return VoxelHelper.GetCubeVoxels(firstPosition, secondPosition);
    }

    protected override void GenerateMeshHighlight(Vector3I[] voxelPositions)
    {
        ctx.meshHighlightMeshInstance.Mesh = MeshHelper.CreateCubeMesh(firstPosition, secondPosition);
    }

    protected override void PlaceAction(Vector3I[] voxels)
    {
        GameManager.CommandManager.ExecuteCommand(new AddSelectionListCommand(voxels));
    }

    protected override void BreakAction(Vector3I[] voxels)
    {
        GameManager.CommandManager.ExecuteCommand(new ClearSelectionListCommand(voxels));
    }
}
