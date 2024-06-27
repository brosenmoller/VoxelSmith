using Godot;
using System;
using System.Collections.Generic;

public abstract partial class WorldMesh : Node3D
{
    [Export] private Material material;
    [Export] protected PackedScene chunkScene;
    [Export(PropertyHint.Layers3DPhysics)] private uint collisionLayer;

    protected ChunkedMeshGenerator meshGenerator;

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
        layerNumber--;
        value = !value;

        if (layerNumber < 0 || layerNumber > 31)
        {
            throw new ArgumentOutOfRangeException(nameof(layerNumber), "Layer number must be between 0 and 31.");
        }

        if (value)
        {
            // Set the bit at layerNumber to 1
            collisionLayer |= (1u << layerNumber);
        }
        else
        {
            // Set the bit at layerNumber to 0
            collisionLayer &= ~(1u << layerNumber);
        }

        foreach (Chunk chunk in meshGenerator.chunks.Values)
        {
            chunk.CollisionLayer = collisionLayer;
            GD.Print(chunk.GetCollisionLayerValue(layerNumber));
        }
    }

    protected void UpdateAll(Dictionary<Vector3I, Guid> voxelData)
    {
        //UpdateAllDelayed(voxelData);
        meshGenerator.ResetAll();

        foreach (Vector3I position in voxelData.Keys)
        {
            UpdateChunkVoxel(position, voxelData[position], voxelData);
        }
    }

    //private async void UpdateAllDelayed(Dictionary<Vector3I, Guid> voxelData)
    //{
    //    GD.Print("Start");
    //    //meshGenerator.ResetAll();
    //    await Task.Delay(1000);

    //    GD.Print("Update All");
    //    GD.Print("VoxelData Count " + voxelData.Count);

    //    foreach (Vector3I position in voxelData.Keys)
    //    {
    //        UpdateVoxel(position, voxelData[position]);
    //    }
    //}

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

        UpdateChunkVoxel(position, guid, voxelData);
    }

    private void UpdateChunkVoxel(Vector3I position, Guid guid, Dictionary<Vector3I, Guid> voxelData)
    {
        Vector3I chunkPosition = meshGenerator.GetChunkPosition(position);

        if (!meshGenerator.chunks.TryGetValue(chunkPosition, out Chunk chunk))
        {
            chunk = chunkScene.Instantiate<Chunk>();
            chunk.CollisionLayer = collisionLayer;
            chunk.meshInstance.MaterialOverride = material;
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

