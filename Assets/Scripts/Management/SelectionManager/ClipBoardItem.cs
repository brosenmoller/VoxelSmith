using Godot;

public class ClipBoardItem
{
    public VoxelMemoryItem[] voxelMemory;

    public ClipBoardItem(VoxelMemoryItem[] voxelMemory)
    {
        this.voxelMemory = voxelMemory;
    }

    private Vector3I RotatePointClockWise(Vector3I point)
    {
        return new Vector3I(-point.Z, point.Y, point.X);
    }

    private Vector3I RotatePointAntiClockwise(Vector3I point)
    {
        return new Vector3I(point.Z, point.Y, -point.X);
    }

    public void RotateClockWise()
    {
        for (int i = 0; i < voxelMemory.Length; i++)
        {
            voxelMemory[i].position = RotatePointClockWise(voxelMemory[i].position);
        }
    }

    public void RotateAntiClockWise()
    {
        for (int i = 0; i < voxelMemory.Length; i++)
        {
            voxelMemory[i].position = RotatePointAntiClockwise(voxelMemory[i].position);
        }
    }

    public void Flip()
    {
        GD.Print("FLip");
        Vector3 axis = GameManager.Player.pivot.GlobalBasis.Z;
        axis = axis.Normalized();
        axis = SnapToCardinalDirection(axis);

        for (int i = 0; i < voxelMemory.Length; i++)
        {
            VoxelMemoryItem item = voxelMemory[i];

            if (axis.X != 0)
            {
                item.position.X = -item.position.X;
            }
            if (axis.Y != 0)
            {
                item.position.Y = -item.position.Y;
            }
            if (axis.Z != 0)
            {
                item.position.Z = -item.position.Z;
            }

            voxelMemory[i] = item;
        }
    }

    private Vector3 SnapToCardinalDirection(Vector3 direction)
    {
        Vector3[] cardinalDirections = {
            Vector3.Right,
            Vector3.Left,
            Vector3.Up,
            Vector3.Down,
            Vector3.Forward,
            Vector3.Back
        };

        Vector3 closestDirection = cardinalDirections[0];
        float maxDot = direction.Dot(closestDirection);

        foreach (var cardinal in cardinalDirections)
        {
            float dot = direction.Dot(cardinal);
            if (dot > maxDot)
            {
                maxDot = dot;
                closestDirection = cardinal;
            }
        }

        return closestDirection;
    }
}
