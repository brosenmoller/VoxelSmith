using Godot;
using System;

public class PlaceVoxelCommand : ICommand
{
    private Vector3I voxelPosition;
    private Guid paletteSwatchID;
    private PaletteType paletteType;

    public PlaceVoxelCommand(Vector3I voxelPosition)
    {
        this.voxelPosition = voxelPosition;
        paletteSwatchID = GameManager.DataManager.ProjectData.selectedPaletteSwatchId;
        paletteType = GameManager.DataManager.ProjectData.selectedPaletteType;
    }

    public void Execute()
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

    public void Undo()
    {
        if (GameManager.DataManager.ProjectData.voxelColors.ContainsKey(voxelPosition))
        {
            GameManager.DataManager.ProjectData.voxelColors.Remove(voxelPosition);
            GameManager.SurfaceMesh.UpdateMesh();
        }
        else if (GameManager.DataManager.ProjectData.voxelPrefabs.ContainsKey(voxelPosition))
        {
            GameManager.DataManager.ProjectData.voxelPrefabs.Remove(voxelPosition);
            GameManager.PrefabMesh.UpdateMesh();
        }
    }
}

