using Godot;

public class VoxelCubeTool : TwoPointsTool
{
    protected override Vector3I[] GetVoxelPositions()
    {
        return GetCubeVoxels();
    }

    protected override void GenerateMeshHighlight(Vector3I[] voxelPositions)
    {
        ctx.meshHighlightMeshInstance.Mesh = MeshHelper.CreateCubeMesh(firstPosition, secondPosition);
    }

    protected override void PlaceAction(Vector3I[] voxels)
    {
        GameManager.CommandManager.ExecuteCommand(new PlaceListCommand(voxels));
    }

    protected override void BreakAction(Vector3I[] voxels)
    {
        GameManager.CommandManager.ExecuteCommand(new BreakListCommand(voxels));
    }
}
