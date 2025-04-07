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
    Dictionary<Vector3I, Guid> voxels;
    Dictionary<Guid, VoxelColor> palette;

    public string CreateMesh(ExportSettingsData exportSettings, Dictionary<Vector3I, Guid> voxels, Dictionary<Guid, VoxelColor> palette, string name)
    {
        this.name = name;
        this.exportSettings = exportSettings;
        this.voxels = voxels;
        this.palette = palette;

        defaultMeshes.Clear();
        floorMeshes.Clear();
        ceilingMeshes.Clear();

        defaultMesh.Reset(string.Empty);
        floorMesh.Reset(FLOOR_MESH_NAME);
        ceilingMesh.Reset(CEILING_MESH_NAME);

        if (exportSettings.enableGreedyMeshing) { GreedyMesher(); }
        else { StandardMesher(); }

        return ConstructObj();
    }

    private void StandardMesher()
    {
        HashSet<Vector3I> allVoxels = new(voxels.Keys);

        foreach (Vector3I voxel in allVoxels)
        {
            if (exportSettings.enableBarrierBlockCulling &&
                palette[voxels[voxel]].minecraftIDlist.Contains(PaletteDataFactory.MINECRAFT_BARRIER)
            ) { continue; }

            Vector3I chunk = GetChunk(voxel);

            for (int i = 0; i < 6; i++)
            {
                Vector3I normal = CubeValues.cubeOffsets[i];
                if (allVoxels.Contains(voxel + normal)) { continue; }

                ObjMeshSurface meshSurface = GetMeshSurface(chunk, normal);
                ObjFace face = new(i);
                int[] vertexIndices = CubeValues.cubeVertexFaceIndicesExporter[i];
                for (int j = 0; j < vertexIndices.Length; j++)
                {
                    Vector3I vertexPosition = voxel + CubeValues.cubeVertices[vertexIndices[j]];
                    face.vertexIndices[j] = meshSurface.GetVertexIndex(vertexPosition);
                }

                meshSurface.AddFace(face);
            }
        }
    }

    private static (Vector3I primary, Vector3I secondary) GetDirections(Vector3I normal)
    {
        return normal switch
        {
            (1, 0, 0) => (new Vector3I(0, 1, 0), new Vector3I(0, 0, 1)),
            (-1, 0, 0) => (new Vector3I(0, -1, 0), new Vector3I(0, 0, -1)),
            (0, 1, 0) => (new Vector3I(1, 0, 0), new Vector3I(0, 0, 1)),
            (0, -1, 0) => (new Vector3I(-1, 0, 0), new Vector3I(0, 0, -1)),
            (0, 0, 1) => (new Vector3I(1, 0, 0), new Vector3I(0, 1, 0)),
            (0, 0, -1) => (new Vector3I(-1, 0, 0), new Vector3I(0, -1, 0)),
            _ => throw new ArgumentException("Invalid normal vector")
        };
    }

    private void GreedyMesher()
    {
        HashSet<Vector3I> allVoxels = new(voxels.Keys);
        HashSet<FaceData> allProcessedFaces = new();

        foreach (Vector3I voxel in allVoxels)
        {
            if (exportSettings.enableBarrierBlockCulling &&
                palette[voxels[voxel]].minecraftIDlist.Contains(PaletteDataFactory.MINECRAFT_BARRIER)
            ) { continue; }

            Vector3I chunk = GetChunk(voxel);

            for (int i = 0; i < 6; i++)
            {
                Vector3I normal = CubeValues.cubeOffsets[i];
                if (allVoxels.Contains(voxel + normal)) { continue; }

                FaceData faceData = new(voxel, normal);
                if (allProcessedFaces.Contains(faceData)) { continue; }
                allProcessedFaces.Add(faceData);

                (Vector3I primaryDirection, Vector3I secondaryDirection) = GetDirections(normal);

                ObjMeshSurface meshSurface = GetMeshSurface(chunk, normal);

                List<FaceData> facesPrimaryDirection = new() { faceData };
                Vector3I currentPrimaryStep = Vector3I.Zero;

                while (true)
                {
                    Vector3I newStep = currentPrimaryStep + primaryDirection;
                    FaceData faceToBeMerged = new(voxel + newStep, normal);
                    if (!allVoxels.Contains(faceToBeMerged.position)) { break; }
                    if (GetChunk(faceToBeMerged.position) != chunk) { break; }
                    if (allProcessedFaces.Contains(faceToBeMerged)) { break; }

                    currentPrimaryStep = newStep;
                    facesPrimaryDirection.Add(faceToBeMerged);
                    allProcessedFaces.Add(faceToBeMerged);
                }

                Vector3I currentSecondaryStep = Vector3I.Zero;

                bool IsSecondaryDirectionMergeable(Vector3I step)
                {
                    for (int i = 0; i < facesPrimaryDirection.Count; i++)
                    {
                        FaceData faceToBeMerged = new(facesPrimaryDirection[i].position + step, normal);

                        if (!allVoxels.Contains(faceToBeMerged.position)) { return false; }
                        if (allProcessedFaces.Contains(faceToBeMerged)) { return false; }
                        if (GetChunk(faceToBeMerged.position) != chunk) { return false; }
                    }

                    return true;
                }

                while (true)
                {
                    Vector3I nextStep = currentSecondaryStep + secondaryDirection;

                    if (!IsSecondaryDirectionMergeable(nextStep)) { break; }

                    currentSecondaryStep = nextStep;

                    for (int faceIndex = 0; faceIndex < facesPrimaryDirection.Count; faceIndex++)
                    {
                        FaceData newFace = facesPrimaryDirection[i];
                        newFace.position += currentSecondaryStep;
                        allProcessedFaces.Add(newFace);
                    }
                }

                ObjFace face = new(i);
                int[] vertexIndices = CubeValues.cubeVertexFaceIndicesExporter[i];
                for (int j = 0; j < vertexIndices.Length; j++)
                {
                    Vector3I vertexPosition = voxel + CubeValues.cubeVertices[vertexIndices[j]];
                    face.vertexIndices[j] = meshSurface.GetVertexIndex(vertexPosition);
                }

                meshSurface.AddFace(face);
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
        const int CHUNK_SIZE = 32;
        return new Vector3I(
            Mathf.FloorToInt(voxel.X / CHUNK_SIZE),
            Mathf.FloorToInt(voxel.Y / CHUNK_SIZE),
            Mathf.FloorToInt(voxel.Z / CHUNK_SIZE)
        );
    }

    private struct FaceData
    {
        public Vector3I position;
        public Vector3I normal;

        public FaceData(Vector3I position, Vector3I normal)
        {
            this.position = position;
            this.normal = normal;
        }
    }
}