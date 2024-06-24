#[compute]
#version 450

layout(local_size_x = 4, local_size_y = 1, local_size_z = 1) in;

layout(set = 0, binding = 0, std430) restrict buffer VoxelBuffer {
    vec3 voxels[];
} voxel_buffer;

layout(set = 0, binding = 1, std430) buffer VertexBuffer {
    vec3 vertices[];
} vertex_buffer;

layout(set = 0, binding = 2, std430) buffer IndexBuffer {
    int indices[];
} index_buffer;

void main() {
    uint index = gl_GlobalInvocationID.x;

    // Example: simple voxel to mesh conversion
    // You will need to implement a proper greedy meshing algorithm here

    // Each voxel is a cube of size 1
    vec3 voxel = voxel_buffer.voxels[index];

    // Compute vertices of the voxel
    vec3 v0 = voxel;
    vec3 v1 = voxel + vec3(1.0, 0.0, 0.0);
    vec3 v2 = voxel + vec3(1.0, 1.0, 0.0);
    vec3 v3 = voxel + vec3(0.0, 1.0, 0.0);
    vec3 v4 = voxel + vec3(0.0, 0.0, 1.0);
    vec3 v5 = voxel + vec3(1.0, 0.0, 1.0);
    vec3 v6 = voxel + vec3(1.0, 1.0, 1.0);
    vec3 v7 = voxel + vec3(0.0, 1.0, 1.0);

    // Store vertices (example for a single voxel)
    uint vertexIndex = index * 8;
    vertex_buffer.vertices[vertexIndex + 0] = v0;
    vertex_buffer.vertices[vertexIndex + 1] = v1;
    vertex_buffer.vertices[vertexIndex + 2] = v2;
    vertex_buffer.vertices[vertexIndex + 3] = v3;
    vertex_buffer.vertices[vertexIndex + 4] = v4;
    vertex_buffer.vertices[vertexIndex + 5] = v5;
    vertex_buffer.vertices[vertexIndex + 6] = v6;
    vertex_buffer.vertices[vertexIndex + 7] = v7;

    // Store indices (example for a single voxel)
    uint indexOffset = index * 36;
    int baseIndex = int(vertexIndex);
    int[] voxelIndices = int[](
        baseIndex + 0, baseIndex + 1, baseIndex + 2, baseIndex + 2, baseIndex + 3, baseIndex + 0,  // Front
        baseIndex + 4, baseIndex + 5, baseIndex + 6, baseIndex + 6, baseIndex + 7, baseIndex + 4,  // Back
        baseIndex + 0, baseIndex + 4, baseIndex + 7, baseIndex + 7, baseIndex + 3, baseIndex + 0,  // Left
        baseIndex + 1, baseIndex + 5, baseIndex + 6, baseIndex + 6, baseIndex + 2, baseIndex + 1,  // Right
        baseIndex + 3, baseIndex + 2, baseIndex + 6, baseIndex + 6, baseIndex + 7, baseIndex + 3,  // Top
        baseIndex + 0, baseIndex + 1, baseIndex + 5, baseIndex + 5, baseIndex + 4, baseIndex + 0   // Bottom
    );

    for (uint i = 0; i < 36; i++) {
        index_buffer.indices[indexOffset + i] = voxelIndices[i];
    }
}