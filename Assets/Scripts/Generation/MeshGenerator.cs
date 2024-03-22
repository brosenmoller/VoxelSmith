using Godot;
using System.Collections.Generic;

public class MeshGenerator<T> where T : VoxelData
{
    private const float voxelSize = 1f;

    private readonly Material defaultMaterial;
    private readonly SurfaceTool surfaceTool;

    private static readonly Vector3[] Vertices = {
        new(0, 0, 0), new(1, 0, 0),
        new(1, 0, 1), new(0, 0, 1),
        new(0, 1, 0), new(1, 1, 0),
        new(1, 1, 1), new(0, 1, 1)
    };

    public MeshGenerator()
    {
        surfaceTool = new SurfaceTool();
        defaultMaterial = new StandardMaterial3D() { VertexColorUseAsAlbedo = true };
    }

    public Mesh CreateMesh(Dictionary<Vector3I, T> voxels)
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

    private void CreateVoxel(Vector3I position, Color color, Dictionary<Vector3I, T> voxels)
    {
        bool left = !voxels.ContainsKey(position + new Vector3I(-1, 0, 0));
        bool right = !voxels.ContainsKey(position + new Vector3I(1, 0, 0));
        bool back = !voxels.ContainsKey(position + new Vector3I(0, 0, -1));
        bool front = !voxels.ContainsKey(position + new Vector3I(0, 0, 1));
        bool top = !voxels.ContainsKey(position + new Vector3I(0, 1, 0));
        bool bottom = !voxels.ContainsKey(position + new Vector3I(0, -1, 0));

        surfaceTool.SetColor(color);

        void addVertex(Vector3 pos) => surfaceTool.AddVertex(pos * voxelSize);

        Vector3 vertexOffset = position;
        if (top)
        {
            surfaceTool.SetNormal(new Vector3(0, -1, 0));
            addVertex(Vertices[4] + vertexOffset);
            addVertex(Vertices[5] + vertexOffset);
            addVertex(Vertices[7] + vertexOffset);
            addVertex(Vertices[5] + vertexOffset);
            addVertex(Vertices[6] + vertexOffset);
            addVertex(Vertices[7] + vertexOffset);
        }
        if (right)
        {
            surfaceTool.SetNormal(new Vector3(1, 0, 0));
            addVertex(Vertices[2] + vertexOffset);
            addVertex(Vertices[5] + vertexOffset);
            addVertex(Vertices[1] + vertexOffset);
            addVertex(Vertices[2] + vertexOffset);
            addVertex(Vertices[6] + vertexOffset);
            addVertex(Vertices[5] + vertexOffset);

        }
        if (left)
        {
            surfaceTool.SetNormal(new Vector3(-1, 0, 0));
            addVertex(Vertices[0] + vertexOffset);
            addVertex(Vertices[7] + vertexOffset);
            addVertex(Vertices[3] + vertexOffset);
            addVertex(Vertices[0] + vertexOffset);
            addVertex(Vertices[4] + vertexOffset);
            addVertex(Vertices[7] + vertexOffset);
        }
        if (front)
        {
            surfaceTool.SetNormal(new Vector3(0, 0, 1));
            addVertex(Vertices[3] + vertexOffset);
            addVertex(Vertices[6] + vertexOffset);
            addVertex(Vertices[2] + vertexOffset);
            addVertex(Vertices[3] + vertexOffset);
            addVertex(Vertices[7] + vertexOffset);
            addVertex(Vertices[6] + vertexOffset);
        }
        if (back)
        {
            surfaceTool.SetNormal(new Vector3(0, 0, -1));
            addVertex(Vertices[0] + vertexOffset);
            addVertex(Vertices[1] + vertexOffset);
            addVertex(Vertices[5] + vertexOffset);
            addVertex(Vertices[5] + vertexOffset);
            addVertex(Vertices[4] + vertexOffset);
            addVertex(Vertices[0] + vertexOffset);
        }
        if (bottom)
        {
            surfaceTool.SetNormal(new Vector3(0, 1, 0));
            addVertex(Vertices[1] + vertexOffset);
            addVertex(Vertices[3] + vertexOffset);
            addVertex(Vertices[2] + vertexOffset);
            addVertex(Vertices[1] + vertexOffset);
            addVertex(Vertices[0] + vertexOffset);
            addVertex(Vertices[3] + vertexOffset);
        }
    }
}

