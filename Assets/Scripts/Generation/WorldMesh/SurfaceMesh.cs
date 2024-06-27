using Godot;
using System;
using System.Collections.Generic;

public partial class SurfaceMesh : WorldMesh
{
    public override void _Process(double delta)
    {
        UpdateMesh(
            new HashSet<Vector3I>(GameManager.DataManager.ProjectData.voxelColors.Keys),
            GameManager.DataManager.PaletteData.paletteColors
        );
    }

    public override void UpdateVoxel(Vector3I position, Guid guid)
    {
        UpdateVoxel(position, guid, GameManager.DataManager.ProjectData.voxelColors);
    }

    public override void ClearVoxel(Vector3I position)
    {
        ClearVoxel(position, GameManager.DataManager.ProjectData.voxelColors);
    }

    public override void UpdateAll()
    {
        UpdateAll(GameManager.DataManager.ProjectData.voxelColors);
    }

    public override void UpdateAllOfGUID(Guid guid)
    {
        UpdateAllOfGUID(guid, GameManager.DataManager.ProjectData.voxelColors);
    }
}
