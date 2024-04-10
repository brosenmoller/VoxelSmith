using Godot;

public class CubeTool : State<ToolUser> 
{
    private const float checkLength = 7;
    private const float emptyDistance = 3;

    private Vector3I firstPosition;
    private Vector3I secondPosition;

    private bool placeSequence = false;

    public override void OnEnter()
    {
        ctx.cornerHighlight1.Show();
    }

    public override void OnUpdate(double delta)
    {
        if (!placeSequence)
        {
            firstPosition = GetPosition();
            ctx.cornerHighlight1.GlobalPosition = firstPosition;
        }

        if (Input.IsActionJustPressed("place"))
        {
            placeSequence = true;
            ctx.cornerHighlight2.Show();
        }

        if (placeSequence)
        {
            secondPosition = GetPosition();
            ctx.cornerHighlight2.GlobalPosition = secondPosition;
        }

        if (Input.IsActionJustReleased("place"))
        {
            placeSequence = false;
            ctx.cornerHighlight2.Hide();

            Vector3I[] voxelPositions = GetAllPointsInCube();
            GameManager.CommandManager.ExecuteCommand(new PlaceListCommand(voxelPositions));
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

    private Vector3I GetPosition()
    {
        Vector3 normalizedGlobalDirection = (-1 * ctx.GlobalTransform.Basis.Z).Normalized();

        Vector3 checkEndPoint = normalizedGlobalDirection * checkLength + ctx.GlobalPosition;

        if (ctx.RayCast3D(ctx.GlobalPosition, checkEndPoint, out var hitInfo, ctx.CollisionMask, false, true))
        {
            Vector3I voxelPostion = ctx.GetGridPositionFromHitPoint(hitInfo.position, hitInfo.normal);
            Vector3I nextVoxel = voxelPostion + (Vector3I)hitInfo.normal.Normalized();
            return nextVoxel;
        }

        Vector3 emptySpacePoint = normalizedGlobalDirection * emptyDistance + ctx.GlobalPosition;
        return ctx.GetGridPositionFromHitPoint(emptySpacePoint, normalizedGlobalDirection);
    }
}

