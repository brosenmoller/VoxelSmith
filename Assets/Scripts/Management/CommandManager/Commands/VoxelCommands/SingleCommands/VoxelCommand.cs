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
        }
        else if (paletteType == PaletteType.Prefab)
        {
            GameManager.PrefabMesh.UpdateVoxel(voxelPosition, paletteSwatchID);
        }
    }

    protected void Break()
    {
        if (paletteType == PaletteType.Color)
        {
            GameManager.SurfaceMesh.ClearVoxel(voxelPosition);
        }
        else if (paletteType == PaletteType.Prefab)
        {
            GameManager.PrefabMesh.ClearVoxel(voxelPosition);
        }
    }
}