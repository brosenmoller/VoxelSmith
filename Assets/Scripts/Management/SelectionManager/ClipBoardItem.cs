using Godot;
using System.Collections.Generic;
using System;
using System.Linq;

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

    public void ChangeGUIDsToNewProject(PaletteData oldPaletteData)
    {
        Dictionary<Guid, Guid> knownConversions = new();

        for (int i = 0; i < voxelMemory.Length; i++)
        {
            VoxelMemoryItem item = voxelMemory[i];

            if (item.type == VoxelType.Air) { continue; }

            if (knownConversions.TryGetValue(item.id, out Guid newId))
            {
                item.id = newId;
            }
            else
            {
                if (item.type == VoxelType.Color)
                {
                    item.id = GetMatchingId(item.id, oldPaletteData.paletteColors, GameManager.DataManager.PaletteData.paletteColors, knownConversions);
                }
                else if (item.type == VoxelType.Prefab)
                {
                    item.id = GetMatchingId(item.id, oldPaletteData.palletePrefabs, GameManager.DataManager.PaletteData.palletePrefabs, knownConversions);
                }
            }

            voxelMemory[i] = item;
        }
    }

    private Guid GetMatchingId<TVoxelData>(Guid oldId, Dictionary<Guid, TVoxelData> oldData, Dictionary<Guid, TVoxelData> newData, Dictionary<Guid, Guid> knownConversions) where TVoxelData : VoxelData
    {
        if (newData.ContainsKey(oldId))
        {
            return oldId;
        }

        foreach (var newItem in newData)
        {
            if (IsColorEqual(newItem.Value.color, oldData[oldId].color))
            {
                knownConversions[oldId] = newItem.Key;
                return newItem.Key;
            }
        }

        return newData.First().Key;
    }

    private bool IsColorEqual(Color color1, Color color2)
    {
        if (color1 == color2) { return true; }

        float tolerance = 0.01f;
        return Math.Abs(color1.R - color2.R) < tolerance &&
               Math.Abs(color1.G - color2.G) < tolerance &&
               Math.Abs(color1.B - color2.B) < tolerance &&
               Math.Abs(color1.A - color2.A) < tolerance;
    }
}
