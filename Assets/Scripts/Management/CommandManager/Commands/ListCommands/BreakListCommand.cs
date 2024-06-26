﻿using Godot;

public class BreakListCommand : VoxelListCommand, ICommand
{
    public BreakListCommand(params Vector3I[] voxelPositions) : base(voxelPositions) { }

    public void Execute()
    {
        Break();
    }

    public void Undo()
    {
        ReplaceFromMemory();
    }

    public void Break()
    {
        foreach (Vector3I voxelPosition in voxelPositions)
        {
            //projectData.voxelColors.Remove(voxelPosition);
            GameManager.SurfaceMesh.ClearVoxel(voxelPosition);

            //projectData.voxelPrefabs.Remove(voxelPosition);
            GameManager.PrefabMesh.ClearVoxel(voxelPosition);
        }

        //if (containsColors)
        //{
        //    GameManager.SurfaceMesh.UpdateMesh();
        //}

        //if (containsPrefabs)
        //{
        //    GameManager.PrefabMesh.UpdateMesh();
        //}
    }
}

