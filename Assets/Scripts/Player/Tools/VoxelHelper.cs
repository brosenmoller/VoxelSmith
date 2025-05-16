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
        CubeInfo cubeInfo = new(firstPosition, secondPosition);
        if (cubeInfo.Invalid) { return Array.Empty<Vector3I>(); }

        Vector3I[] voxelPositions = new Vector3I[cubeInfo.volume];

        int i = 0;
        for (int x = cubeInfo.minX; x <= cubeInfo.maxX; x++)
        {
            for (int y = cubeInfo.minY; y <= cubeInfo.maxY; y++)
            {
                for (int z = cubeInfo.minZ; z <= cubeInfo.maxZ; z++)
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

    public static Vector3I[] GetHollowCubeVoxels(Vector3I firstPosition, Vector3I secondPosition, CubeFaces SolidFaces = CubeFaces.All)
    {
        CubeInfo cubeInfo = new(firstPosition, secondPosition);
        if (cubeInfo.Invalid) { return Array.Empty<Vector3I>(); }

        List<Vector3I> voxelPositions = new();

        for (int x = cubeInfo.minX; x <= cubeInfo.maxX; x++)
        {
            for (int y = cubeInfo.minY; y <= cubeInfo.maxY; y++)
            {
                for (int z = cubeInfo.minZ; z <= cubeInfo.maxZ;  z++)
                {
                    if (((SolidFaces & CubeFaces.Left) != 0 && x == cubeInfo.minX) ||
                        ((SolidFaces & CubeFaces.Right) != 0 && x == cubeInfo.maxX) ||
                        ((SolidFaces & CubeFaces.Top) != 0 && y == cubeInfo.minY) ||
                        ((SolidFaces & CubeFaces.Bottom) != 0 && y == cubeInfo.maxY) ||
                        ((SolidFaces & CubeFaces.Front) != 0 && z == cubeInfo.minZ) ||
                        ((SolidFaces & CubeFaces.Back) != 0 && z == cubeInfo.maxZ))
                    {
                        voxelPositions.Add(new Vector3I(x, y, z));
                    }
                }
            }
        }

        return voxelPositions.ToArray();
    }

    public static Vector3I[] GetSphereVoxels(Vector3I origin, Vector3I secondPosition, bool isHollow)
    {
        Vector3 originFloat = origin;
        float radius = originFloat.DistanceTo(secondPosition);
        int intRadius = Mathf.RoundToInt(radius);

        return GetShphereVoxels(origin, intRadius, isHollow);
    }

    public static Vector3I[] GetShphereVoxels(Vector3I origin, int radius, bool isHollow)
    {
        Vector3I cornerVector = new(radius, radius, radius);
        Vector3I corner1 = origin + cornerVector;
        Vector3I corner2 = origin - cornerVector;

        CubeInfo cubeInfo = new(corner1, corner2);
        List<Vector3I> voxelPositions = new();

        for (int x = cubeInfo.minX; x <= cubeInfo.maxX; x++)
        {
            for (int y = cubeInfo.minY; y <= cubeInfo.maxY; y++)
            {
                for (int z = cubeInfo.minZ; z <= cubeInfo.maxZ; z++)
                {
                    Vector3I currentVoxel = new(x, y, z);
                    float distance = ((Vector3)currentVoxel).DistanceTo(origin);
                    
                    if (isHollow && (distance > radius + 0.49f || distance < radius - 0.49f)) { continue; }
                    else if (!isHollow && distance > radius) { continue; }

                    voxelPositions.Add(currentVoxel);
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

    public class CubeInfo
    {
        public readonly int minX, maxX, minY, maxY, minZ, maxZ;
        public readonly int sizeX, sizeY, sizeZ;
        public readonly int volume;
        public bool Invalid => sizeX <= 0 || sizeY <= 0 || sizeZ <= 0;

        public CubeInfo(Vector3I firstPosition, Vector3I secondPosition)
        {
            minX = Mathf.Min(firstPosition.X, secondPosition.X);
            maxX = Mathf.Max(firstPosition.X, secondPosition.X);
            minY = Mathf.Min(firstPosition.Y, secondPosition.Y);
            maxY = Mathf.Max(firstPosition.Y, secondPosition.Y);
            minZ = Mathf.Min(firstPosition.Z, secondPosition.Z);
            maxZ = Mathf.Max(firstPosition.Z, secondPosition.Z);

            sizeX = maxX - minX + 1;
            sizeY = maxY - minY + 1;
            sizeZ = maxZ - minZ + 1;

            volume = sizeX * sizeY * sizeZ;
        }
    }
}