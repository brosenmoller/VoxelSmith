using Godot;
using System;

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
//        Mesh = meshGenerator.CreateColorMesh(GameManager.DataManager.ProjectData.voxelColors, GameManager.DataManager.PaletteData.paletteColors);
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


public abstract partial class WorldMesh<TVoxelData> : MeshInstance3D where TVoxelData : VoxelData
{
    [Export] protected Material material;

    protected CollisionShape3D collisionShape;
    protected ChunkedMeshGenerator<TVoxelData> meshGenerator;

    public void Setup()
    {
        collisionShape = GetParent().GetChildByType<CollisionShape3D>();
        SetupMeshGenerator();
    }

    public abstract void SetupMeshGenerator();
    public abstract void UpdateMesh();


    public override void _Process(double delta)
    {
        UpdateMesh();
        meshGenerator.chunksToBeUpdated.Clear();
    }

    public void UpdateVoxel(Vector3I position, Guid guid)
    {
        if (GameManager.DataManager.ProjectData.voxelColors.TryGetValue(position, out Guid existingGuid))
        {
            if (existingGuid == guid) { return; }
        }

        GameManager.DataManager.ProjectData.voxelColors[position] = guid;

        Vector3I chunkPosition = meshGenerator.GetChunkPosition(position);

        if (!meshGenerator.chunks.TryGetValue(chunkPosition, out Chunk<TVoxelData> chunk))
        {
            chunk = new Chunk<TVoxelData>();
            meshGenerator.chunks.Add(chunkPosition, chunk);
        }

        chunk.chunkPositions[position] = guid;
        meshGenerator.chunksToBeUpdated.Add(chunk);
    }

    public void ClearVoxel(Vector3I position)
    {
        if (GameManager.DataManager.ProjectData.voxelColors.ContainsKey(position))
        {
            GameManager.DataManager.ProjectData.voxelColors.Remove(position);

            Vector3I chunkPosition = meshGenerator.GetChunkPosition(position);
            if (meshGenerator.chunks.TryGetValue(chunkPosition, out Chunk<TVoxelData> chunk))
            {
                chunk.chunkPositions.Remove(position);
                meshGenerator.chunksToBeUpdated.Add(chunk);
            }
            else
            {
                GD.PrintErr("WorldMesh.cs: void ClearVoxel(): Chunk not found, but should always exist");
            }
        }
    }
}

