using Godot;
using System.Collections.Generic;
using System;

public interface IMeshGenerator<TVoxelData> where TVoxelData : VoxelData
{
    public Mesh CreateColorMesh(Dictionary<Vector3I, Guid> voxels, Dictionary<Guid, TVoxelData> palette);

    public Mesh CreateMesh(Vector3I[] voxelPositionList);
}

