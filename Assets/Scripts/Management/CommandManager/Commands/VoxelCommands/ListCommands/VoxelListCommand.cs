using Godot;
using System;

public class VoxelListCommand
{
    protected Vector3I[] voxelPositions;
    private VoxelMemory[] voxelMemory;

    protected ProjectData projectData;

    public VoxelListCommand(params Vector3I[] voxelPositions)
    {
        this.voxelPositions = voxelPositions;
        projectData = GameManager.DataManager.ProjectData;
        CreateVoxelMemory();
    }

    private void CreateVoxelMemory()
    {
        voxelMemory = new VoxelMemory[voxelPositions.Length];

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

            voxelMemory[i] = new VoxelMemory(position, type, id);
        }
    }

    public void ReplaceFromMemory()
    {
        for (int i = 0; i < voxelMemory.Length; i++)
        {
            VoxelMemory memory = voxelMemory[i];

            if (memory.type == VoxelType.Air)
            {
                GameManager.SurfaceMesh.ClearVoxel(memory.position);
                GameManager.PrefabMesh.ClearVoxel(memory.position);
            }
            else if (memory.type == VoxelType.Color)
            {
                GameManager.PrefabMesh.ClearVoxel(memory.position);
                GameManager.SurfaceMesh.UpdateVoxel(memory.position, memory.id);
            }
            else if (memory.type == VoxelType.Prefab)
            {
                GameManager.SurfaceMesh.ClearVoxel(memory.position);
                GameManager.PrefabMesh.UpdateVoxel(memory.position, memory.id);
            }
        }
    }

    public struct VoxelMemory 
    {
        public Vector3I position;
        public VoxelType type;
        public Guid id;

        public VoxelMemory(Vector3I position, VoxelType type, Guid id)
        {
            this.position = position;
            this.type = type;
            this.id = id;
        }
    }

    public enum VoxelType
    {
        Air,
        Color,
        Prefab
    }
}
