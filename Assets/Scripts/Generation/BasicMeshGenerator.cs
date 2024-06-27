using Godot;
using System;
using System.Collections.Generic;

public class BasicMeshGenerator<TVoxelData> : IMeshGenerator<TVoxelData> where TVoxelData : VoxelData
{
    private const float voxelSize = 1f;

    private readonly Material defaultMaterial;
    private readonly SurfaceTool surfaceTool;

    private readonly bool[] faces = new bool[6];

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
            faces[i] = !voxelPositions.Contains(position + CubeValues.cubeOffsets[i]);
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

