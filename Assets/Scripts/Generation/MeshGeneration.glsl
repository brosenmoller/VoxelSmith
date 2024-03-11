#[compute]
#version 450

struct Triangle {
    vec3 vertexC;
    vec3 vertexB;
    vec3 vertexA;
};

layout(set = 0, binding = 0) buffer MapBuffer {
    float map[];
};

layout(set = 0, binding = 1) buffer TriangleBuffer {
    Triangle triangles[];
};

uniform float surfaceLevel;
uniform uint mapSize;
uniform float squareSize;

vec3 interpolateVerts(vec4 v1, vec4 v2) {
    float t = (surfaceLevel - v1.w) / (v2.w - v1.w);
    return v1.xyz + t * (v2.xyz - v1.xyz);
}

int indexFromCoord(int x, int y, int z) {
    return int(z * mapSize * mapSize + y * mapSize + x);
}

vec4 cubeCorner(uint x, uint y, uint z) {
    float isoLevel = map[indexFromCoord(x, y, z)];

    return vec4(
        -1.0 * float(mapSize) * squareSize / 2.0 + float(x) * squareSize + squareSize / 2.0,
        -1.0 * float(mapSize) * squareSize / 2.0 + float(y) * squareSize + squareSize / 2.0,
        -1.0 * float(mapSize) * squareSize / 2.0 + float(z) * squareSize + squareSize / 2.0,
        isoLevel
    );
}

void main() {

    ivec3 id = ivec3(gl_GlobalInvocationID);

    if (id.x >= mapSize - 1 || id.y >= mapSize - 1 || id.z >= mapSize - 1) {
        return;
    }

    // 8 corners of the current cube
    vec4 cubeCorners[8] = vec4[](
        cubeCorner(id.x, id.y, id.z),
        cubeCorner(id.x + 1, id.y, id.z),
        cubeCorner(id.x + 1, id.y, id.z + 1),
        cubeCorner(id.x, id.y, id.z + 1),
        cubeCorner(id.x, id.y + 1, id.z),
        cubeCorner(id.x + 1, id.y + 1, id.z),
        cubeCorner(id.x + 1, id.y + 1, id.z + 1),
        cubeCorner(id.x, id.y + 1, id.z + 1)
    );

    // Calculate unique index for each cube configuration.
    // There are 256 possible values
    // A value of 0 means cube is entirely inside surface; 255 entirely outside.
    // The value is used to look up the edge table, which indicates which edges of the cube are cut by the isosurface.
    int cubeIndex = 0;
    if (cubeCorners[0].w < surfaceLevel) cubeIndex |= 1;
    if (cubeCorners[1].w < surfaceLevel) cubeIndex |= 2;
    if (cubeCorners[2].w < surfaceLevel) cubeIndex |= 4;
    if (cubeCorners[3].w < surfaceLevel) cubeIndex |= 8;
    if (cubeCorners[4].w < surfaceLevel) cubeIndex |= 16;
    if (cubeCorners[5].w < surfaceLevel) cubeIndex |= 32;
    if (cubeCorners[6].w < surfaceLevel) cubeIndex |= 64;
    if (cubeCorners[7].w < surfaceLevel) cubeIndex |= 128;

    // Create triangles for the current cube configuration
    for (int i = 0; triangulation[cubeIndex][i] != -1; i += 3) {
        // Get indices of corner points A and B for each of the three edges
        // of the cube that need to be joined to form the triangle.
        int a0 = cornerIndexAFromEdge[triangulation[cubeIndex][i]];
        int b0 = cornerIndexBFromEdge[triangulation[cubeIndex][i]];

        int a1 = cornerIndexAFromEdge[triangulation[cubeIndex][i + 1]];
        int b1 = cornerIndexBFromEdge[triangulation[cubeIndex][i + 1]];

        int a2 = cornerIndexAFromEdge[triangulation[cubeIndex][i + 2]];
        int b2 = cornerIndexBFromEdge[triangulation[cubeIndex][i + 2]];

        Triangle tri;
        tri.vertexA = interpolateVerts(cubeCorners[a0], cubeCorners[b0]);
        tri.vertexB = interpolateVerts(cubeCorners[a1], cubeCorners[b1]);
        tri.vertexC = interpolateVerts(cubeCorners[a2], cubeCorners[b2]);
        triangles.push_back(tri);
    }
}
