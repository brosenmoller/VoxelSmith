using Godot;
using System.Collections.Generic;
using System;

public partial class PrefabMesh : WorldMesh
{
    public override void _Process(double delta)
    {
        UpdateMesh(
            new HashSet<Vector3I>(GameManager.DataManager.ProjectData.voxelPrefabs.Keys),
            GameManager.DataManager.PaletteData.palletePrefabs
        );
    }

    public override void UpdateVoxel(Vector3I position, Guid guid)
    {
        UpdateVoxel(position, guid, GameManager.DataManager.ProjectData.voxelPrefabs);
    }

    public override void ClearVoxel(Vector3I position)
    {
        ClearVoxel(position, GameManager.DataManager.ProjectData.voxelPrefabs);
    }

    public override void UpdateAll()
    {
        UpdateAll(GameManager.DataManager.ProjectData.voxelPrefabs);
    }

    public override void UpdateAllOfGUID(Guid guid)
    {
        UpdateAllOfGUID(guid, GameManager.DataManager.ProjectData.voxelPrefabs);
    }
}

