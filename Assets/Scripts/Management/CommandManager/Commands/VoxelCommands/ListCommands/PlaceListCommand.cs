using Godot;
using System;

public class PlaceListCommand : VoxelListCommand, ICommand
{
    private Guid paletteSwatchID;
    private PaletteType paletteType;

    public PlaceListCommand(params Vector3I[] voxelPositions) : base(voxelPositions)
    {
        paletteSwatchID = GameManager.DataManager.ProjectData.selectedPaletteSwatchId;
        paletteType = GameManager.DataManager.ProjectData.selectedPaletteType;
    }

    public void Execute()
    {
        Place();
    }

    public void Undo()
    {
        VoxelMemoryItem.ReplaceFromMemory(voxelMemory);
    }

    public void Place()
    {
        if (paletteType == PaletteType.Color)
        {
            foreach (Vector3I voxelPosition in voxelPositions)
            {
                GameManager.PrefabMesh.ClearVoxel(voxelPosition);
                GameManager.SurfaceMesh.UpdateVoxel(voxelPosition, paletteSwatchID);
            }
        }
        else if (paletteType == PaletteType.Prefab)
        {
            foreach (Vector3I voxelPosition in voxelPositions)
            {
                GameManager.SurfaceMesh.ClearVoxel(voxelPosition);
                GameManager.PrefabMesh.UpdateVoxel(voxelPosition, paletteSwatchID);
            }
        }
    }
}

