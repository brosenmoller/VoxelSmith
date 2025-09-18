using System;
using Godot;

public class PlaceVoxelCommand : VoxelCommand, ICommand
{
    public PlaceVoxelCommand(Vector3I voxelPosition) : base(voxelPosition)
    {
        paletteSwatchID = projectData.selectedPaletteSwatchId;
        paletteType = projectData.selectedPaletteType;
    }

    private PaletteType previousPaletteType;
    private Guid previousPaletteSwatchID;
    private bool hadPreviousVoxel;

    public void Execute()
    {
        hadPreviousVoxel = true;
        if (projectData.voxelColors.ContainsKey(voxelPosition))
        {
            previousPaletteType = PaletteType.Color;
            previousPaletteSwatchID = projectData.voxelColors[voxelPosition];
        }
        else if (projectData.voxelPrefabs.ContainsKey(voxelPosition))
        {
            previousPaletteType = PaletteType.Prefab;
            previousPaletteSwatchID = projectData.voxelPrefabs[voxelPosition];
        }
        else
        {
            hadPreviousVoxel = false;
        }

        Place();
    }

    public void Undo()
    {
        if (hadPreviousVoxel)
        {
            if (previousPaletteType == PaletteType.Color)
            {
                GameManager.SurfaceMesh.UpdateVoxel(voxelPosition, previousPaletteSwatchID);
            }
            else if (previousPaletteType == PaletteType.Prefab)
            {
                GameManager.PrefabMesh.UpdateVoxel(voxelPosition, previousPaletteSwatchID);
            }
        }
        else
        {
            Break();
        }
    }
}