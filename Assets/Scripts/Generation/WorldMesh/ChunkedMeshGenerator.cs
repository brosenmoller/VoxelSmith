using Godot;
using System;
using System.Collections.Generic;

public class ChunkedMeshGenerator
{
    private readonly int chunkSize;
    public Dictionary<Vector3I, Chunk> chunks;
    public HashSet<Chunk> chunksToBeUpdated;

    public Vector3I GetChunkPosition(Vector3I position)
    {
        return new Vector3I(
            Mathf.FloorToInt(position.X / chunkSize),
            Mathf.FloorToInt(position.Y / chunkSize),
            Mathf.FloorToInt(position.Z / chunkSize)
        );
    }

    public ChunkedMeshGenerator(int chunkSize)
    {
        this.chunkSize = chunkSize;
        chunks = new Dictionary<Vector3I, Chunk>();
        chunksToBeUpdated = new HashSet<Chunk>();
    }

    public void CreateMesh<TVoxelData>(HashSet<Vector3I> allPositions, Dictionary<Guid, TVoxelData> palette) where TVoxelData : VoxelData
    {
        foreach (var chunkData in chunks)
        {
            Chunk chunk = chunkData.Value;

            if (chunksToBeUpdated.Contains(chunk))
            {
                if (chunk.chunkPositions.Count <= 0)
                {
                    chunks.Remove(chunkData.Key);
                    chunk.QueueFree();
                    continue;
                }

                chunk.UpdateChunkMesh(allPositions, palette);
            }
        }
    }

    public void ResetAll()
    {
        foreach (var chunkData in chunks)
        {
            chunkData.Value.QueueFree();
        }

        chunks.Clear();
        chunksToBeUpdated.Clear();
    }
}
