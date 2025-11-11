using Godot;

public class VoxelCubeTool : TwoPointsTool
{
    public class Options : IToolOptions
    {
        public bool walls = false;
    }

    private readonly Options options = new();
    public override IToolOptions GetToolOptions() => options;

    protected override Vector3I[] GetVoxelPositions()
    {
        if (options.walls)
        {
            return VoxelHelper.GetHollowCubeVoxels(firstPosition, secondPosition, VoxelHelper.CubeFaces.XZ);
        }
        return VoxelHelper.GetCubeVoxels(firstPosition, secondPosition);
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