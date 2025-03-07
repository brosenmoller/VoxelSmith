using System.Collections.Generic;
using System.Linq;
using Godot;

public class VoxelCoverTool : Tool
{
    private readonly HashSet<Vector3I> voxelPositionSet = new();
    private Vector3I[] voxelPositions;

    public override void OnEnter()
    {
        voxelPositionSet.Clear();
        ctx.meshHighlightMeshInstance.MaterialOverride = ctx.whiteMaterial;

        SelectMenu.OnCutSelectionPressed += ClearVoxelPositions;
        SelectMenu.OnPastePressed += ClearVoxelPositions;
        SelectMenu.OnDeleteSelectionPressed += ClearVoxelPositions;
    }

    public override void OnExit()
    {
        ctx.meshHighlight.Hide();
        SelectMenu.OnCutSelectionPressed -= ClearVoxelPositions;
        SelectMenu.OnPastePressed -= ClearVoxelPositions;
        SelectMenu.OnDeleteSelectionPressed -= ClearVoxelPositions;
    }

    public override void OnUpdate(double delta)
    {
        if (!ctx.HasHit)
        {
            ctx.meshHighlight.Hide();
            return;
        }

        ctx.meshHighlight.Show();
        Vector3I nextVoxel = ctx.GetNextVoxel();
        if (!voxelPositionSet.Contains(nextVoxel))
        {
            CalculateVoxelPositions(nextVoxel);
        }

        if (Input.IsActionJustPressed("place"))
        {
            GameManager.CommandManager.ExecuteCommand(new PlaceListCommand(voxelPositions));
            voxelPositionSet.Clear();
        }
        else if (Input.IsActionJustPressed("break"))
        {
            Vector3I[] insetArray = new Vector3I[voxelPositions.Length];
            Vector3I normal = (Vector3I)ctx.Normal;
            for (int i = 0; i < voxelPositions.Length; i++)
            {
                insetArray[i] = voxelPositions[i] - normal;
            }
            GameManager.CommandManager.ExecuteCommand(new BreakListCommand(insetArray));
            voxelPositionSet.Clear();
        }
    }

    private void CalculateVoxelPositions(Vector3I startPosition)
    {
        voxelPositionSet.Clear();

        HashSet<Vector3I> checkedPositions = new();
        Queue<Vector3I> positionQueue = new();

        positionQueue.Enqueue(startPosition);

        Vector3I normal = (Vector3I)ctx.Normal;
        Vector3I firstPerpendicular = new(normal.Z, normal.X, normal.Y);
        Vector3I secondPerpendicular = new(-normal.Y, -normal.Z, -normal.X);
        
        while (positionQueue.Count > 0) 
        {
            Vector3I currentPosition = positionQueue.Dequeue();

            if (checkedPositions.Contains(currentPosition)) { continue; }
            checkedPositions.Add(currentPosition);

            if (IsVoxelAtPosition(currentPosition)) { continue; }
            if (!IsVoxelAtPosition(currentPosition - normal)) { continue; }

            voxelPositionSet.Add(currentPosition);
            
            positionQueue.Enqueue(currentPosition + firstPerpendicular);
            positionQueue.Enqueue(currentPosition - firstPerpendicular);
            positionQueue.Enqueue(currentPosition + secondPerpendicular);
            positionQueue.Enqueue(currentPosition - secondPerpendicular);
        }

        voxelPositions = voxelPositionSet.ToArray();
        ctx.GenerateVoxelBasedMeshHighlight(voxelPositions);
    }

    private static bool IsVoxelAtPosition(Vector3I position)
    {
        if (GameManager.DataManager.ProjectData.voxelColors.ContainsKey(position)) { return true; }
        if (GameManager.DataManager.ProjectData.voxelPrefabs.ContainsKey(position)) { return true; }
        return false;
    }

    private void ClearVoxelPositions()
    {
        voxelPositionSet.Clear();
    }
}