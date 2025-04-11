using Godot;
using System;

public readonly struct FaceData : IEquatable<FaceData>
{
    public readonly Vector3I position;
    public readonly Vector3I normal;

    public FaceData(Vector3I position, Vector3I normal)
    {
        this.position = position;
        this.normal = normal;
    }

    public readonly bool Equals(FaceData other)
    {
        return position.Equals(other.position) && normal.Equals(other.normal);
    }

    public override readonly bool Equals(object obj)
    {
        return obj is FaceData other && Equals(other);
    }

    public override readonly int GetHashCode()
    {
        return HashCode.Combine(position, normal);
    }

    public static bool operator ==(FaceData left, FaceData right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(FaceData left, FaceData right)
    {
        return !(left == right);
    }
}