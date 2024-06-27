using Godot;
using System;

public class VoxelCommand
{
    protected Vector3I voxelPosition;
    protected Guid paletteSwatchID;
    protected PaletteType paletteType;

    protected ProjectData projectData;

    public VoxelCommand(Vector3I voxelPosition)
    {
        this.voxelPosition = voxelPosition;
        projectData = GameManager.DataManager.ProjectData;
    }

    protected void Place()
    {
        if (paletteType == PaletteType.Color)
        {
            GameManager.SurfaceMesh.UpdateVoxel(voxelPosition, paletteSwatchID);

            //projectData.voxelColors[voxelPosition] = paletteSwatchID;
            //GameManager.SurfaceMesh.UpdateMesh();
        }
        else if (paletteType == PaletteType.Prefab)
        {
            GameManager.PrefabMesh.UpdateVoxel(voxelPosition, paletteSwatchID);

            //projectData.voxelPrefabs[voxelPosition] = paletteSwatchID;
            //GameManager.PrefabMesh.UpdateMesh();
        }
    }

    protected void Break()
    {
        if (paletteType == PaletteType.Color)
        {
            GameManager.SurfaceMesh.ClearVoxel(voxelPosition);
            
            //projectData.voxelColors.Remove(voxelPosition);
            //GameManager.SurfaceMesh.UpdateMesh();
        }
        else if (paletteType == PaletteType.Prefab)
        {
            GameManager.PrefabMesh.ClearVoxel(voxelPosition);
            //projectData.voxelPrefabs.Remove(voxelPosition);
            //GameManager.PrefabMesh.UpdateMesh();
        }
    }
}

