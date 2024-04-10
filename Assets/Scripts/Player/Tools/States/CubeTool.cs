using Godot;

public class CubeTool : State<ToolUser> 
{
    private const float checkLength = 7;
    private const float emptyDistance = 3;

    private Vector3I firstPosition;
    private Vector3I secondPosition;

    private bool placeSequence;
    private bool breakSequence;

    public override void OnEnter()
    {
        ctx.cornerHighlight1.Show();
        placeSequence = false;
    }

    public override void OnUpdate(double delta)
    {
        if (!placeSequence && !breakSequence)
        {
            firstPosition = ctx.GetPosition(checkLength, emptyDistance);
            ctx.cornerHighlight1.GlobalPosition = firstPosition;

            if (Input.IsActionJustPressed("place"))
            {
                placeSequence = true;
            }
            else if (Input.IsActionJustPressed("break"))
            {
                breakSequence = true;
            }

            if (placeSequence || breakSequence)
            {
                ctx.cornerHighlight2.Show();
                ctx.meshHighlight.Show();
            }
        }
        else if (placeSequence || breakSequence)
        {
            secondPosition = ctx.GetPosition(checkLength, emptyDistance);

            Vector3I[] voxelPositions = GetAllPointsInCube();
            ctx.GenerateMeshHighlight(voxelPositions);
            ctx.cornerHighlight2.GlobalPosition = secondPosition;

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
    }

    private Vector3I[] GetAllPointsInCube()
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
}

