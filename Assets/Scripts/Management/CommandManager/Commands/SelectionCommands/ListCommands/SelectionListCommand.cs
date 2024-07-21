using Godot;
using System.Collections.Generic;

public class SelectionListCommand
{
    protected Vector3I[] voxelPositions;
    private HashSet<Vector3I> voxelMemory;

    protected ProjectData projectData;

    public SelectionListCommand(params Vector3I[] voxelPositions)
    {
        this.voxelPositions = voxelPositions;
        projectData = GameManager.DataManager.ProjectData;
        CreateVoxelMemory();
    }

    private void CreateVoxelMemory()
    {
        voxelMemory = new HashSet<Vector3I>();

        for (int i = 0; i < voxelPositions.Length; i++)
        {
            Vector3I position = voxelPositions[i];
            if (GameManager.SelectionManager.CurrentSelection.Contains(position))
            {
                voxelMemory.Add(position);
            }
        }
    }

    private void UpdateIfContains(Vector3I voxelPosition)
    {
        if (GameManager.DataManager.ProjectData.voxelColors.ContainsKey(voxelPosition))
        {
            GameManager.SurfaceMesh.UpdateChunkContainingPosition(voxelPosition);
        }
        else
        {
            GameManager.PrefabMesh.UpdateChunkContainingPosition(voxelPosition);
        }
    }

    public void AddToSelection(Vector3I voxelPosition)
    {
        if (!GameManager.SelectionManager.CurrentSelection.Contains(voxelPosition) && 
            (GameManager.DataManager.ProjectData.voxelColors.ContainsKey(voxelPosition) || GameManager.DataManager.ProjectData.voxelPrefabs.ContainsKey(voxelPosition)))
        {
            GameManager.SelectionManager.CurrentSelection.Add(voxelPosition);
            UpdateIfContains(voxelPosition);
        }
    }

    public void RemoveFromSelection(Vector3I voxelPosition)
    {
        if (GameManager.SelectionManager.CurrentSelection.Contains(voxelPosition))
        {
            GameManager.SelectionManager.CurrentSelection.Remove(voxelPosition);
            UpdateIfContains(voxelPosition);
        }
    }

    public void ReplaceFromMemory()
    {
        for (int i = 0; i < voxelPositions.Length; i++)
        {
            Vector3I position = voxelPositions[i];
            if (voxelMemory.Contains(position))
            {
                GameManager.SelectionManager.CurrentSelection.Add(position);
            }
            else
            {
                GameManager.SelectionManager.CurrentSelection.Remove(position);
            }

            GameManager.SurfaceMesh.UpdateChunkContainingPosition(position);
            GameManager.PrefabMesh.UpdateChunkContainingPosition(position);
        }
    }
}

