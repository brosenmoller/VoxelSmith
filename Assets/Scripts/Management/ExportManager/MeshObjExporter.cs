using System.Collections.Generic;
using System.IO;
using System.Text;
using Godot;

public class MeshObjExporter : IExporter
{
    private static readonly Dictionary<Vector2, int> cubeUVs = new()
    {
        { new(0, 0), 1 },
        { new(1, 1), 2 },
        { new(1, 0), 3 },
        { new(0, 1), 4 },
    };

    public void Export(ExportSettingsData exportSettings, EditorData.ExportPathData exportPath)
    {
        string name = exportPath.fileName;
        if (exportPath.fileName.Contains('.'))
        {
            name = exportPath.fileName[..exportPath.fileName.IndexOf(".")];
        }

        string output = ExportMeshGenerator.CreateMesh(exportSettings, GameManager.DataManager.ProjectData.voxelColors, GameManager.DataManager.PaletteData.paletteColors);
        File.WriteAllText(Path.Combine(exportPath.directoryPath, name + ".obj"), output);

        string matOutput = ConstructMaterial();
        File.WriteAllText(Path.Combine(exportPath.directoryPath, name + ".mtl"), matOutput);
    }

    private static void AddMeshObjectToString(StringBuilder output, MeshDataTool tool, string name)
    {
        output.AppendLine("o " + name);

        output.AppendLine("vn -1 0 0");  // 1
        output.AppendLine("vn 1 0 0");   // 2
        output.AppendLine("vn 0 0 -1");  // 3
        output.AppendLine("vn 0 0 1");   // 4
        output.AppendLine("vn 0 -1 0");  // 5
        output.AppendLine("vn 0 1 0");   // 6

        output.AppendLine("vt 0 0"); // 1
        output.AppendLine("vt 1 0"); // 2
        output.AppendLine("vt 0 1"); // 3
        output.AppendLine("vt 1 1"); // 4

        for (int i = 0; i < tool.GetVertexCount(); i++)
        {
            output.AppendLine($"v {tool.GetVertex(i).X} {tool.GetVertex(i).Y} {tool.GetVertex(i).Z}");
        }

        output.AppendLine($"usemtl defaultMat");

        for (int i = 0; i < tool.GetFaceCount(); i++)
        {
            Vector3I normal = (Vector3I)tool.GetFaceNormal(i);
            int normalIndex = 0;
            if (normal == Vector3I.Left) { normalIndex = 1; }
            else if (normal == Vector3I.Right) { normalIndex = 2; }
            else if (normal == Vector3I.Back) { normalIndex = 3; }
            else if (normal == Vector3I.Forward) { normalIndex = 4; }
            else if (normal == Vector3I.Down) { normalIndex = 5; }
            else if (normal == Vector3I.Up) { normalIndex = 6; }

            int vertex1 = tool.GetFaceVertex(i, 2) + 1;
            int vertex2 = tool.GetFaceVertex(i, 1) + 1;
            int vertex3 = tool.GetFaceVertex(i, 0) + 1;

            string uv1 = "";
            string uv2 = "";
            string uv3 = "";

            int vertexCount = tool.GetVertexCount();

            if (vertex1 < vertexCount)
            {
                uv1 = cubeUVs[tool.GetVertexUV(vertex1)].ToString();
            }

            if (vertex2 < vertexCount)
            {
                uv2 = cubeUVs[tool.GetVertexUV(vertex2)].ToString();
            }

            if (vertex3 < vertexCount)
            {
                uv3 = cubeUVs[tool.GetVertexUV(vertex3)].ToString();
            }

            output.AppendLine($"f {vertex1}/{uv1}/{normalIndex} {vertex2}/{uv2}/{normalIndex} {vertex3}/{uv3}/{normalIndex}");
        }
    }

    private static string ConstructMaterial()
    {
        StringBuilder matOutput = new();
        matOutput.AppendLine("# Material Exported Using VoxelSmith");
        matOutput.AppendLine("# https://github.com/brosenmoller/VoxelSmith");

        matOutput.AppendLine($"newmtl defaultMat");
        matOutput.AppendLine($"Kd 0.1 0.1 0.1");
        matOutput.AppendLine($"Ks 0.50 0.50 0.50");
        matOutput.AppendLine($"Ns 18.00");

        return matOutput.ToString();
    }
}