using Godot;

public class BreakVoxelCommand : ICommand
{
    public Vector3I voxelPosition;
    public VoxelData voxelData;

    public BreakVoxelCommand(Vector3I voxelPosition)
    {
        this.voxelPosition = voxelPosition;
    }

    public void Execute()
    {
        if (GameManager.DataManager.ProjectData.voxelColors.ContainsKey(voxelPosition))
        {
            voxelData = GameManager.DataManager.ProjectData.voxelColors[voxelPosition];
            GameManager.DataManager.ProjectData.voxelColors.Remove(voxelPosition);

            GameManager.SurfaceMesh.UpdateMesh();
        }

        if (GameManager.DataManager.ProjectData.voxelPrefabs.ContainsKey(voxelPosition))
        {
            voxelData = GameManager.DataManager.ProjectData.voxelPrefabs[voxelPosition];
            GameManager.DataManager.ProjectData.voxelPrefabs.Remove(voxelPosition);
        }
    }

    public void Undo()
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
}

