using Godot;

public class BrushTool : State<ToolUser>
{
    protected readonly VoxelSmith.Timer placeTimer;
    protected readonly VoxelSmith.Timer breakTimer;

    public BrushTool() 
    {
        placeTimer = new VoxelSmith.Timer(0.2f, PlaceBlock, autoStart: false, loop: true);
        breakTimer = new VoxelSmith.Timer(0.2f, BreakBlock, autoStart: false, loop: true);
    }

    public override void OnUpdate(double delta)
    {
        if (!ctx.HasHit) 
        {
            ctx.voxelHiglight.Hide();
            return; 
        }

        ctx.voxelHiglight.Show();
        ctx.voxelHiglight.GlobalPosition = ctx.VoxelPosition;

        if (Input.IsActionJustPressed("place"))
        {
            placeTimer.Restart();
            PlaceBlock();
        }
        else if(Input.IsActionJustPressed("break"))
        {
            breakTimer.Restart();
            BreakBlock();
        }

        if (!Input.IsActionPressed("place"))
        {
            placeTimer.Pause();
        }

        if (!Input.IsActionPressed("break"))
        {
            breakTimer.Pause();
        }
    }
    public override void OnExit()
    {
        placeTimer.Pause();
        breakTimer.Pause();
        ctx.voxelHiglight.Hide();
    }

    private void BreakBlock()
    {
        GameManager.CommandManager.ExecuteCommand(new BreakVoxelCommand(ctx.VoxelPosition));
    }

    private void PlaceBlock()
    {
        Vector3I nextVoxel = ctx.VoxelPosition + (Vector3I)ctx.Normal.Normalized();

        if ((!ctx.IsVoxelInPlayer(nextVoxel) || ctx.ignorePlayerCheck) && GameManager.DataManager.ProjectData.SelectedVoxelData != null)
        {
            GameManager.CommandManager.ExecuteCommand(new PlaceVoxelCommand(nextVoxel));
        }
    }
}
