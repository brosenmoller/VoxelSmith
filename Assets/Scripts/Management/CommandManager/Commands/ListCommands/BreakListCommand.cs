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
        ReplaceFromMemory();
    }

    public void Break()
    {
        foreach (Vector3I voxelPosition in voxelPositions)
        {
            projectData.voxelColors.Remove(voxelPosition);
            projectData.voxelPrefabs.Remove(voxelPosition);
        }

        if (containsColors)
        {
            GameManager.SurfaceMesh.UpdateMesh();
        }

        if (containsPrefabs)
        {
            GameManager.PrefabMesh.UpdateMesh();
        }
    }
}

