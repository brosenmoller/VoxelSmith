using Godot;
using System;
using System.Collections.Generic;

public class ChunkedMeshGenerator<TVoxelData> where TVoxelData : VoxelData
{
    private int chunkSize;
    public Dictionary<Vector3I, Chunk<TVoxelData>> chunks;
    public HashSet<Chunk<TVoxelData>> chunksToBeUpdated;

    public Vector3I GetChunkPosition(Vector3I position)
    {
        return new Vector3I(
            Mathf.FloorToInt(position.X / chunkSize),
            Mathf.FloorToInt(position.Y / chunkSize),
            Mathf.FloorToInt(position.Z / chunkSize)
        );
    }

    public ChunkedMeshGenerator(int chunkSize)
    {
        this.chunkSize = chunkSize;
        chunks = new Dictionary<Vector3I, Chunk<TVoxelData>>();
        chunksToBeUpdated = new HashSet<Chunk<TVoxelData>>();
    }

    public Mesh CreateColorMesh(HashSet<Vector3I> allPositions, Dictionary<Guid, TVoxelData> palette)
    {
        ArrayMesh completeMesh = new();

        foreach (Chunk<TVoxelData> chunk in chunks.Values)
        {
            if (chunksToBeUpdated.Contains(chunk))
            {
                chunk.CreateChunkColorMesh(allPositions, palette);
            }

            completeMesh.AddSurfaceFromArrays(Mesh.PrimitiveType.Triangles, chunk.meshArray);
        }

        return completeMesh;
    }

    public Mesh CreateMesh(HashSet<Vector3I> allPositions)
    {
        ArrayMesh completeMesh = new();

        foreach (Chunk<TVoxelData> chunk in chunks.Values)
        {
            if (chunksToBeUpdated.Contains(chunk))
            {
                chunk.CreateChunkMesh(allPositions);
            }

            completeMesh.AddSurfaceFromArrays(Mesh.PrimitiveType.Triangles, chunk.meshArray);
        }

        return completeMesh;
    }
}

public class Chunk<TVoxelData> where TVoxelData : VoxelData
{
    private const float voxelSize = 1f;

    public Godot.Collections.Array meshArray;

    private readonly SurfaceTool surfaceTool;
    private readonly bool[] faces = new bool[6];

    public Dictionary<Vector3I, Guid> chunkPositions;

    public Chunk()
    {
        surfaceTool = new SurfaceTool();
        chunkPositions = new Dictionary<Vector3I, Guid>();
    }

    public void CreateChunkMesh(HashSet<Vector3I> allPositions)
    {
        surfaceTool.Begin(Mesh.PrimitiveType.Triangles);

        foreach (Vector3I voxel in chunkPositions.Keys)
        {
            CreateVoxel(voxel, allPositions);
        }

        surfaceTool.Index();
        meshArray = surfaceTool.CommitToArrays();
    }
    public void CreateChunkColorMesh(HashSet<Vector3I> allPositions, Dictionary<Guid, TVoxelData> palette)
    {
        surfaceTool.Begin(Mesh.PrimitiveType.Triangles);

        foreach (var voxel in chunkPositions)
        {
            surfaceTool.SetColor(palette[voxel.Value].color);
            CreateVoxel(voxel.Key, allPositions);
        }

        surfaceTool.Index();
        meshArray = surfaceTool.CommitToArrays();
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
            surfaceTool.AddVertex(pos * voxelSize);
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
