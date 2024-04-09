using Godot;
using System;

public class VoxelCommand
{
    private readonly Vector3I[] voxelPositions;
    private readonly Guid paletteSwatchID;
    private readonly PaletteType paletteType;

    public VoxelCommand(params Vector3I[] voxelPositions)
    {
        this.voxelPositions = voxelPositions;
        paletteSwatchID = GameManager.DataManager.ProjectData.selectedPaletteSwatchId;
        paletteType = GameManager.DataManager.ProjectData.selectedPaletteType;
    }

    public void Execute()
    {
        if (paletteType == PaletteType.Color)
        {
            foreach (Vector3I voxelPosition in voxelPositions)
            {
                if (!GameManager.DataManager.ProjectData.voxelColors.ContainsKey(voxelPosition))
                {
                    GameManager.DataManager.ProjectData.voxelColors.Add(voxelPosition, paletteSwatchID);
                }
            }

            GameManager.SurfaceMesh.UpdateMesh();
        }
        else if (paletteType == PaletteType.Prefab)
        {
            foreach (Vector3I voxelPosition in voxelPositions)
            {
                if (!GameManager.DataManager.ProjectData.voxelPrefabs.ContainsKey(voxelPosition))
                {
                    GameManager.DataManager.ProjectData.voxelPrefabs.Add(voxelPosition, paletteSwatchID);
                }
            }

            GameManager.PrefabMesh.UpdateMesh();
        }
    }

    public void Undo()
    {
        if (paletteType == PaletteType.Color)
        {
            foreach (Vector3I voxelPosition in voxelPositions)
            {
                if (GameManager.DataManager.ProjectData.voxelColors.ContainsKey(voxelPosition))
                {
                    GameManager.DataManager.ProjectData.voxelColors.Remove(voxelPosition);
                }
            }

            GameManager.SurfaceMesh.UpdateMesh();
        }
        else if (paletteType == PaletteType.Prefab)
        {
            foreach (Vector3I voxelPosition in voxelPositions)
            {
                if (GameManager.DataManager.ProjectData.voxelPrefabs.ContainsKey(voxelPosition))
                {
                    GameManager.DataManager.ProjectData.voxelPrefabs.Remove(voxelPosition);
                }
            }

            GameManager.PrefabMesh.UpdateMesh();
        }
    }
}

