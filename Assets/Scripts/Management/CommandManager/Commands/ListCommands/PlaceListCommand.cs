using Godot;
using System;

public class PlaceListCommand : VoxelListCommand, ICommand
{
    private Guid paletteSwatchID;
    private PaletteType paletteType;

    public PlaceListCommand(params Vector3I[] voxelPositions) : base(voxelPositions)
    {
        paletteSwatchID = projectData.selectedPaletteSwatchId;
        paletteType = projectData.selectedPaletteType;
    }

    public void Execute()
    {
        Place();
    }

    public void Undo()
    {
        ReplaceFromMemory();
    }

    public void Place()
    {
        if (paletteType == PaletteType.Color)
        {
            foreach (Vector3I voxelPosition in voxelPositions)
            {
                //projectData.voxelPrefabs.Remove(voxelPosition);
                GameManager.PrefabMesh.ClearVoxel(voxelPosition);

                //projectData.voxelColors[voxelPosition] = paletteSwatchID;
                GameManager.SurfaceMesh.UpdateVoxel(voxelPosition, paletteSwatchID);
            }

            //GameManager.SurfaceMesh.UpdateMesh();
        }
        else if (paletteType == PaletteType.Prefab)
        {
            foreach (Vector3I voxelPosition in voxelPositions)
            {
                //projectData.voxelColors.Remove(voxelPosition);
                GameManager.SurfaceMesh.ClearVoxel(voxelPosition);

                //projectData.voxelPrefabs[voxelPosition] = paletteSwatchID;
                GameManager.PrefabMesh.UpdateVoxel(voxelPosition, paletteSwatchID);
            }

            //GameManager.PrefabMesh.UpdateMesh();
        }
    }
}

