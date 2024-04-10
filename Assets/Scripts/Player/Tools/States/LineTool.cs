using Godot;
using System.Collections.Generic;
using System.Linq;

public class LineTool : TwoPointsTool
{
    private const float stepLength = 0.2f;

    protected override Vector3I[] GetVoxelPositions()
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
