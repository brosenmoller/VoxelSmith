using Godot;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;

//using Godot;

//public partial class SurfaceMesh : MeshInstance3D
//{
//    [Export] private Material material;

//    private CollisionShape3D collisionShape;
//    private IMeshGenerator<VoxelColor> meshGenerator;

//    public void Setup()
//    {
//        collisionShape = GetParent().GetChildByType<CollisionShape3D>();
//        meshGenerator = new BasicMeshGenerator<VoxelColor>(material);
//    }

//    public void UpdateMesh()
//    {
//        Mesh = meshGenerator.CreateColorMesh(voxelData, GameManager.DataManager.PaletteData.paletteColors);
//        collisionShape.Shape = Mesh.CreateTrimeshShape();
//    }
//}

//using Godot;

//public partial class PrefabMesh : MeshInstance3D
//{
//    [Export] private Material material;

//    private CollisionShape3D collisionShape;
//    private IMeshGenerator<VoxelPrefab> meshGenerator;

//    public void Setup()
//    {
//        collisionShape = GetParent().GetChildByType<CollisionShape3D>();
//        meshGenerator = new BasicMeshGenerator<VoxelPrefab>(material);
//    }

//    public void UpdateMesh()
//    {
//        Mesh = meshGenerator.CreateColorMesh(GameManager.DataManager.ProjectData.voxelPrefabs, GameManager.DataManager.PaletteData.palletePrefabs);
//        collisionShape.Shape = Mesh.CreateTrimeshShape();
//    }
//}


public abstract partial class WorldMesh : Node3D
{
    [Export] protected PackedScene chunkScene;
    [Export] protected Material material;

    protected ChunkedMeshGenerator meshGenerator;
    protected uint currentCollisionLayer = 0;

    public void Setup()
    {
        meshGenerator = new ChunkedMeshGenerator(8);
    }

    public abstract void UpdateVoxel(Vector3I position, Guid guid);
    public abstract void ClearVoxel(Vector3I position);

    public abstract void UpdateAll();
    public abstract void UpdateAllOfGUID(Guid guid);

    public void SetCollisionLayerValue(int layerNumber, bool value)
    {
        bool first = true;
        foreach (Chunk chunk in meshGenerator.chunks.Values)
        {
            if (first)
            {
                currentCollisionLayer = chunk.CollisionLayer;
                first = false;
            }

            chunk.SetCollisionLayerValue(layerNumber, value);
        }
    }

    protected void UpdateAll(Dictionary<Vector3I, Guid> voxelData)
    {
        meshGenerator.ResetAll();

        foreach (Vector3I position in voxelData.Keys)
        {
            UpdateVoxel(position, voxelData[position]);
        }
    }

    protected void UpdateAllOfGUID(Guid guid, Dictionary<Vector3I, Guid> voxelData)
    {
        foreach (var voxel in voxelData)
        {
            if (voxel.Value != guid) { continue; }
            
            if (meshGenerator.chunks.TryGetValue(meshGenerator.GetChunkPosition(voxel.Key), out Chunk chunk))
            {
                meshGenerator.chunksToBeUpdated.Add(chunk);
            }
            else
            {
                GD.PrintErr("WorldMesh.cs: void UpdateAllOfGUID(): Chunk not found, but should always exist");
            }
        }
    }

    protected void UpdateMesh<TVoxelData>(HashSet<Vector3I> allPositions, Dictionary<Guid, TVoxelData> palette) where TVoxelData : VoxelData
    {
        meshGenerator.CreateMesh(allPositions, palette);
        meshGenerator.chunksToBeUpdated.Clear();
    }

    protected void UpdateVoxel(Vector3I position, Guid guid, Dictionary<Vector3I, Guid> voxelData)
    {
        if (voxelData.TryGetValue(position, out Guid existingGuid))
        {
            if (existingGuid == guid) { return; }
        }

        voxelData[position] = guid;

        Vector3I chunkPosition = meshGenerator.GetChunkPosition(position);

        if (!meshGenerator.chunks.TryGetValue(chunkPosition, out Chunk chunk))
        {
            chunk = chunkScene.Instantiate<Chunk>();
            
            if (currentCollisionLayer > 0) { chunk.CollisionLayer = currentCollisionLayer; }

            chunk.Setup();
            AddChild(chunk);

            meshGenerator.chunks.Add(chunkPosition, chunk);
        }

        chunk.chunkPositions[position] = guid;

        meshGenerator.chunksToBeUpdated.Add(chunk);
        CheckAdjacentChunksForUpdate(position, voxelData);
    }

    protected void ClearVoxel(Vector3I position, Dictionary<Vector3I, Guid> voxelData)
    {
        if (!voxelData.ContainsKey(position)) { return; }
        
        voxelData.Remove(position);

        Vector3I chunkPosition = meshGenerator.GetChunkPosition(position);
        if (meshGenerator.chunks.TryGetValue(chunkPosition, out Chunk chunk))
        {
            chunk.chunkPositions.Remove(position);
            meshGenerator.chunksToBeUpdated.Add(chunk);
            CheckAdjacentChunksForUpdate(position, voxelData);
        }
        else
        {
            GD.PrintErr("WorldMesh.cs: void ClearVoxel(): Chunk not found, but should always exist");
        }
    }

    private void CheckAdjacentChunksForUpdate(Vector3I position, Dictionary<Vector3I, Guid> voxelData)
    {
        for (int i = 0; i < CubeValues.cubeOffsets.Length; i++)
        {
            Vector3I offsetPosition = CubeValues.cubeOffsets[i] + position;

            if (!voxelData.ContainsKey(offsetPosition)) { continue; }
            
            if (meshGenerator.chunks.TryGetValue(meshGenerator.GetChunkPosition(offsetPosition), out Chunk chunk))
            {
                meshGenerator.chunksToBeUpdated.Add(chunk);
            }
            else
            {
                GD.PrintErr("WorldMesh.cs: void CheckAdjacentChunksForUpdate(): Chunk not found, but should always exist");
            }
        }
    }
}

