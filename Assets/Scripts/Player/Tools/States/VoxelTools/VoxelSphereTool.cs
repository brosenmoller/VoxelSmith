using Godot;

public class VoxelSphereTool : TwoPointsTool
{
    public class Options : IToolOptions
    {
        public bool hollow = true;
    }

    private Options options = new();
    public override IToolOptions GetToolOptions() => options;

    protected override Vector3I[] GetVoxelPositions()
    {
        return VoxelHelper.GetSphereVoxels(firstPosition, secondPosition, options.hollow);
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