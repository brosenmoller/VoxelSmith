using Godot;

public static class CubeValues
{
    public const float FACE_LEFT = 0;
    public const float FACE_RIGHT = 1;
    public const float FACE_BOTTOM = 2;
    public const float FACE_TOP = 3;
    public const float FACE_BACK = 4;
    public const float FACE_FRONT = 5;

    public static readonly Vector3I[] cubeVertices =
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

    public static readonly Vector3I[] cubeDiagonals =
{
        new(1, -1, 1),
        new(1, -1, -1),
        new(-1, -1, 1),
        new(-1, -1, -1),

        new(1, 0, 1),
        new(1, 0, -1),
        new(-1, 0, 1),
        new(-1, 0, -1),

        new(1, 1, 1),
        new(1, 1, -1),
        new(-1, 1, 1),
        new(-1, 1, -1),
    };

    public static readonly Vector2I[] cubeUVs =
    {
        new(0, 0),
        new(1, 0),
        new(0, 1),
        new(1, 1),
    };

    public static readonly int[] cubeUVFaceIndices = { 0, 2, 3, 0, 3, 1 };

    public static readonly int[][] cubeVertexFaceIndices_Godot = new int[][]
    {
        new[] { 0, 4, 7, 0, 7, 3 },
        new[] { 2, 6, 5, 2, 5, 1 },
        new[] { 0, 3, 2, 0, 2, 1 },
        new[] { 7, 4, 5, 7, 5, 6 },
        new[] { 1, 5, 4, 1, 4, 0 },
        new[] { 3, 7, 6, 3, 6, 2 },
    };

    public static readonly int[][] cubeVertexFaceIndices_Export = new int[][]
{
        new[] { 0, 7, 4, 0, 3, 7 },
        new[] { 2, 5, 6, 2, 1, 5 },
        new[] { 0, 2, 3, 0, 1, 2 },
        new[] { 7, 5, 4, 7, 6, 5 },
        new[] { 1, 4, 5, 1, 0, 4 },
        new[] { 3, 6, 7, 3, 2, 6 },
};
}