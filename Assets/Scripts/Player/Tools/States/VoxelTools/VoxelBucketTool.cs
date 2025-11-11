using System.Collections.Generic;
using Godot;

public class VoxelBucketTool : Tool
{
    public class Options : IToolOptions
    {
        public bool ignoreVoxelColor = false;
        public bool ignorePaletteType = false;
        public bool allowDiagonals = false;
    }

    private readonly Options options = new();
    public override IToolOptions GetToolOptions() => options;

    public override void OnUpdate(double delta)
    {
        if (!ctx.HasHit || !DoesVoxelExist(ctx.VoxelPosition))
        {
            ctx.voxelHiglight.Hide();
            return;
        }

        ctx.voxelHiglight.Show();
        ctx.voxelHiglight.GlobalPosition = ctx.VoxelPosition;

        if (Input.IsActionJustPressed("place"))
        {
            GameManager.CommandManager.ExecuteCommand(new PlaceListCommand(GetVoxelPositions()));
        }
    }

    private bool DoesVoxelExist(Vector3I voxel)
    {
        return GameManager.DataManager.ProjectData.voxelColors.ContainsKey(ctx.VoxelPosition) || GameManager.DataManager.ProjectData.voxelPrefabs.ContainsKey(ctx.VoxelPosition);
    }

    public Vector3I[] GetVoxelPositions()
    {
        List<Vector3I> voxelPositions = new();
        Queue<Vector3I> voxelQueue = new();
        HashSet<Vector3I> processedVoxels = new();

        voxelQueue.Enqueue(ctx.VoxelPosition);

        while (voxelQueue.Count > 0) 
        {
            Vector3I currentVoxel = voxelQueue.Dequeue();
            processedVoxels.Add(currentVoxel);

            if (!DoesVoxelExist(currentVoxel)) { continue; }
            //if (!ignorePalette)
            //{
            //    if (GameManager.DataManager.ProjectData.voxelColors.TryGetValue())
            //}

            voxelPositions.Add(currentVoxel);

            for (int i = 0; i < CubeValues.cubeOffsets.Length; i++)
            {
                Vector3I cubeOffset = CubeValues.cubeOffsets[i];
                voxelQueue.Enqueue(currentVoxel + cubeOffset);
            }

        }

        return voxelPositions.ToArray();
    }
}