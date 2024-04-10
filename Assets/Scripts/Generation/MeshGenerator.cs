using Godot;
using System;
using System.Collections.Generic;

public class MeshGenerator<TVoxelData> where TVoxelData : VoxelData
{
    private const float voxelSize = 1f;

    private readonly Material defaultMaterial;
    private readonly SurfaceTool surfaceTool;

    private static readonly Vector3[] cubeVertices = 
    {
        new(0, 0, 0), new(1, 0, 0),
        new(1, 0, 1), new(0, 0, 1),
        new(0, 1, 0), new(1, 1, 0),
        new(1, 1, 1), new(0, 1, 1)
    };

    private static readonly Vector3I[] offsets =
    {
        new(-1, 0, 0), // left
        new(1, 0, 0), // right
        new(0, -1, 0), // bottom
        new(0, 1, 0), // top
        new(0, 0, -1),  // back
        new(0, 0, 1) // front
    };

    private static readonly Vector2[] cubeUVs =
    {
        new(0, 0),
        new(1, 1),
        new(1, 0),
        new(0, 0),
        new(0, 1),
        new(1, 1)
    };

    private readonly bool[] faces = new bool[6];

    public MeshGenerator(Material material)
    {
        surfaceTool = new SurfaceTool();

        if (material != null) { defaultMaterial = material; }
        else { defaultMaterial = new StandardMaterial3D() { VertexColorUseAsAlbedo = true }; }
    }

    public MeshGenerator()
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
            CreateVoxel(voxel, voxelPositions);
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
            CreateVoxel(voxel, voxelPositions);
        }

        surfaceTool.Index();
        return surfaceTool.Commit();
    }

    private void CreateVoxel(Vector3I position, HashSet<Vector3I> voxelPositions)
    {
        for (int i = 0; i < 6; i++)
        {
            faces[i] = !voxelPositions.Contains(position + offsets[i]);
        }

        void addVertex(Vector3 pos, Vector2 uv)
        {
            surfaceTool.SetUV(uv);
            surfaceTool.AddVertex(pos * voxelSize);
        }

        Vector3 vertexOffset = position;
        if (faces[0])
        {
            surfaceTool.SetNormal(new Vector3(-1, 0, 0));
            addVertex(cubeVertices[0] + vertexOffset, cubeUVs[0]);
            addVertex(cubeVertices[7] + vertexOffset, cubeUVs[1]);
            addVertex(cubeVertices[3] + vertexOffset, cubeUVs[2]);
            addVertex(cubeVertices[0] + vertexOffset, cubeUVs[3]);
            addVertex(cubeVertices[4] + vertexOffset, cubeUVs[4]);
            addVertex(cubeVertices[7] + vertexOffset, cubeUVs[5]);
        }
        if (faces[1])
        {
            surfaceTool.SetNormal(new Vector3(1, 0, 0));
            addVertex(cubeVertices[2] + vertexOffset, cubeUVs[0]);
            addVertex(cubeVertices[5] + vertexOffset, cubeUVs[1]);
            addVertex(cubeVertices[1] + vertexOffset, cubeUVs[2]);
            addVertex(cubeVertices[2] + vertexOffset, cubeUVs[3]);
            addVertex(cubeVertices[6] + vertexOffset, cubeUVs[4]);
            addVertex(cubeVertices[5] + vertexOffset, cubeUVs[5]);

        }
        if (faces[2])
        {
            surfaceTool.SetNormal(new Vector3(0, 1, 0));
            addVertex(cubeVertices[1] + vertexOffset, cubeUVs[0]);
            addVertex(cubeVertices[3] + vertexOffset, cubeUVs[1]);
            addVertex(cubeVertices[2] + vertexOffset, cubeUVs[2]);
            addVertex(cubeVertices[1] + vertexOffset, cubeUVs[3]);
            addVertex(cubeVertices[0] + vertexOffset, cubeUVs[4]);
            addVertex(cubeVertices[3] + vertexOffset, cubeUVs[5]);
        }
        if (faces[3])
        {
            surfaceTool.SetNormal(new Vector3(0, -1, 0));
            addVertex(cubeVertices[4] + vertexOffset, cubeUVs[0]);
            addVertex(cubeVertices[5] + vertexOffset, cubeUVs[1]);
            addVertex(cubeVertices[7] + vertexOffset, cubeUVs[2]);
            addVertex(cubeVertices[5] + vertexOffset, cubeUVs[3]);
            addVertex(cubeVertices[6] + vertexOffset, cubeUVs[4]);
            addVertex(cubeVertices[7] + vertexOffset, cubeUVs[5]);
        }
        if (faces[4])
        {
            surfaceTool.SetNormal(new Vector3(0, 0, -1));
            addVertex(cubeVertices[0] + vertexOffset, cubeUVs[0]);
            addVertex(cubeVertices[1] + vertexOffset, cubeUVs[1]);
            addVertex(cubeVertices[5] + vertexOffset, cubeUVs[2]);
            addVertex(cubeVertices[5] + vertexOffset, cubeUVs[3]);
            addVertex(cubeVertices[4] + vertexOffset, cubeUVs[4]);
            addVertex(cubeVertices[0] + vertexOffset, cubeUVs[5]);
        }
        if (faces[5])
        {
            surfaceTool.SetNormal(new Vector3(0, 0, 1));
            addVertex(cubeVertices[3] + vertexOffset, cubeUVs[0]);
            addVertex(cubeVertices[6] + vertexOffset, cubeUVs[1]);
            addVertex(cubeVertices[2] + vertexOffset, cubeUVs[2]);
            addVertex(cubeVertices[3] + vertexOffset, cubeUVs[3]);
            addVertex(cubeVertices[7] + vertexOffset, cubeUVs[4]);
            addVertex(cubeVertices[6] + vertexOffset, cubeUVs[5]);
        }
    }
}

