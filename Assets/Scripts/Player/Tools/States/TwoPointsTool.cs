using Godot;
using System.Collections.Generic;
using System.Linq;

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

    public Vector3I[] GetCubeVoxels()
    {
        int minX = Mathf.Min(firstPosition.X, secondPosition.X);
        int maxX = Mathf.Max(firstPosition.X, secondPosition.X);
        int minY = Mathf.Min(firstPosition.Y, secondPosition.Y);
        int maxY = Mathf.Max(firstPosition.Y, secondPosition.Y);
        int minZ = Mathf.Min(firstPosition.Z, secondPosition.Z);
        int maxZ = Mathf.Max(firstPosition.Z, secondPosition.Z);

        int sizeX = maxX - minX + 1;
        int sizeY = maxY - minY + 1;
        int sizeZ = maxZ - minZ + 1;

        if (sizeX <= 0 || sizeY <= 0 || sizeZ <= 0)
        {
            return System.Array.Empty<Vector3I>();
        }

        Vector3I[] voxelPositions = new Vector3I[sizeX * sizeY * sizeZ];

        int i = 0;

        for (int x = minX; x <= maxX; x++)
        {
            for (int y = minY; y <= maxY; y++)
            {
                for (int z = minZ; z <= maxZ; z++)
                {
                    voxelPositions[i++] = new Vector3I(x, y, z);
                }
            }
        }

        return voxelPositions;
    }

    public Vector3I[] GetLineVoxels(float stepLength)
    {
        Vector3 line = secondPosition - firstPosition;
        float squareMagnitude = line.LengthSquared();

        Vector3 direction = line.Normalized();
        Vector3 step = direction * stepLength;

        HashSet<Vector3I> voxelPostions = new()
        {
            firstPosition,
            secondPosition
        };

        Vector3 currentPoint = firstPosition;
        while ((currentPoint - firstPosition).LengthSquared() < squareMagnitude)
        {
            Vector3I gridPosition = ctx.GetGridPosition(currentPoint);
            voxelPostions.Add(gridPosition);

            currentPoint += step;
        }

        return voxelPostions.ToArray();
    }
}