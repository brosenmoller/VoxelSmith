using Godot;
using System;
using System.Collections.Generic;

public class BasicMeshGenerator<TVoxelData> : IMeshGenerator<TVoxelData> where TVoxelData : VoxelData
{
    private readonly Material defaultMaterial;
    private readonly SurfaceTool surfaceTool;

    public BasicMeshGenerator(Material material)
    {
        surfaceTool = new SurfaceTool();

        if (material != null) { defaultMaterial = material; }
        else { defaultMaterial = new StandardMaterial3D() { VertexColorUseAsAlbedo = true }; }
    }

    public BasicMeshGenerator()
    {
        surfaceTool = new SurfaceTool();
        defaultMaterial = new StandardMaterial3D() { VertexColorUseAsAlbedo = true };
    }

    public Mesh CreateColorMesh(Dictionary<Vector3I, Guid> voxels, Dictionary<Guid, TVoxelData> palette)
    {
        surfaceTool.Begin(Mesh.PrimitiveType.Triangles);
        surfaceTool.SetMaterial(defaultMaterial);

        HashSet<Vector3I> voxelPositions = new(voxels.Keys);

        foreach (Vector3I voxel in voxels.Keys)
        {
            surfaceTool.SetColor(palette[voxels[voxel]].color);
            MeshHelper.CreateVoxel(surfaceTool, voxel, voxelPositions);
        }

        surfaceTool.Index();
        return surfaceTool.Commit();
    }

    public Mesh CreateMesh(Vector3I[] voxelPositionList)
    {
        surfaceTool.Begin(Mesh.PrimitiveType.Triangles);

        HashSet<Vector3I> voxelPositions = new(voxelPositionList);

        foreach (Vector3I voxel in voxelPositions)
        {
            MeshHelper.CreateVoxel(surfaceTool, voxel, voxelPositions);
        }

        surfaceTool.Index();
        return surfaceTool.Commit();
    }
}

