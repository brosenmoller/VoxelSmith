using System;
using System.Collections.Generic;
using Godot;

public class VoxelBucketTool : Tool
{
    public class Options : IToolOptions
    {
        public bool ignoreVoxelType = false;
        public bool allowDiagonals = false;
    }

    private readonly Options options = new();
    public override IToolOptions GetToolOptions() => options;

    public override void OnUpdate(double delta)
    {
        if (!ctx.HasHit || !TryGetPaletteGuidForVoxel(ctx.VoxelPosition, out Guid paletteGuid))
        {
            ctx.voxelHiglight.Hide();
            return;
        }

        ctx.voxelHiglight.Show();
        ctx.voxelHiglight.GlobalPosition = ctx.VoxelPosition;

        if (Input.IsActionJustPressed("place"))
        {
            GameManager.CommandManager.ExecuteCommand(new PlaceListCommand(GetVoxelPositions(paletteGuid)));
        }
    }

    public Vector3I[] GetVoxelPositions(Guid originalPaletteGuid)
    {
        List<Vector3I> voxelPositions = new();
        Queue<Vector3I> voxelQueue = new();
        HashSet<Vector3I> processedVoxels = new();

        voxelQueue.Enqueue(ctx.VoxelPosition);

        while (voxelQueue.Count > 0) 
        {
            Vector3I currentVoxel = voxelQueue.Dequeue();

            if (processedVoxels.Contains(currentVoxel)) { continue; }
            processedVoxels.Add(currentVoxel);

            if (!TryGetPaletteGuidForVoxel(currentVoxel, out Guid currentPaletteGuid)) { continue; }
            if (!options.ignoreVoxelType && currentPaletteGuid != originalPaletteGuid) { continue; }

            voxelPositions.Add(currentVoxel);

            for (int i = 0; i < CubeValues.cubeOffsets.Length; i++)
            {
                Vector3I cubeOffset = CubeValues.cubeOffsets[i];
                Vector3I newVoxel = currentVoxel + cubeOffset;
                if (processedVoxels.Contains(newVoxel)) { continue; }

                voxelQueue.Enqueue(newVoxel);
            }

            if (!options.allowDiagonals) { continue; }

            for (int i = 0; i < CubeValues.cubeDiagonals.Length; i++)
            {
                Vector3I cubeDiagonal = CubeValues.cubeDiagonals[i];
                Vector3I newVoxel = currentVoxel + cubeDiagonal;
                if (processedVoxels.Contains(newVoxel)) { continue; }

                voxelQueue.Enqueue(newVoxel);
            }
        }

        return voxelPositions.ToArray();
    }

    private static bool TryGetPaletteGuidForVoxel(Vector3I voxel, out Guid paletteGuid)
    {
        if (GameManager.DataManager.ProjectData.voxelColors.TryGetValue(voxel, out paletteGuid)) { return true; }
        if (GameManager.DataManager.ProjectData.voxelPrefabs.TryGetValue(voxel, out paletteGuid)) { return true; }

        return false;
    }
}