using Godot;
using System.Collections.Generic;
using System.Linq;

public abstract class TwoPointsTool : Tool
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

    public override void OnExit()
    {
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