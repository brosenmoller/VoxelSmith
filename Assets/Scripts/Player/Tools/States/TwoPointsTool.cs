using Godot;

public abstract class TwoPointsTool : Tool
{
    protected const float emptyDistance = 3;

    protected Vector3I firstPosition;
    protected Vector3I secondPosition;

    private bool placeSequence;
    private bool breakSequence;

    private bool lockX;
    private bool lockY;
    private bool lockZ;

    public override void OnEnter()
    {
        ctx.cornerHighlight1.Show();
        placeSequence = false;
        breakSequence = false;

        GameManager.TopBarUI.ToggleShowToolSizeText(true);
    }

    public override void OnUpdate(double delta)
    {
        if (!placeSequence && !breakSequence)
        {
            GameManager.TopBarUI.SetToolSizeText(Vector3.Zero);
            lockX = lockY = lockZ = false;

            firstPosition = ctx.GetVoxelPositionFromLook(Mathf.Abs(ctx.TargetPosition.Z), emptyDistance, ctx.selectInsideEnabled);
            ctx.cornerHighlight1.GlobalPosition = firstPosition;

            if (Input.IsActionJustPressed("place"))
            {
                placeSequence = true;
                ctx.meshHighlightMeshInstance.MaterialOverride = ctx.whiteMaterial;
            }
            else if (Input.IsActionJustPressed("break"))
            {
                breakSequence = true;
                ctx.meshHighlightMeshInstance.MaterialOverride = ctx.redMaterial;
            }

            if (placeSequence || breakSequence)
            {
                ctx.cornerHighlight2.Show();
                ctx.meshHighlight.Show();
            }
        }
        else if (placeSequence || breakSequence)
        {
            secondPosition = ctx.GetVoxelPositionFromLook(Mathf.Abs(ctx.TargetPosition.Z), emptyDistance, ctx.selectInsideEnabled);

            AxisLocking();

            Vector3 size = new(
                Mathf.Abs(firstPosition.X - secondPosition.X),
                Mathf.Abs(firstPosition.Y - secondPosition.Y),
                Mathf.Abs(firstPosition.Z - secondPosition.Z)
            );
            GameManager.TopBarUI.SetToolSizeText(size);

            Vector3I[] voxelPositions = GetVoxelPositions();
            GenerateMeshHighlight(voxelPositions);
            ctx.cornerHighlight2.GlobalPosition = secondPosition;

            if (Input.IsActionJustPressed("place") && breakSequence)
            {
                breakSequence = false;
            }
            else if (Input.IsActionJustPressed("break") && placeSequence)
            {
                placeSequence = false;
            }

            if (Input.IsActionJustReleased("place") && placeSequence)
            {
                placeSequence = false;
                PlaceAction(voxelPositions);
            }
            else if (Input.IsActionJustReleased("break") && breakSequence)
            {
                breakSequence = false;
                BreakAction(voxelPositions);
            }

            if (!placeSequence && !breakSequence)
            {
                ctx.cornerHighlight2.Hide();
                ctx.meshHighlight.Hide();
            }
        }
    }

    private void AxisLocking()
    {
        if (Input.IsActionJustPressed("lock_x") && !Input.IsKeyPressed(Key.Ctrl))
        {
            lockY = false;

            if (lockX || lockZ)
            {
                lockX = lockZ = false;
                return;
            }

            float xDiff = Mathf.Abs(firstPosition.X - secondPosition.X);
            float zDiff = Mathf.Abs(firstPosition.Z - secondPosition.Z);

            if (xDiff < zDiff)
            {
                lockX = true;
            }
            else
            {
                lockZ = true;
            }
        }

        if (Input.IsActionJustPressed("lock_y") && !Input.IsKeyPressed(Key.Ctrl))
        {
            lockX = lockZ = false;

            lockY = !lockY;
        }

        Vector3 planeNormal;
        if (lockX)
        {
            planeNormal = new Vector3(Mathf.Clamp(ctx.GlobalPosition.X - firstPosition.X , - 1, 1), 0, 0);
        }
        else if (lockY)
        {
            planeNormal = new Vector3(0, Mathf.Clamp(ctx.GlobalPosition.Y - firstPosition.Y, - 1, 1), 0);
        }
        else if (lockZ)
        {
            planeNormal = new Vector3(0, 0, Mathf.Clamp(ctx.GlobalPosition.Z - firstPosition.Z, - 1, 1));
        }
        else
        { 
            return; 
        }

        if (ctx.GetVoxelPositionFromPlane(planeNormal, firstPosition, out Vector3I voxelPosition))
        {
            const int MAX_DISTANCE = 500;
            secondPosition = voxelPosition;
            secondPosition.X = Mathf.Clamp(secondPosition.X, firstPosition.X - MAX_DISTANCE, firstPosition.X + MAX_DISTANCE);
            secondPosition.Y = Mathf.Clamp(secondPosition.Y, firstPosition.Y - MAX_DISTANCE, firstPosition.Y + MAX_DISTANCE);
            secondPosition.Z = Mathf.Clamp(secondPosition.Z, firstPosition.Z - MAX_DISTANCE, firstPosition.Z + MAX_DISTANCE);
            return;
        }

        if (lockX) { secondPosition.X = firstPosition.X; }
        if (lockY) { secondPosition.Y = firstPosition.Y; }
        if (lockZ) { secondPosition.Z = firstPosition.Z; }
    }

    public override void OnExit()
    {
        GameManager.TopBarUI.ToggleShowToolSizeText(false);

        ctx.cornerHighlight1.Hide();
        ctx.cornerHighlight2.Hide();
        ctx.meshHighlight.Hide();
    }

    protected abstract Vector3I[] GetVoxelPositions();
    protected abstract void PlaceAction(Vector3I[] voxels);
    protected abstract void BreakAction(Vector3I[] voxels);

    protected virtual void GenerateMeshHighlight(Vector3I[] voxelPositions)
    {
        ctx.GenerateVoxelBasedMeshHighlight(voxelPositions);
    }
}