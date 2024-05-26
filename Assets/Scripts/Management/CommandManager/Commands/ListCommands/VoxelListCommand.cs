using Godot;
using System;

public class VoxelListCommand
{
    protected Vector3I[] voxelPositions;
    private VoxelMemory[] voxelMemory;

    protected bool containsColors = false;
    protected bool containsPrefabs = false;

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
                containsColors = true;
                type = VoxelType.Color;
            }
            else if (projectData.voxelPrefabs.TryGetValue(position, out id))
            {
                containsPrefabs = true;
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
                projectData.voxelColors.Remove(memory.position);
                projectData.voxelPrefabs.Remove(memory.position);
            }
            else if (memory.type == VoxelType.Color)
            {
                projectData.voxelPrefabs.Remove(memory.position);
                projectData.voxelColors[memory.position] = memory.id;
            }
            else if (memory.type == VoxelType.Prefab)
            {
                projectData.voxelColors.Remove(memory.position);
                projectData.voxelPrefabs[memory.position] = memory.id;
            }
        }

        GameManager.SurfaceMesh.UpdateMesh();
        GameManager.PrefabMesh.UpdateMesh();
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
