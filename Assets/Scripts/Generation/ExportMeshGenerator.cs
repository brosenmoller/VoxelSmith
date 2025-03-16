using Godot;
using System;
using System.Collections.Generic;
using System.Text;

public class ExportMeshGenerator
{
    public static string CreateMesh(ExportSettingsData exportSettings, Dictionary<Vector3I, Guid> voxels, Dictionary<Guid, VoxelColor> palette)
    {
        Dictionary<Vector3I, MeshSurface> wallMeshes = new();
        Dictionary<Vector3I, MeshSurface> floorMeshes = new();
        Dictionary<Vector3I, MeshSurface> ceilingMeshes = new();

        static MeshSurface GetMeshSurface(Vector3I chunk, Dictionary<Vector3I, MeshSurface> meshDictionary, string baseName)
        {
            if (!meshDictionary.TryGetValue(chunk, out MeshSurface surface))
            {
                surface = new MeshSurface($"{baseName}_({chunk.X},{chunk.Y},{chunk.Z})");
                meshDictionary.Add(chunk, surface);
            }
            return surface;
        }

        HashSet<Vector3I> allVoxels = new(voxels.Keys);

        foreach (Vector3I voxel in voxels.Keys)
        {
            if (exportSettings.enableBarrierBlockCulling && palette[voxels[voxel]].minecraftIDlist.Contains("minecraft:barrier")) { continue; }

            Vector3I chunk = GetChunk(voxel);

            for (int i = 0; i < 6; i++)
            {
                Vector3I offset = CubeValues.cubeOffsets[i];
                if (allVoxels.Contains(voxel + offset)) { continue; }

                MeshSurface meshSurface;
                if (offset.Equals(Vector3I.Up)) { meshSurface = GetMeshSurface(chunk, floorMeshes, "Floor"); }
                else if (offset.Equals(Vector3I.Down)) { meshSurface = GetMeshSurface(chunk, ceilingMeshes, "Ceiling"); }
                else { meshSurface = GetMeshSurface(chunk, wallMeshes, "Walls"); }

                Face face = new(i);
                int[] vertexIndices = CubeValues.cubeVertexSquareFaceIndices[i];
                for (int j = 0; j < vertexIndices.Length; j++)
                {
                    Vector3I vertexPosition = voxel + CubeValues.cubeVertices[vertexIndices[j]];
                    face.vertexIndices[j] = meshSurface.GetVertexIndex(vertexPosition);
                }

                meshSurface.faces.Add(face);
            }
        }

        StringBuilder obj = new();
        obj.AppendLine("# Mesh Exported Using VoxelSmith");
        obj.AppendLine("# https://github.com/brosenmoller/VoxelSmith");

        foreach (MeshSurface meshSurface in wallMeshes.Values) { obj.Append(meshSurface.ConvertToObj()); }
        foreach (MeshSurface meshSurface in floorMeshes.Values) { obj.Append(meshSurface.ConvertToObj()); }
        foreach (MeshSurface meshSurface in ceilingMeshes.Values) { obj.Append(meshSurface.ConvertToObj()); }

        return obj.ToString();
    }

    private static Vector3I GetChunk(Vector3I voxel)
    {
        const int CHUNK_SIZE = 16;
        return new Vector3I(
            Mathf.FloorToInt(voxel.X / CHUNK_SIZE),
            Mathf.FloorToInt(voxel.Y / CHUNK_SIZE),
            Mathf.FloorToInt(voxel.Z / CHUNK_SIZE)
        );
    }

    public class Face
    {
        public int normalIndex;
        public readonly int[] vertexIndices = new int[4];

        public Face(int normalIndex)
        {
            this.normalIndex = normalIndex;
        }

        public (int[], int[]) GetTriangles()
        {
            int[] triangle1 = new int[] { vertexIndices[0] + 1, vertexIndices[1] + 1, vertexIndices[2] + 1 };
            int[] triangle2 = new int[] { vertexIndices[0] + 1, vertexIndices[2] + 1, vertexIndices[3] + 1 };
            return (triangle1, triangle2);
        }

        public int GetNormal()
        {
            return normalIndex + 1;
        }
    }

    private class MeshSurface
    {
        public readonly List<Face> faces = new();

        private readonly List<Vector3I> vertices = new();
        private readonly Dictionary<Vector3I, int> vertexMap = new();

        public readonly string name;

        public MeshSurface(string name)
        {
            this.name = name;
        }

        public int GetVertexIndex(Vector3I vertex)
        {
            if (!vertexMap.TryGetValue(vertex, out int index))
            {
                index = vertices.Count;
                vertexMap[vertex] = index;
                vertices.Add(vertex);
            }
            return index;
        }

        public string ConvertToObj()
        {
            StringBuilder obj = new();
            obj.AppendLine("o " + name);

            for (int i = 0; i < CubeValues.cubeOffsets.Length; i++)
            {
                obj.AppendLine($"vn {CubeValues.cubeOffsets[i].X} {CubeValues.cubeOffsets[i].Y} {CubeValues.cubeOffsets[i].Z}");
            }

            for (int i = 0; i < CubeValues.cubeUVs.Length; i++)
            {
                obj.AppendLine($"vt {CubeValues.cubeUVs[i].X} {CubeValues.cubeUVs[i].Y}");
            }

            for (int i = 0; i < vertices.Count; i++)
            {
                obj.AppendLine($"v {vertices[i].X} {vertices[i].Y} {vertices[i].Z}");
            }

            for (int i = 0; i < faces.Count; i++)
            {
                Face face = faces[i];
                (int[] triangle1, int[] triangle2) = face.GetTriangles();
                int normal = face.GetNormal();

                int[] uvIndices = CubeValues.cubeUVFaceIndicesUnity[face.normalIndex];

                obj.AppendLine($"f {triangle1[0]}/{uvIndices[0]}/{normal} {triangle1[1]}/{3}/{normal} {triangle1[2]}/{4}/{normal}");
                obj.AppendLine($"f {triangle2[0]}/{uvIndices[3]}/{normal} {triangle2[1]}/{uvIndices[4]}/{normal} {triangle2[2]}/{uvIndices[5]}/{normal}");
            }

            return obj.ToString();
        }
    }
}