using Godot;

public class BrushTool : State<ToolUser>
{
    private readonly VoxelSmith.Timer placeTimer;
    private readonly VoxelSmith.Timer breakTimer;

    public BrushTool() 
    {
        placeTimer = new VoxelSmith.Timer(0.2f, PlaceBlock, autoStart: false, loop: true);
        breakTimer = new VoxelSmith.Timer(0.2f, BreakBlock, autoStart: false, loop: true);
    }

    public override void OnUpdate(double delta)
    {
        if (!ctx.HasHit) { return; }

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

        if (Input.IsActionJustReleased("place"))
        {
            placeTimer.Pause();
        }
        else if (Input.IsActionJustReleased("break"))
        {
            breakTimer.Pause();
        }
    }

    private void BreakBlock()
    {
        GameManager.CommandManager.ExecuteCommand(new BreakVoxelCommand(ctx.VoxelPosition));
    }

    private void PlaceBlock()
    {
        Vector3I nextVoxel = ctx.VoxelPosition + (Vector3I)ctx.Normal.Normalized();

        if ((!ctx.IsVoxelInPlayer(nextVoxel) || !ctx.checkForPlayerInside) && GameManager.DataManager.ProjectData.SelectedVoxelData != null)
        {
            GameManager.CommandManager.ExecuteCommand(new PlaceVoxelCommand(nextVoxel));
        }
    }
}
