using Godot;
using System;

public enum VoxelType
{
    Air,
    Color,
    Prefab
}

public struct VoxelMemoryItem
{
    public Vector3I position;
    public VoxelType type;
    public Guid id;

    public VoxelMemoryItem(Vector3I position, VoxelType type, Guid id)
    {
        this.position = position;
        this.type = type;
        this.id = id;
    }

    public static VoxelMemoryItem[] CreateVoxelMemory(Vector3I[] voxelPositions, Func<Vector3I, Vector3I> modifyPosition = null)
    {
        VoxelMemoryItem[] voxelMemory = new VoxelMemoryItem[voxelPositions.Length];

        ProjectData projectData = GameManager.DataManager.ProjectData;

        for (int i = 0; i < voxelPositions.Length; i++)
        {
            Vector3I position = voxelPositions[i];
            VoxelType type = VoxelType.Air;
            Guid id;

            if (projectData.voxelColors.TryGetValue(position, out id))
            {
                type = VoxelType.Color;
            }
            else if (projectData.voxelPrefabs.TryGetValue(position, out id))
            {
                type = VoxelType.Prefab;
            }

            if (modifyPosition != null) { position = modifyPosition(position); }

            voxelMemory[i] = new VoxelMemoryItem(position, type, id);
        }

        return voxelMemory;
    }

    public static void ReplaceFromMemory(VoxelMemoryItem[] voxelMemory, Func<Vector3I, Vector3I> modifyPosition = null)
    {
        for (int i = 0; i < voxelMemory.Length; i++)
        {
            VoxelMemoryItem memoryItem = voxelMemory[i];
            Vector3I position = memoryItem.position;
            if (modifyPosition != null) { position = modifyPosition(position); }

            if (memoryItem.type == VoxelType.Air)
            {
                GameManager.SurfaceMesh.ClearVoxel(position);
                GameManager.PrefabMesh.ClearVoxel(position);
            }
            else if (memoryItem.type == VoxelType.Color)
            {
                GameManager.PrefabMesh.ClearVoxel(position);
                GameManager.SurfaceMesh.UpdateVoxel(position, memoryItem.id);
            }
            else if (memoryItem.type == VoxelType.Prefab)
            {
                GameManager.SurfaceMesh.ClearVoxel(position);
                GameManager.PrefabMesh.UpdateVoxel(position, memoryItem.id);
            }
        }
    }
}
