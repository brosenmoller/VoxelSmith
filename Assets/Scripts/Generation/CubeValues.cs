using Godot;

public static class CubeValues
{
    public const float FACE_LEFT = 0;
    public const float FACE_RIGHT = 1;
    public const float FACE_BOTTOM = 2;
    public const float FACE_TOP = 3;
    public const float FACE_BACK = 4;
    public const float FACE_FRONT = 5;

    public static readonly Vector3[] cubeVertices =
    {
        new(0, 0, 0), 
        new(1, 0, 0),
        new(1, 0, 1), 
        new(0, 0, 1),
        new(0, 1, 0), 
        new(1, 1, 0),
        new(1, 1, 1), 
        new(0, 1, 1),
    };

    public static readonly Vector3I[] cubeOffsets =
    {
        new(-1, 0, 0),
        new(1, 0, 0),
        new(0, -1, 0),
        new(0, 1, 0),
        new(0, 0, -1),
        new(0, 0, 1),
    };

    public static readonly Vector2[] cubeUVs =
    {
        new(0, 0),
        new(1, 0),
        new(0, 1),
        new(1, 1),
    };

    public static readonly int[][] cubeVertexFaceIndices = new int[][]
    {
        new[] { 0, 7, 3, 0, 4, 7 },
        new[] { 2, 5, 1, 2, 6, 5 },
        new[] { 1, 3, 2, 1, 0, 3 },
        new[] { 4, 5, 7, 5, 6, 7 },
        new[] { 0, 1, 5, 5, 4, 0 },
        new[] { 3, 6, 2, 3, 7, 6 },
    };

    public static readonly int[][] cubeUVFaceIndices = new int[][]
    {
        new[] { 0, 3, 1, 0, 2, 3 },
        new[] { 1, 2, 0, 1, 3, 2 },
        new[] { 0, 1, 3, 0, 2, 3 },
        new[] { 2, 3, 0, 3, 1, 0 },
        new[] { 0, 1, 3, 3, 2, 0 },
        new[] { 1, 2, 0, 1, 3, 2 },
    };

    public static readonly int[][] cubeUVFaceIndicesUnity = new int[][]
    {
        new[] { 0, 1, 2, 0, 3, 1 },
        new[] { 0, 1, 2, 0, 3, 1 },
        new[] { 0, 1, 2, 0, 3, 1 },
        new[] { 0, 3, 2, 3, 1, 2 },
        new[] { 0, 1, 3, 3, 2, 0 },
        new[] { 0, 1, 2, 0, 3, 1 },
    };
}
