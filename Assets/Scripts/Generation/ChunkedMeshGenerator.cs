using Godot;
using System;
using System.Collections.Generic;

public class ChunkedMeshGenerator<TVoxelData> : IMeshGenerator<TVoxelData> where TVoxelData : VoxelData
{
    private int chunkSize;
    private Dictionary<Vector3I, Chunk> chunks;
    private HashSet<Vector3I> lastPositions;

    public ChunkedMeshGenerator(int chunkSize)
    {
        this.chunkSize = chunkSize;
        chunks = new Dictionary<Vector2I, Chunk>();
        lastPositions = new HashSet<Vector3I>();
    }

    public Mesh CreateColorMesh(Dictionary<Vector3I, Guid> voxels, Dictionary<Guid, TVoxelData> palette)
    {
        HashSet<Vector3I> voxelPositions = new(voxels.Keys);

        throw new NotImplementedException();
    }

    public Mesh CreateMesh(Vector3I[] voxelPositionList)
    {
        HashSet<Vector3I> voxelPositions = new(voxelPositionList);

        voxelPositions.IntersectWith(lastPositions);

        List<Chunk> chunksToBeUpdate = new();

        foreach (Vector3I item in voxelPositions)
        {
            
        }

        foreach (Chunk chunk in chunksToBeUpdate)
        {
            //chunk.CreateChunkMesh()
        }

        throw new NotImplementedException();
    }
}

public class Chunk
{
    private const float voxelSize = 1f;

    public Mesh mesh;

    private readonly SurfaceTool surfaceTool;
    private readonly bool[] faces = new bool[6];

    public HashSet<Vector3I> chunkPositions;

    public Chunk()
    {
        surfaceTool = new SurfaceTool();
        chunkPositions = new HashSet<Vector3I>();
    }

    public Mesh CreateChunkMesh(HashSet<Vector3I> allPositions)
    {
        surfaceTool.Begin(Mesh.PrimitiveType.Triangles);

        foreach (Vector3I voxel in chunkPositions)
        {
            CreateVoxel(voxel, allPositions);
        }

        surfaceTool.Index();
        return surfaceTool.Commit();
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
