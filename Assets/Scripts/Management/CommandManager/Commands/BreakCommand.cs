using Godot;

public class BreakCommand
{
    public Vector3I voxelPosition;
    public VoxelData voxelData;
    public SurfaceMesh surfaceMesh;

    public BreakCommand(Vector3I voxelPosition, VoxelData voxelData, SurfaceMesh surfaceMesh)
    {
        this.voxelPosition = voxelPosition;
        this.voxelData = voxelData;
        this.surfaceMesh = surfaceMesh;
    }

    public void Execute()
    {
        if (GameManager.DataManager.ProjectData.voxels.ContainsKey(voxelPosition))
        {
            GameManager.DataManager.ProjectData.voxels.Remove(voxelPosition);
        }

        surfaceMesh.UpdateMesh();
    }

    public void Undo()
    {
        if (!GameManager.DataManager.ProjectData.voxels.ContainsKey(voxelPosition))
        {
            GameManager.DataManager.ProjectData.voxels.Add(voxelPosition, voxelData);
        }

        surfaceMesh.UpdateMesh();
    }
}

