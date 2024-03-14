using Godot;

public class PlaceVoxelCommand : ICommand
{
    public Vector3I voxelPosition;
    public VoxelData voxelData;

    public PlaceVoxelCommand(Vector3I voxelPosition, VoxelData voxelData)
    {
        this.voxelPosition = voxelPosition;
        this.voxelData = voxelData;
    }

    public void Execute()
    {
        if (!GameManager.DataManager.ProjectData.voxels.ContainsKey(voxelPosition))
        {
            GameManager.DataManager.ProjectData.voxels.Add(voxelPosition, voxelData);
        }

        GameManager.SurfaceMesh.UpdateMesh();
    }

    public void Undo()
    {
        if (GameManager.DataManager.ProjectData.voxels.ContainsKey(voxelPosition))
        {
            GameManager.DataManager.ProjectData.voxels.Remove(voxelPosition);
        }

        GameManager.SurfaceMesh.UpdateMesh();
    }
}

