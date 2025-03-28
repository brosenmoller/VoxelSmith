using Godot;
using System.Collections.Generic;
using System.Text;

public class ObjMeshSurface
{
    public readonly List<ObjFace> faces = new();
    public readonly Dictionary<(Vector3I, Vector3I), ObjFace> faceMapVoxelKeyed = new();
    public readonly Dictionary<ObjFace, (Vector3I, Vector3I)> faceMapFaceKeyed = new();

    private readonly List<Vector3I> vertices = new();
    private readonly Dictionary<Vector3I, int> vertexMap = new();

    public string name { get; private set; }

    public int VertexCount => vertices.Count;

    public ObjMeshSurface() 
    { 
        name = "";
    }

    public ObjMeshSurface(string name)
    {
        this.name = name;
    }

    public void Reset(string name)
    {
        this.name = name;
        faces.Clear();
        faceMapFaceKeyed.Clear();
        faceMapVoxelKeyed.Clear();
        vertices.Clear();
        vertexMap.Clear();
    }

    public void AddFace(ObjFace face, Vector3I voxel, Vector3I normal)
    {
        faces.Add(face);
        faceMapFaceKeyed.Add(face, (voxel, normal));
        faceMapVoxelKeyed.Add((voxel, normal), face);
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

    public string ConvertToObj(int startingIndex)
    {
        if (faces.Count <= 0 || vertices.Count <= 0) { return string.Empty; }

        StringBuilder obj = new();
        obj.AppendLine("g " + name);

        for (int i = 0; i < vertices.Count; i++)
        {
            obj.AppendLine($"v {vertices[i].X} {vertices[i].Y} {vertices[i].Z}");
        }

        for (int i = 0; i < faces.Count; i++)
        {
            ObjFace face = faces[i];
            int normal = face.GetNormal();

            int[] uvIndices = CubeValues.cubeUVFaceIndicesUnity[face.normalIndex];

            obj.AppendLine($"f {face.vertexIndices[0] + startingIndex}/{uvIndices[0] + 1}/{normal} {face.vertexIndices[1] + startingIndex}/{uvIndices[1] + 1}/{normal} {face.vertexIndices[2] + startingIndex}/{uvIndices[2] + 1}/{normal}");
            obj.AppendLine($"f {face.vertexIndices[3] + startingIndex}/{uvIndices[3] + 1}/{normal} {face.vertexIndices[4] + startingIndex}/{uvIndices[4] + 1}/{normal} {face.vertexIndices[5] + startingIndex}/{uvIndices[5] + 1}/{normal}");
        }

        return obj.ToString();
    }
}