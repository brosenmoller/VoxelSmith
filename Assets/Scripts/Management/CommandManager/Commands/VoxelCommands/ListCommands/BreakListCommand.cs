using Godot;

public class BreakListCommand : VoxelListCommand, ICommand
{
    public BreakListCommand(params Vector3I[] voxelPositions) : base(voxelPositions) { }

    public void Execute()
    {
        Break();
    }

    public void Undo()
    {
        VoxelMemoryItem.ReplaceFromMemory(voxelMemory);
    }

    public void Break()
    {
        foreach (Vector3I voxelPosition in voxelPositions)
        {
            GameManager.SurfaceMesh.ClearVoxel(voxelPosition);
            GameManager.PrefabMesh.ClearVoxel(voxelPosition);
        }
    }
}

