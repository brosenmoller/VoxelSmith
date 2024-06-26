using Godot;
using System;
using System.Collections.Generic;

public partial class Chunk : StaticBody3D
{
    [Export] private CollisionShape3D collisionShape;
    [Export] private MeshInstance3D meshInstance;

    public Dictionary<Vector3I, Guid> chunkPositions;

    private SurfaceTool surfaceTool;
    private readonly bool[] faces = new bool[6];

    public void Setup()
    {
        surfaceTool = new SurfaceTool();
        chunkPositions = new Dictionary<Vector3I, Guid>();
    }

    public void UpdateChunkMesh<TVoxelData>(HashSet<Vector3I> allPositions, Dictionary<Guid, TVoxelData> palette) where TVoxelData : VoxelData
    {
        surfaceTool.Begin(Mesh.PrimitiveType.Triangles);

        foreach (var voxel in chunkPositions)
        {
            surfaceTool.SetColor(palette[voxel.Value].color);
            CreateVoxel(voxel.Key, allPositions);
        }

        surfaceTool.Index();
        meshInstance.Mesh = surfaceTool.Commit();
        collisionShape.Shape = meshInstance.Mesh.CreateTrimeshShape();
    }

    private void CreateVoxel(Vector3I position, HashSet<Vector3I> allPositions)
    {
        for (int i = 0; i < 6; i++)
        {
            faces[i] = !allPositions.Contains(position + CubeValues.cubeOffsets[i]);
        }

        void addVertex(Vector3 pos, Vector2 uv)
        {
            surfaceTool.SetUV(uv);
            surfaceTool.AddVertex(pos);
        }

        Vector3 vertexOffset = position;
        if (faces[0])//left
        {
            surfaceTool.SetNormal(new Vector3(-1, 0, 0));
            addVertex(CubeValues.cubeVertices[0] + vertexOffset, CubeValues.cubeUVs[0]);
            addVertex(CubeValues.cubeVertices[7] + vertexOffset, CubeValues.cubeUVs[3]);
            addVertex(CubeValues.cubeVertices[3] + vertexOffset, CubeValues.cubeUVs[2]);
            addVertex(CubeValues.cubeVertices[0] + vertexOffset, CubeValues.cubeUVs[0]);
            addVertex(CubeValues.cubeVertices[4] + vertexOffset, CubeValues.cubeUVs[1]);
            addVertex(CubeValues.cubeVertices[7] + vertexOffset, CubeValues.cubeUVs[3]);
        }
        if (faces[1])//right
        {
            surfaceTool.SetNormal(new Vector3(1, 0, 0));
            addVertex(CubeValues.cubeVertices[2] + vertexOffset, CubeValues.cubeUVs[0]);
            addVertex(CubeValues.cubeVertices[5] + vertexOffset, CubeValues.cubeUVs[3]);
            addVertex(CubeValues.cubeVertices[1] + vertexOffset, CubeValues.cubeUVs[2]);
            addVertex(CubeValues.cubeVertices[2] + vertexOffset, CubeValues.cubeUVs[0]);
            addVertex(CubeValues.cubeVertices[6] + vertexOffset, CubeValues.cubeUVs[1]);
            addVertex(CubeValues.cubeVertices[5] + vertexOffset, CubeValues.cubeUVs[3]);

        }
        if (faces[2])//bottom
        {
            surfaceTool.SetNormal(new Vector3(0, 1, 0));
            addVertex(CubeValues.cubeVertices[1] + vertexOffset, CubeValues.cubeUVs[0]);
            addVertex(CubeValues.cubeVertices[3] + vertexOffset, CubeValues.cubeUVs[3]);
            addVertex(CubeValues.cubeVertices[2] + vertexOffset, CubeValues.cubeUVs[2]);
            addVertex(CubeValues.cubeVertices[1] + vertexOffset, CubeValues.cubeUVs[0]);
            addVertex(CubeValues.cubeVertices[0] + vertexOffset, CubeValues.cubeUVs[1]);
            addVertex(CubeValues.cubeVertices[3] + vertexOffset, CubeValues.cubeUVs[3]);
        }
        if (faces[3])//top
        {
            surfaceTool.SetNormal(new Vector3(0, -1, 0));
            addVertex(CubeValues.cubeVertices[4] + vertexOffset, CubeValues.cubeUVs[0]);
            addVertex(CubeValues.cubeVertices[5] + vertexOffset, CubeValues.cubeUVs[1]);
            addVertex(CubeValues.cubeVertices[7] + vertexOffset, CubeValues.cubeUVs[2]);
            addVertex(CubeValues.cubeVertices[5] + vertexOffset, CubeValues.cubeUVs[1]);
            addVertex(CubeValues.cubeVertices[6] + vertexOffset, CubeValues.cubeUVs[3]);
            addVertex(CubeValues.cubeVertices[7] + vertexOffset, CubeValues.cubeUVs[2]);
        }
        if (faces[4])//back
        {
            surfaceTool.SetNormal(new Vector3(0, 0, -1));
            addVertex(CubeValues.cubeVertices[0] + vertexOffset, CubeValues.cubeUVs[0]);
            addVertex(CubeValues.cubeVertices[1] + vertexOffset, CubeValues.cubeUVs[3]);
            addVertex(CubeValues.cubeVertices[5] + vertexOffset, CubeValues.cubeUVs[1]);
            addVertex(CubeValues.cubeVertices[5] + vertexOffset, CubeValues.cubeUVs[1]);
            addVertex(CubeValues.cubeVertices[4] + vertexOffset, CubeValues.cubeUVs[2]);
            addVertex(CubeValues.cubeVertices[0] + vertexOffset, CubeValues.cubeUVs[0]);
        }
        if (faces[5])//front
        {
            surfaceTool.SetNormal(new Vector3(0, 0, 1));
            addVertex(CubeValues.cubeVertices[3] + vertexOffset, CubeValues.cubeUVs[0]);
            addVertex(CubeValues.cubeVertices[6] + vertexOffset, CubeValues.cubeUVs[3]);
            addVertex(CubeValues.cubeVertices[2] + vertexOffset, CubeValues.cubeUVs[2]);
            addVertex(CubeValues.cubeVertices[3] + vertexOffset, CubeValues.cubeUVs[0]);
            addVertex(CubeValues.cubeVertices[7] + vertexOffset, CubeValues.cubeUVs[1]);
            addVertex(CubeValues.cubeVertices[6] + vertexOffset, CubeValues.cubeUVs[3]);
        }
    }
}
