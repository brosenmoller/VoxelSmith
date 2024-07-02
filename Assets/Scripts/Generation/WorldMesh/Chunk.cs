using Godot;
using System;
using System.Collections.Generic;

public partial class Chunk : StaticBody3D
{
    [Export] public CollisionShape3D collisionShape;
    [Export] public MeshInstance3D meshInstance;
    [Export] public MeshInstance3D selectionMeshInstance;

    public Dictionary<Vector3I, Guid> chunkPositions;

    private SurfaceTool surfaceTool;
    private SurfaceTool selectionSurfaceTool;

    public void Setup()
    {
        surfaceTool = new SurfaceTool();
        selectionSurfaceTool = new SurfaceTool();
        chunkPositions = new Dictionary<Vector3I, Guid>();
    }

    public void UpdateChunkMesh<TVoxelData>(HashSet<Vector3I> allPositions, Dictionary<Guid, TVoxelData> palette) where TVoxelData : VoxelData
    {
        surfaceTool.Begin(Mesh.PrimitiveType.Triangles);
        selectionSurfaceTool.Begin(Mesh.PrimitiveType.Triangles);

        bool hasSelection = false;

        foreach (var voxel in chunkPositions)
        {
            SurfaceTool tool = GameManager.SelectionManager.CurrentSelection.Contains(voxel.Key) ? selectionSurfaceTool : surfaceTool;
            tool.SetColor(palette[voxel.Value].color);
            MeshHelper.CreateVoxel(tool, voxel.Key, allPositions);
            hasSelection = hasSelection || tool == selectionSurfaceTool;
        }

        ArrayMesh collisionMesh = new();
        meshInstance.Mesh = CreateMeshFromSurfaceTool(surfaceTool, collisionMesh);
        selectionMeshInstance.Mesh = hasSelection ? CreateMeshFromSurfaceTool(selectionSurfaceTool, collisionMesh) : null;
        collisionShape.Shape = collisionMesh.CreateTrimeshShape();
    }

    private ArrayMesh CreateMeshFromSurfaceTool(SurfaceTool tool, ArrayMesh collisionMesh)
    {
        tool.Index();
        Godot.Collections.Array surfaceArray = tool.CommitToArrays();
        ArrayMesh mesh = new();
        mesh.AddSurfaceFromArrays(Mesh.PrimitiveType.Triangles, surfaceArray);
        collisionMesh.AddSurfaceFromArrays(Mesh.PrimitiveType.Triangles, surfaceArray);
        return mesh;
    }
}
