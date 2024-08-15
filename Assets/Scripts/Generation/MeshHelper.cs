using Godot;
using System.Collections.Generic;

public static class MeshHelper
{
    public static void CreateVoxel(SurfaceTool surfaceTool, Vector3I position, HashSet<Vector3I> allPositions, bool unityUVS = false)
    {
        void AddFace(ref Vector3I normal, ref int[] vertexIndices, ref int[] uvIndices)
        {
            surfaceTool.SetNormal(normal);
            for (int i = 0; i < vertexIndices.Length; i++)
            {
                surfaceTool.SetUV(CubeValues.cubeUVs[uvIndices[i]]);
                surfaceTool.AddVertex(CubeValues.cubeVertices[vertexIndices[i]] + position);
            }
        }

        for (int i = 0; i < 6; i++)
        {
            if (!allPositions.Contains(position + CubeValues.cubeOffsets[i]))
            {
                if (unityUVS)
                {
                    AddFace(ref CubeValues.cubeOffsets[i], ref CubeValues.cubeVertexFaceIndices[i], ref CubeValues.cubeUVFaceIndicesUnity[i]);
                }
                else
                {
                    AddFace(ref CubeValues.cubeOffsets[i], ref CubeValues.cubeVertexFaceIndices[i], ref CubeValues.cubeUVFaceIndices[i]);
                }
            }
        }
    }

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

        void AddFace(Vector3 normal, int[] vertexIndices, int[] uvIndices)
        {
            surfaceTool.SetNormal(normal);
            for (int i = 0; i < vertexIndices.Length; i++)
            {
                surfaceTool.SetUV(CubeValues.cubeUVs[uvIndices[i]]);
                surfaceTool.AddVertex(CubeValues.cubeVertices[vertexIndices[i]] * size + point1);
            }
        }

        for (int i = 0; i < 6; i++)
        {
            AddFace(CubeValues.cubeOffsets[i], CubeValues.cubeVertexFaceIndices[i], CubeValues.cubeUVFaceIndices[i]);
        }

        surfaceTool.Index();
        return surfaceTool.Commit();
    }
}
