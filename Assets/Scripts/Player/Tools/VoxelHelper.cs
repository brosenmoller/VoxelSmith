using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public static class VoxelHelper
{
    public static Vector3I GetGridVoxelFromHitPoint(Vector3 hitPoint, Vector3 normal)
    {
        Vector3 insetPoint = hitPoint - (normal * 0.1f);
        return GetGridVoxel(insetPoint);
    }

    public static Vector3I GetGridVoxel(Vector3 point)
    {
        return new(
            Mathf.FloorToInt(point.X),
            Mathf.FloorToInt(point.Y),
            Mathf.FloorToInt(point.Z)
        );
    }

    public static Vector3I[] GetCubeVoxels(Vector3I firstPosition, Vector3I secondPosition)
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

    [Flags]
    public enum CubeFaces
    {
        None = 0,
        Left = 1 << 2,
        Right = 1 << 3,
        X = Left | Right,
        Bottom = 1 << 4,
        Top = 1 << 5,
        Y = Bottom | Top,
        Front = 1 << 0,
        Back = 1 << 1,
        Z = Front | Back,
        XZ = X | Z,
        XY = X | Y,
        YZ = Y | Z,
        All = Front | Back | Left | Right | Bottom | Top
    }

    public static Vector3I[] GetHollowCubeVoxels(Vector3I firstPosition, Vector3I secondPosition, CubeFaces hollowFaces)
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
            return Array.Empty<Vector3I>();
        }

        List<Vector3I> voxelPositions = new();

        for (int x = minX; x <= maxX; x++)
        {
            for (int y = minY; y <= maxY; y++)
            {
                for (int z = minZ; z <= maxZ; z++)
                {
                    if (((hollowFaces & CubeFaces.Front) != 0 && x == minX) ||
                        ((hollowFaces & CubeFaces.Back) != 0 && x == maxX) ||
                        ((hollowFaces & CubeFaces.Left) != 0 && y == minY) ||
                        ((hollowFaces & CubeFaces.Right) != 0 && y == maxY) ||
                        ((hollowFaces & CubeFaces.Bottom) != 0 && z == minZ) ||
                        ((hollowFaces & CubeFaces.Top) != 0 && z == maxZ))
                    {
                        voxelPositions.Add(new Vector3I(x, y, z));
                    }
                }
            }
        }

        return voxelPositions.ToArray();
    }

    public static Vector3I[] GetLineVoxels(Vector3I firstPosition, Vector3I secondPosition, float stepLength)
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
            Vector3I gridPosition = GetGridVoxel(currentPoint);
            voxelPostions.Add(gridPosition);

            currentPoint += step;
        }

        return voxelPostions.ToArray();
    }
}
