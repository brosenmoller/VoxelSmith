using Godot;

public class ClipBoardItem
{
    public VoxelMemoryItem[] voxelMemory;

    public ClipBoardItem(VoxelMemoryItem[] voxelMemory)
    {
        this.voxelMemory = voxelMemory;
    }

    public void Rotate(float degrees)
    {
        float radians = Mathf.DegToRad(degrees);

        Basis basis = Basis.Identity.Rotated(Vector3.Down, radians);

        for (int i = 0; i < voxelMemory.Length; i++)
        {
            VoxelMemoryItem item = voxelMemory[i];
            item.position = (Vector3I)(basis * (Vector3)item.position);
        }
    }

    public void Flip()
    {
        Vector3 axis = GameManager.Player.GlobalBasis.Z;
        axis = axis.Normalized();
        axis = SnapToCardinalDirection(axis);

        for (int i = 0; i < voxelMemory.Length; i++)
        {
            VoxelMemoryItem item = voxelMemory[i];
            Vector3I pos = item.position;

            if (axis.X != 0)
            {
                pos.X = -pos.X;
            }
            if (axis.Y != 0)
            {
                pos.Y = -pos.Y;
            }
            if (axis.Z != 0)
            {
                pos.Z = -pos.Z;
            }

            item.position = pos;
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
