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
        if (voxelData is VoxelColor color)
        {
            if (!GameManager.DataManager.ProjectData.voxelColors.ContainsKey(voxelPosition))
            {
                GameManager.DataManager.ProjectData.voxelColors.Add(voxelPosition, color);
            }

            GameManager.SurfaceMesh.UpdateMesh();
        }
        else if (voxelData is VoxelPrefab prefab)
        {
            if (!GameManager.DataManager.ProjectData.voxelPrefabs.ContainsKey(voxelPosition))
            {
                GameManager.DataManager.ProjectData.voxelPrefabs.Add(voxelPosition, prefab);
            }
        }
    }

    public void Undo()
    {
        if (voxelData is VoxelColor)
        {
            if (GameManager.DataManager.ProjectData.voxelColors.ContainsKey(voxelPosition))
            {
                GameManager.DataManager.ProjectData.voxelColors.Remove(voxelPosition);
            }

            GameManager.SurfaceMesh.UpdateMesh();
        }
        else if (voxelData is VoxelPrefab)
        {
            if (GameManager.DataManager.ProjectData.voxelPrefabs.ContainsKey(voxelPosition))
            {
                GameManager.DataManager.ProjectData.voxelPrefabs.Remove(voxelPosition);
            }
        }
    }
}

