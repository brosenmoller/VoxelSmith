﻿using Godot;
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

    private readonly bool[] faces = new bool[6];

    public MeshGenerator(Material material)
    {
        surfaceTool = new SurfaceTool();

        if (material != null) { defaultMaterial = material; }
        else { defaultMaterial = new StandardMaterial3D() { VertexColorUseAsAlbedo = true }; }
    }

    public Mesh CreateMesh(Dictionary<Vector3I, TVoxelData> voxels)
    {
        surfaceTool.Begin(Mesh.PrimitiveType.Triangles);
        surfaceTool.SetMaterial(defaultMaterial);

        foreach (Vector3I voxel in voxels.Keys)
        {
            CreateVoxel(voxel, voxels[voxel].color, voxels);
        }

        surfaceTool.Index();
        return surfaceTool.Commit();
    }

    private void CreateVoxel(Vector3I position, Color color, Dictionary<Vector3I, TVoxelData> voxels)
    {
        for (int i = 0; i < 6; i++)
        {
            faces[i] = !voxels.ContainsKey(position + offsets[i]);
        }

        surfaceTool.SetColor(color);

        void addVertex(Vector3 pos) => surfaceTool.AddVertex(pos * voxelSize);

        Vector3 vertexOffset = position;
        if (faces[0])
        {
            surfaceTool.SetNormal(new Vector3(-1, 0, 0));
            addVertex(cubeVertices[0] + vertexOffset);
            addVertex(cubeVertices[7] + vertexOffset);
            addVertex(cubeVertices[3] + vertexOffset);
            addVertex(cubeVertices[0] + vertexOffset);
            addVertex(cubeVertices[4] + vertexOffset);
            addVertex(cubeVertices[7] + vertexOffset);
        }
        if (faces[1])
        {
            surfaceTool.SetNormal(new Vector3(1, 0, 0));
            addVertex(cubeVertices[2] + vertexOffset);
            addVertex(cubeVertices[5] + vertexOffset);
            addVertex(cubeVertices[1] + vertexOffset);
            addVertex(cubeVertices[2] + vertexOffset);
            addVertex(cubeVertices[6] + vertexOffset);
            addVertex(cubeVertices[5] + vertexOffset);

        }
        if (faces[2])
        {
            surfaceTool.SetNormal(new Vector3(0, 1, 0));
            addVertex(cubeVertices[1] + vertexOffset);
            addVertex(cubeVertices[3] + vertexOffset);
            addVertex(cubeVertices[2] + vertexOffset);
            addVertex(cubeVertices[1] + vertexOffset);
            addVertex(cubeVertices[0] + vertexOffset);
            addVertex(cubeVertices[3] + vertexOffset);
        }
        if (faces[3])
        {
            surfaceTool.SetNormal(new Vector3(0, -1, 0));
            addVertex(cubeVertices[4] + vertexOffset);
            addVertex(cubeVertices[5] + vertexOffset);
            addVertex(cubeVertices[7] + vertexOffset);
            addVertex(cubeVertices[5] + vertexOffset);
            addVertex(cubeVertices[6] + vertexOffset);
            addVertex(cubeVertices[7] + vertexOffset);
        }
        if (faces[4])
        {
            surfaceTool.SetNormal(new Vector3(0, 0, -1));
            addVertex(cubeVertices[0] + vertexOffset);
            addVertex(cubeVertices[1] + vertexOffset);
            addVertex(cubeVertices[5] + vertexOffset);
            addVertex(cubeVertices[5] + vertexOffset);
            addVertex(cubeVertices[4] + vertexOffset);
            addVertex(cubeVertices[0] + vertexOffset);
        }
        if (faces[5])
        {
            surfaceTool.SetNormal(new Vector3(0, 0, 1));
            addVertex(cubeVertices[3] + vertexOffset);
            addVertex(cubeVertices[6] + vertexOffset);
            addVertex(cubeVertices[2] + vertexOffset);
            addVertex(cubeVertices[3] + vertexOffset);
            addVertex(cubeVertices[7] + vertexOffset);
            addVertex(cubeVertices[6] + vertexOffset);
        }
    }
}

