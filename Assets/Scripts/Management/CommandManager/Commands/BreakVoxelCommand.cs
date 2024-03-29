using Godot;
using System;

public class BreakVoxelCommand : ICommand
{
    private Vector3I voxelPosition;
    private Guid paletteSwatchID;
    private PaletteType paletteType;

    public BreakVoxelCommand(Vector3I voxelPosition)
    {
        this.voxelPosition = voxelPosition;
        paletteType = PaletteType.None;
    }

    public void Execute()
    {
        if (GameManager.DataManager.ProjectData.voxelColors.ContainsKey(voxelPosition))
        {
            paletteType = PaletteType.Color;
            paletteSwatchID = GameManager.DataManager.ProjectData.voxelColors[voxelPosition];

            GameManager.DataManager.ProjectData.voxelColors.Remove(voxelPosition);
            GameManager.SurfaceMesh.UpdateMesh();
        }
        else if (GameManager.DataManager.ProjectData.voxelPrefabs.ContainsKey(voxelPosition))
        {
            paletteType = PaletteType.Prefab;
            paletteSwatchID = GameManager.DataManager.ProjectData.voxelPrefabs[voxelPosition];

            GameManager.DataManager.ProjectData.voxelPrefabs.Remove(voxelPosition);
            GameManager.PrefabMesh.UpdateMesh();
        }
    }

    public void Undo()
    {
        if (paletteType == PaletteType.Color)
        {
            if (!GameManager.DataManager.ProjectData.voxelColors.ContainsKey(voxelPosition))
            {
                GameManager.DataManager.ProjectData.voxelColors.Add(voxelPosition, paletteSwatchID);
            }

            GameManager.SurfaceMesh.UpdateMesh();
        }
        else if (paletteType == PaletteType.Prefab)
        {
            if (!GameManager.DataManager.ProjectData.voxelPrefabs.ContainsKey(voxelPosition))
            {
                GameManager.DataManager.ProjectData.voxelPrefabs.Add(voxelPosition, paletteSwatchID);
            }

            GameManager.PrefabMesh.UpdateMesh();
        }
    }
}

