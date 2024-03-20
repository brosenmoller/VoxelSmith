using Godot;
using System.IO;
using System.Text;

public class ExportManager : Manager
{
    public override void OnFixedUpdate()
    {
        if (Input.IsActionJustPressed("debug"))
        {
            SaveMeshToFiles(GameManager.SurfaceMesh.Mesh, "C:\\Users\\Ben\\Downloads\\", "test");
        }
    }

    public void SaveMeshToFiles(Mesh mesh, string filePath, string objectName)
    {
        ArrayMesh arrayMesh = new();
        arrayMesh.AddSurfaceFromArrays(Mesh.PrimitiveType.Triangles, mesh.SurfaceGetArrays(0));

        MeshDataTool tool = new();
        tool.CreateFromSurface(arrayMesh, 0);

        StandardMaterial3D blankMaterial = new();
        blankMaterial.ResourceName = "BlankMaterial";

        StringBuilder output = new();
        StringBuilder matOutput = new();

        output.AppendLine("# Mesh Exported Using VoxelSmith");
        output.AppendLine("# https://github.com/brosenmoller/VoxelSmith");

        matOutput.AppendLine("# Material Exported Using VoxelSmith");
        matOutput.AppendLine("# https://github.com/brosenmoller/VoxelSmith");

        output.AppendLine("mtllib " + objectName + ".mtl");
        output.AppendLine("o " + objectName);

        output.AppendLine("vn -1 0 0");  // 1
        output.AppendLine("vn 1 0 0");   // 2
        output.AppendLine("vn 0 0 -1");  // 3
        output.AppendLine("vn 0 0 1");   // 4
        output.AppendLine("vn 0 -1 0");  // 5
        output.AppendLine("vn 0 1 0");   // 6

        StandardMaterial3D mat = (StandardMaterial3D)mesh.SurfaceGetMaterial(0);
        mat ??= blankMaterial;

        for (int i = 0; i < tool.GetVertexCount(); i++)
        {
            output.AppendLine($"v {tool.GetVertex(i).X} {tool.GetVertex(i).Y} {tool.GetVertex(i).Z}");
        }

        output.AppendLine($"usemtl {mat.ToString()}");

        for (int i = 0;i < tool.GetFaceCount(); i++)
        {
            Vector3I normal = (Vector3I)tool.GetFaceNormal(i);
            int normalIndex = 0;
            if (normal == Vector3.Left) { normalIndex = 1; }
            if (normal == Vector3.Right) { normalIndex = 2; }
            if (normal == Vector3.Back) { normalIndex = 3; }
            if (normal == Vector3.Forward) { normalIndex = 4; }
            if (normal == Vector3.Down) { normalIndex = 5; }
            if (normal == Vector3.Up) { normalIndex = 6; }

            output.AppendLine($"f {tool.GetFaceVertex(i, 2) + 1}//{normalIndex} {tool.GetFaceVertex(i, 1) + 1}//{normalIndex} {tool.GetFaceVertex(i, 0) + 1}//{normalIndex}");
        }

        matOutput.AppendLine($"newmtl {mat.ToString()}");
        matOutput.AppendLine($"Kd {mat.AlbedoColor.R.ToString()} {mat.AlbedoColor.G.ToString()} {mat.AlbedoColor.B.ToString()}");
        matOutput.AppendLine($"Ke {mat.Emission.R.ToString()} {mat.Emission.G.ToString()} {mat.Emission.B.ToString()}");
        matOutput.AppendLine($"d {mat.AlbedoColor.A.ToString()}");

        if (!filePath.EndsWith("/"))
        {
            filePath += "/";
        }

        string objFilePath = filePath + objectName + ".obj";
        File.WriteAllText(objFilePath, string.Empty);
        using (FileStream fileStream = File.Open(objFilePath, FileMode.OpenOrCreate))
        {
            byte[] info = new UTF8Encoding(true).GetBytes(output.ToString());
            fileStream.Write(info, 0, info.Length);
        }


        string matFilePath = filePath + objectName + ".mtl";
        File.WriteAllText(matFilePath, string.Empty);
        using (FileStream fileStream = File.Open(matFilePath, FileMode.OpenOrCreate))
        {
            byte[] info = new UTF8Encoding(true).GetBytes(matOutput.ToString());
            fileStream.Write(info, 0, info.Length);
        }
    }
}