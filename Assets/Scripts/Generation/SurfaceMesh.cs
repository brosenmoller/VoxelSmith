using Godot;
using System.Collections.Generic;

public partial class SurfaceMesh : MeshInstance3D
{
    public float VoxelSize = 1f;
    public Material DefaultMaterial = new StandardMaterial3D() { VertexColorUseAsAlbedo = true };

    private Dictionary<Vector3I, VoxelData> Voxels => GameManager.DataManager.ProjectData.voxels;

    private SurfaceTool SurfaceTool = new();

    private static readonly Vector3[] Vertices = {
        new(0, 0, 0), new(1, 0, 0),
        new(1, 0, 1), new(0, 0, 1),
        new(0, 1, 0), new(1, 1, 0),
        new(1, 1, 1), new(0, 1, 1)
    };

    private CollisionShape3D collisionShape;

    public void UpdateMesh()
    {
        Mesh = CreateMesh();
        collisionShape ??= GetParent().GetChildByType<CollisionShape3D>();
        collisionShape.Shape = Mesh.CreateTrimeshShape();
    }

    private Mesh CreateMesh()
    {
        SurfaceTool.Begin(Mesh.PrimitiveType.Triangles);
        SurfaceTool.SetMaterial(DefaultMaterial);

        foreach (var voxel in Voxels.Keys)
            CreateVoxel(Color.Color8(200, 100, 0, 255), voxel);

        SurfaceTool.Index();
        return SurfaceTool.Commit();
    }

    private void CreateVoxel(Color color, Vector3I position)
    {
        bool left = !Voxels.ContainsKey(position +      new Vector3I(-1, 0, 0));
        bool right = !Voxels.ContainsKey(position +     new Vector3I(1, 0, 0));
        bool back = !Voxels.ContainsKey(position +      new Vector3I(0, 0, -1));
        bool front = !Voxels.ContainsKey(position +     new Vector3I(0, 0, 1));
        bool top = !Voxels.ContainsKey(position +       new Vector3I(0, 1, 0));
        bool bottom = !Voxels.ContainsKey(position +    new Vector3I(0, -1, 0));

        SurfaceTool.SetColor(color);

        void addVertex(Vector3 pos) => SurfaceTool.AddVertex(pos * VoxelSize);

        Vector3 vertexOffset = position;
        if (top)
        {
            SurfaceTool.SetNormal(new Vector3(0, -1, 0));
            addVertex(Vertices[4] + vertexOffset);
            addVertex(Vertices[5] + vertexOffset);
            addVertex(Vertices[7] + vertexOffset);
            addVertex(Vertices[5] + vertexOffset);
            addVertex(Vertices[6] + vertexOffset);
            addVertex(Vertices[7] + vertexOffset);
        }
        if (right)
        {
            SurfaceTool.SetNormal(new Vector3(1, 0, 0));
            addVertex(Vertices[2] + vertexOffset);
            addVertex(Vertices[5] + vertexOffset);
            addVertex(Vertices[1] + vertexOffset);
            addVertex(Vertices[2] + vertexOffset);
            addVertex(Vertices[6] + vertexOffset);
            addVertex(Vertices[5] + vertexOffset);

        }
        if (left)
        {
            SurfaceTool.SetNormal(new Vector3(-1, 0, 0));
            addVertex(Vertices[0] + vertexOffset);
            addVertex(Vertices[7] + vertexOffset);
            addVertex(Vertices[3] + vertexOffset);
            addVertex(Vertices[0] + vertexOffset);
            addVertex(Vertices[4] + vertexOffset);
            addVertex(Vertices[7] + vertexOffset);
        }
        if (front)
        {
            SurfaceTool.SetNormal(new Vector3(0, 0, 1));
            addVertex(Vertices[3] + vertexOffset);
            addVertex(Vertices[6] + vertexOffset);
            addVertex(Vertices[2] + vertexOffset);
            addVertex(Vertices[3] + vertexOffset);
            addVertex(Vertices[7] + vertexOffset);
            addVertex(Vertices[6] + vertexOffset);
        }
        if (back)
        {
            SurfaceTool.SetNormal(new Vector3(0, 0, -1));
            addVertex(Vertices[0] + vertexOffset);
            addVertex(Vertices[1] + vertexOffset);
            addVertex(Vertices[5] + vertexOffset);
            addVertex(Vertices[5] + vertexOffset);
            addVertex(Vertices[4] + vertexOffset);
            addVertex(Vertices[0] + vertexOffset);
        }
        if (bottom)
        {
            SurfaceTool.SetNormal(new Vector3(0, 1, 0));
            addVertex(Vertices[1] + vertexOffset);
            addVertex(Vertices[3] + vertexOffset);
            addVertex(Vertices[2] + vertexOffset);
            addVertex(Vertices[1] + vertexOffset);
            addVertex(Vertices[0] + vertexOffset);
            addVertex(Vertices[3] + vertexOffset);
        }
    }
}
