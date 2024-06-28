using Godot;
using System;
using System.Collections.Generic;

public class ComputeMeshGenerator<TVoxelData> : IMeshGenerator<TVoxelData> where TVoxelData : VoxelData
{

    //public ComputeMeshGenerator()
    //{
    //    RenderingDevice renderingDevice = RenderingServer.CreateLocalRenderingDevice();
    //    RDShaderFile shaderFile = GD.Load<RDShaderFile>("res://Assets/Scripts/Generation/ComputeShaders/VoxelMeshGenerator.glsl");

    //    RDShaderSpirV shaderBytecode = shaderFile.GetSpirV();
    //    Rid shaderID = renderingDevice.ShaderCreateFromSpirV(shaderBytecode);

    //    // Prepare our data. We use floats in the shader, so we need 32 bit.
    //    float[] input = new float[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
    //    byte[] inputBytes = new byte[input.Length * sizeof(float)];
    //    Buffer.BlockCopy(input, 0, inputBytes, 0, inputBytes.Length);

    //    // Create a storage buffer that can hold our float values.
    //    // Each float has 4 bytes (32 bit) so 10 x 4 = 40 bytes
    //    Rid bufferID = renderingDevice.StorageBufferCreate((uint)inputBytes.Length, inputBytes);

    //    RDUniform uniform = new()
    //    {
    //        UniformType = RenderingDevice.UniformType.StorageBuffer,
    //        Binding = 0
    //    };

    //    uniform.AddId(bufferID);
    //    Rid uniformSetID = renderingDevice.UniformSetCreate(new Godot.Collections.Array<RDUniform> { uniform }, shaderID, 0);

    //    Rid pipelineID = renderingDevice.ComputePipelineCreate(shaderID);
    //    long computeListBegin = renderingDevice.ComputeListBegin();

    //    renderingDevice.ComputeListBindComputePipeline(computeListBegin, pipelineID);
    //    renderingDevice.ComputeListBindUniformSet(computeListBegin, uniformSetID, 0);
    //    renderingDevice.ComputeListDispatch(computeListBegin, xGroups: 5, yGroups: 1, zGroups: 1);
    //    renderingDevice.ComputeListEnd();

    //    renderingDevice.Submit();
    //    renderingDevice.Sync(); // Maybe wait a few frames

    //    byte[] outputBytes = renderingDevice.BufferGetData(bufferID);
    //    float[] output = new float[input.Length];

    //    Buffer.BlockCopy(outputBytes, 0, output, 0, outputBytes.Length);

    //    GD.Print("Input: ", string.Join(", ", input));
    //    GD.Print("Output: ", string.Join(", ", output));
    //}

    public Mesh CreateColorMesh(Dictionary<Vector3I, Guid> voxels, Dictionary<Guid, TVoxelData> palette)
    {
        throw new NotImplementedException();
    }


    private readonly RenderingDevice renderingDevice;
    private readonly Rid shaderID;
    private readonly Rid pipelineID;

    public ComputeMeshGenerator()
    {
        renderingDevice = RenderingServer.CreateLocalRenderingDevice();
        RDShaderFile shaderFile = GD.Load<RDShaderFile>("res://Assets/Scripts/Generation/ComputeShaders/VoxelMeshGenerator.glsl");
        RDShaderSpirV shaderBytecode = shaderFile.GetSpirV();
        shaderID = renderingDevice.ShaderCreateFromSpirV(shaderBytecode);
        pipelineID = renderingDevice.ComputePipelineCreate(shaderID);
    }

    public Mesh CreateMesh(Vector3I[] voxelPositionList)
    {
        int voxelCount = voxelPositionList.Length;
        byte[] voxelDataBytes = new byte[voxelCount * 3 * sizeof(float)];
        for (int i = 0; i < voxelCount; i++)
        {
            Buffer.BlockCopy(BitConverter.GetBytes(voxelPositionList[i].X), 0, voxelDataBytes, (i * 3) * sizeof(float), sizeof(float));
            Buffer.BlockCopy(BitConverter.GetBytes(voxelPositionList[i].Y), 0, voxelDataBytes, (i * 3 + 1) * sizeof(float), sizeof(float));
            Buffer.BlockCopy(BitConverter.GetBytes(voxelPositionList[i].Z), 0, voxelDataBytes, (i * 3 + 2) * sizeof(float), sizeof(float));
        }

        Rid voxelBufferID = renderingDevice.StorageBufferCreate((uint)voxelDataBytes.Length, voxelDataBytes);
        Rid vertexBufferID = renderingDevice.StorageBufferCreate((uint)(voxelCount * 8 * 3 * sizeof(float)), null);
        Rid indexBufferID = renderingDevice.StorageBufferCreate((uint)(voxelCount * 36 * sizeof(int)), null);

        RDUniform voxelUniform = new()
        {
            UniformType = RenderingDevice.UniformType.StorageBuffer,
            Binding = 0
        };
        voxelUniform.AddId(voxelBufferID);

        RDUniform vertexUniform = new()
        {
            UniformType = RenderingDevice.UniformType.StorageBuffer,
            Binding = 1
        };
        vertexUniform.AddId(vertexBufferID);

        RDUniform indexUniform = new()
        {
            UniformType = RenderingDevice.UniformType.StorageBuffer,
            Binding = 2
        };
        indexUniform.AddId(indexBufferID);

        Rid uniformSetID = renderingDevice.UniformSetCreate(new Godot.Collections.Array<RDUniform> { voxelUniform, vertexUniform, indexUniform }, shaderID, 0);

        long computeListBegin = renderingDevice.ComputeListBegin();
        renderingDevice.ComputeListBindComputePipeline(computeListBegin, pipelineID);
        renderingDevice.ComputeListBindUniformSet(computeListBegin, uniformSetID, 0);
        renderingDevice.ComputeListDispatch(computeListBegin, (uint)Math.Ceiling(voxelCount / 2.0), 1, 1);
        renderingDevice.ComputeListEnd();

        renderingDevice.Submit();
        renderingDevice.Sync(); // Wait for GPU to finish

        byte[] vertexDataBytes = renderingDevice.BufferGetData(vertexBufferID);
        byte[] indexDataBytes = renderingDevice.BufferGetData(indexBufferID);

        Vector3[] vertices = new Vector3[vertexDataBytes.Length / (3 * sizeof(float))];
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = new Vector3(
                BitConverter.ToSingle(vertexDataBytes, i * 3 * sizeof(float)),
                BitConverter.ToSingle(vertexDataBytes, i * 3 * sizeof(float) + sizeof(float)),
                BitConverter.ToSingle(vertexDataBytes, i * 3 * sizeof(float) + 2 * sizeof(float))
            );
        }

        int[] indices = new int[indexDataBytes.Length / sizeof(int)];
        for (int i = 0; i < indices.Length; i++)
        {
            indices[i] = BitConverter.ToInt32(indexDataBytes, i * sizeof(int));
        }

        return BuildMesh(vertices, indices);
    }

    private Mesh BuildMesh(Vector3[] vertices, int[] indices)
    {
        ArrayMesh mesh = new();
        Godot.Collections.Array meshArrays = new();

        meshArrays.Resize((int)Mesh.ArrayType.Max);
        meshArrays[(int)Mesh.ArrayType.Vertex] = vertices;
        meshArrays[(int)Mesh.ArrayType.Index] = indices;

        mesh.AddSurfaceFromArrays(Mesh.PrimitiveType.Triangles, meshArrays);
        return mesh;
    }
}
