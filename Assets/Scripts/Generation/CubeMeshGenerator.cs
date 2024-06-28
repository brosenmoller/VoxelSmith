using Godot;

public static class CubeMeshGenerator
{
    public static Mesh CreateCubeMesh(Vector3I voxel1, Vector3I voxel2)
    {
        Vector3 point1 = new(
            Mathf.Min(voxel1.X, voxel2.X) - 0.01f,
            Mathf.Min(voxel1.Y, voxel2.Y) - 0.01f,
            Mathf.Min(voxel1.Z, voxel2.Z) - 0.01f
        );

        Vector3 point2 = new(
            Mathf.Max(voxel1.X, voxel2.X) + 1.01f,
            Mathf.Max(voxel1.Y, voxel2.Y) + 1.01f,
            Mathf.Max(voxel1.Z, voxel2.Z) + 1.01f
        );

        Vector3 size = new(
            point2.X - point1.X,
            point2.Y - point1.Y,
            point2.Z - point1.Z
        );

        SurfaceTool surfaceTool = new();
        surfaceTool.Begin(Mesh.PrimitiveType.Triangles);

        // Left Face
        surfaceTool.SetNormal(new Vector3(-1, 0, 0));
        surfaceTool.AddVertex(CubeValues.cubeVertices[0] * size + point1);
        surfaceTool.AddVertex(CubeValues.cubeVertices[7] * size + point1);
        surfaceTool.AddVertex(CubeValues.cubeVertices[3] * size + point1);
        surfaceTool.AddVertex(CubeValues.cubeVertices[0] * size + point1);
        surfaceTool.AddVertex(CubeValues.cubeVertices[4] * size + point1);
        surfaceTool.AddVertex(CubeValues.cubeVertices[7] * size + point1);

        // Right Face
        surfaceTool.SetNormal(new Vector3(1, 0, 0));
        surfaceTool.AddVertex(CubeValues.cubeVertices[2] * size + point1);
        surfaceTool.AddVertex(CubeValues.cubeVertices[5] * size + point1);
        surfaceTool.AddVertex(CubeValues.cubeVertices[1] * size + point1);
        surfaceTool.AddVertex(CubeValues.cubeVertices[2] * size + point1);
        surfaceTool.AddVertex(CubeValues.cubeVertices[6] * size + point1);
        surfaceTool.AddVertex(CubeValues.cubeVertices[5] * size + point1);

        // Bottom Face
        surfaceTool.SetNormal(new Vector3(0, 1, 0));
        surfaceTool.AddVertex(CubeValues.cubeVertices[1] * size + point1);
        surfaceTool.AddVertex(CubeValues.cubeVertices[3] * size + point1);
        surfaceTool.AddVertex(CubeValues.cubeVertices[2] * size + point1);
        surfaceTool.AddVertex(CubeValues.cubeVertices[1] * size + point1);
        surfaceTool.AddVertex(CubeValues.cubeVertices[0] * size + point1);
        surfaceTool.AddVertex(CubeValues.cubeVertices[3] * size + point1);

        // Top Face
        surfaceTool.SetNormal(new Vector3(0, -1, 0));
        surfaceTool.AddVertex(CubeValues.cubeVertices[4] * size + point1);
        surfaceTool.AddVertex(CubeValues.cubeVertices[5] * size + point1);
        surfaceTool.AddVertex(CubeValues.cubeVertices[7] * size + point1);
        surfaceTool.AddVertex(CubeValues.cubeVertices[5] * size + point1);
        surfaceTool.AddVertex(CubeValues.cubeVertices[6] * size + point1);
        surfaceTool.AddVertex(CubeValues.cubeVertices[7] * size + point1);

        // Back Face
        surfaceTool.SetNormal(new Vector3(0, 0, -1));
        surfaceTool.AddVertex(CubeValues.cubeVertices[0] * size + point1);
        surfaceTool.AddVertex(CubeValues.cubeVertices[1] * size + point1);
        surfaceTool.AddVertex(CubeValues.cubeVertices[5] * size + point1);
        surfaceTool.AddVertex(CubeValues.cubeVertices[5] * size + point1);
        surfaceTool.AddVertex(CubeValues.cubeVertices[4] * size + point1);
        surfaceTool.AddVertex(CubeValues.cubeVertices[0] * size + point1);

        // Front Face
        surfaceTool.SetNormal(new Vector3(0, 0, 1));
        surfaceTool.AddVertex(CubeValues.cubeVertices[3] * size + point1);
        surfaceTool.AddVertex(CubeValues.cubeVertices[6] * size + point1);
        surfaceTool.AddVertex(CubeValues.cubeVertices[2] * size + point1);
        surfaceTool.AddVertex(CubeValues.cubeVertices[3] * size + point1);
        surfaceTool.AddVertex(CubeValues.cubeVertices[7] * size + point1);
        surfaceTool.AddVertex(CubeValues.cubeVertices[6] * size + point1);

        surfaceTool.Index();
        return surfaceTool.Commit();
    }
}
