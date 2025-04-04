using Godot;
using System;
using System.Collections.Generic;
using System.Text;

public class ExportObjMeshGenerator
{
    private const string FLOOR_MESH_NAME = "Floor";
    private const string CEILING_MESH_NAME = "Ceiling";
    private const string WALL_MESH_NAME = "Wall";

    private readonly Dictionary<Vector3I, ObjMeshSurface> defaultMeshes = new();
    private readonly Dictionary<Vector3I, ObjMeshSurface> floorMeshes = new();
    private readonly Dictionary<Vector3I, ObjMeshSurface> ceilingMeshes = new();

    private readonly ObjMeshSurface defaultMesh = new();
    private readonly ObjMeshSurface floorMesh = new();
    private readonly ObjMeshSurface ceilingMesh = new();

    private string name;
    private ExportSettingsData exportSettings;

    public string CreateMesh(ExportSettingsData exportSettings, Dictionary<Vector3I, Guid> voxels, Dictionary<Guid, VoxelColor> palette, string name)
    {
        this.name = name;
        this.exportSettings = exportSettings;

        defaultMeshes.Clear();
        floorMeshes.Clear();
        ceilingMeshes.Clear();

        defaultMesh.Reset(string.Empty);
        floorMesh.Reset(FLOOR_MESH_NAME);
        ceilingMesh.Reset(CEILING_MESH_NAME);

        HashSet<Vector3I> allVoxels = new(voxels.Keys);

        foreach (Vector3I voxel in allVoxels)
        {
            if (exportSettings.enableBarrierBlockCulling && palette[voxels[voxel]].minecraftIDlist.Contains("minecraft:barrier")) { continue; }

            Vector3I chunk = GetChunk(voxel);

            for (int i = 0; i < 6; i++)
            {
                Vector3I offset = CubeValues.cubeOffsets[i];
                if (allVoxels.Contains(voxel + offset)) { continue; }

                ObjMeshSurface meshSurface = GetMeshSurface(chunk, offset);
                ObjFace face = new(i);
                int[] vertexIndices = CubeValues.cubeVertexFaceIndicesExporter[i];
                for (int j = 0; j < vertexIndices.Length; j++)
                {
                    Vector3I vertexPosition = voxel + CubeValues.cubeVertices[vertexIndices[j]];
                    face.vertexIndices[j] = meshSurface.GetVertexIndex(vertexPosition);
                }

                meshSurface.AddFace(face, voxel, offset);
            }
        }

        GreedyMeshIfNeeded();

        return ConstructObj();
    }

    private void GreedyMeshIfNeeded()
    {
        if (exportSettings.enableGreedyMeshing)
        {
            if (exportSettings.enableChunkedMeshing)
            {
                if (exportSettings.enableSeparateFloorAndCeiling)
                {
                    foreach (var meshSurface in ceilingMeshes) { ObjGreedyMesher.GreedyMesh(meshSurface.Value); }
                    foreach (var meshSurface in floorMeshes) { ObjGreedyMesher.GreedyMesh(meshSurface.Value); }
                }

                foreach (var meshSurface in defaultMeshes) { ObjGreedyMesher.GreedyMesh(meshSurface.Value); }
            }
            else
            {
                if (exportSettings.enableSeparateFloorAndCeiling)
                {
                    if (floorMesh != null) { ObjGreedyMesher.GreedyMesh(floorMesh); }
                    if (ceilingMesh != null) { ObjGreedyMesher.GreedyMesh(ceilingMesh); }
                }

                if (defaultMesh != null) { ObjGreedyMesher.GreedyMesh(defaultMesh); }
            }
        }
    }

    private string ConstructObj()
    {
        StringBuilder obj = new();
        obj.AppendLine("# Mesh Exported Using VoxelSmith");
        obj.AppendLine("# https://github.com/brosenmoller/VoxelSmith");
        obj.AppendLine($"g {name}");

        for (int i = 0; i < CubeValues.cubeOffsets.Length; i++)
        {
            obj.AppendLine($"vn {CubeValues.cubeOffsets[i].X} {CubeValues.cubeOffsets[i].Y} {CubeValues.cubeOffsets[i].Z}");
        }

        for (int i = 0; i < CubeValues.cubeUVs.Length; i++)
        {
            obj.AppendLine($"vt {CubeValues.cubeUVs[i].X} {CubeValues.cubeUVs[i].Y}");
        }

        int startingIndex = 1;
        void AddObject(ObjMeshSurface surface)
        {
            obj.Append(surface.ConvertToObj(startingIndex));
            startingIndex += surface.VertexCount;
        }

        if (exportSettings.enableChunkedMeshing)
        {
            if (exportSettings.enableSeparateFloorAndCeiling)
            {
                foreach (ObjMeshSurface meshSurface in floorMeshes.Values) { AddObject(meshSurface); }
                foreach (ObjMeshSurface meshSurface in ceilingMeshes.Values) { AddObject(meshSurface); }
            }

            foreach (ObjMeshSurface meshSurface in defaultMeshes.Values) { AddObject(meshSurface); }
        }
        else
        {
            if (exportSettings.enableSeparateFloorAndCeiling)
            {
                AddObject(floorMesh);
                AddObject(ceilingMesh);
            }

            AddObject(defaultMesh);
        }

        return obj.ToString();
    }

    private ObjMeshSurface GetMeshSurface(Vector3I chunk, Vector3I offset)
    {
        if (exportSettings.enableChunkedMeshing)
        {
            if (exportSettings.enableSeparateFloorAndCeiling)
            {
                if (offset.Equals(Vector3I.Up)) { return GetMeshSurfaceFromDictionary(chunk, floorMeshes, FLOOR_MESH_NAME); }
                else if (offset.Equals(Vector3I.Down)) { return GetMeshSurfaceFromDictionary(chunk, ceilingMeshes, CEILING_MESH_NAME); }
            }

            return GetMeshSurfaceFromDictionary(chunk, defaultMeshes, WALL_MESH_NAME);
        }

        if (exportSettings.enableSeparateFloorAndCeiling)
        {
            if (offset.Equals(Vector3I.Up)) { return floorMesh; }
            else if (offset.Equals(Vector3I.Down)) { return ceilingMesh; }
        }

        return defaultMesh;
    }

    private static ObjMeshSurface GetMeshSurfaceFromDictionary(Vector3I chunk, Dictionary<Vector3I, ObjMeshSurface> meshDictionary, string baseName)
    {
        if (!meshDictionary.TryGetValue(chunk, out ObjMeshSurface surface))
        {
            surface = new ObjMeshSurface($"{baseName}_({chunk.X},{chunk.Y},{chunk.Z})");
            meshDictionary.Add(chunk, surface);
        }
        return surface;
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
}