using Godot;

public abstract class BrushTool : State<ToolUser>
{
    private const float TIMER_LENGTH = 0.2f;

    protected readonly VoxelSmith.Timer placeTimer;
    protected readonly VoxelSmith.Timer breakTimer;

    public BrushTool()
    {
        placeTimer = new VoxelSmith.Timer(TIMER_LENGTH, PlaceAction, autoStart: false, loop: true);
        breakTimer = new VoxelSmith.Timer(TIMER_LENGTH, BreakAction, autoStart: false, loop: true);
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
            PlaceAction();
        }
        else if (Input.IsActionJustPressed("break"))
        {
            breakTimer.Restart();
            BreakAction();
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

    protected Vector3I GetNextVoxel()
    {
        return ctx.VoxelPosition + (Vector3I)ctx.Normal.Normalized();
    }

    protected abstract void BreakAction();
    protected abstract void PlaceAction();
}
