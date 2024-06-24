using Godot;

public static class CubeValues
{
    public static readonly Vector3[] cubeVertices =
    {
        new(0, 0, 0), new(1, 0, 0),
        new(1, 0, 1), new(0, 0, 1),
        new(0, 1, 0), new(1, 1, 0),
        new(1, 1, 1), new(0, 1, 1)
    };

    public static readonly Vector3I[] cubeOffsets =
    {
        new(-1, 0, 0), // left
        new(1, 0, 0), // right
        new(0, -1, 0), // bottom
        new(0, 1, 0), // top
        new(0, 0, -1),  // back
        new(0, 0, 1) // front
    };

    public static readonly Vector2[] cubeUVs =
    {
        new(0, 0),
        new(1, 1),
        new(1, 0),
        new(0, 1),
    };
}
