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
        new(-1, 0, 0),  // Left face
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

    //public static readonly int[][] cubeVertexSquareFaceIndices = new int[][]
    //{
    //    new[] { 3, 0, 4, 7 }, // Left face
    //    new[] { 1, 2, 6, 5 }, // Right face
    //    new[] { 0, 1, 2, 3 }, // Bottom face
    //    new[] { 4, 5, 6, 7 }, // Top face
    //    new[] { 2, 3, 7, 6 }, // Back face
    //    new[] { 0, 1, 5, 4 }, // Front face
    //};

    //public static readonly int[][] cubeVertexSquareFaceIndices = new int[][]
    //{
    //    new[] { 0, 4, 7, 3 }, // Left face  (-X)
    //    new[] { 1, 2, 6, 5 }, // Right face (+X)
    //    new[] { 0, 1, 2, 3 }, // Bottom face (-Y)
    //    new[] { 4, 5, 6, 7 }, // Top face (+Y)
    //    new[] { 3, 7, 6, 2 }, // Back face  (-Z)
    //    new[] { 0, 1, 5, 4 }, // Front face (+Z)
    //};

    public static readonly int[][] cubeVertexSquareFaceIndices = new int[][]
    {
            new[] { 0, 3, 7, 4 }, // Left face  (-X)
            new[] { 1, 5, 6, 2 }, // Right face (+X)
            new[] { 0, 1, 2, 3 }, // Bottom face (-Y)
            new[] { 4, 7, 6, 5 }, // Top face (+Y)
            new[] { 3, 2, 6, 7 }, // Back face  (-Z)
            new[] { 0, 4, 5, 1 }, // Front face (+Z)
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

    public static readonly int[][] cubeVertexFaceIndicesExporter = new int[][]
    {
        new[] { 0, 3, 7, 0, 7, 4 },
        new[] { 2, 1, 5, 2, 5, 6 },
        new[] { 2, 3, 0, 2, 0, 1 },
        new[] { 5, 4, 7, 5, 7, 6 },
        new[] { 3, 2, 6, 3, 6, 7 },
        new[] { 1, 0, 4, 1, 4, 5 },
    };
}