﻿using Godot;

public abstract class TwoPointsTool : State<ToolUser>
{
    protected const float emptyDistance = 3;

    protected Vector3I firstPosition;
    protected Vector3I secondPosition;

    private bool placeSequence;
    private bool breakSequence;

    public override void OnEnter()
    {
        ctx.cornerHighlight1.Show();
        placeSequence = false;
        breakSequence = false;
    }

    public override void OnUpdate(double delta)
    {
        if (!placeSequence && !breakSequence)
        {
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
                GameManager.CommandManager.ExecuteCommand(new PlaceListCommand(voxelPositions));
            }
            else if (Input.IsActionJustReleased("break") && breakSequence)
            {
                breakSequence = false;
                GameManager.CommandManager.ExecuteCommand(new BreakListCommand(voxelPositions));
            }

            if (!placeSequence && !breakSequence)
            {
                ctx.cornerHighlight2.Hide();
                ctx.meshHighlight.Hide();
            }
        }
    }

    public override void OnExit()
    {
        ctx.cornerHighlight1.Hide();
        ctx.cornerHighlight2.Hide();
        ctx.meshHighlight.Hide();
    }

    protected abstract Vector3I[] GetVoxelPositions();

    protected virtual void GenerateMeshHighlight(Vector3I[] voxelPositions)
    {
        ctx.GenerateVoxelBasedMeshHighlight(voxelPositions);
    }
}

