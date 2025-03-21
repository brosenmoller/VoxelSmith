using System.IO;
using System.Text;

public class MeshObjExporter : IExporter
{
    public void Export(ExportSettingsData exportSettings, EditorData.ExportPathData exportPath)
    {
        string name = exportPath.fileName;
        if (exportPath.fileName.Contains('.'))
        {
            name = exportPath.fileName[..exportPath.fileName.IndexOf(".")];
        }

        string output = ExportMeshGenerator.CreateMesh(exportSettings, GameManager.DataManager.ProjectData.voxelColors, GameManager.DataManager.PaletteData.paletteColors, name);
        File.WriteAllText(Path.Combine(exportPath.directoryPath, name + ".obj"), output);

        string matOutput = ConstructMaterial();
        File.WriteAllText(Path.Combine(exportPath.directoryPath, name + ".mtl"), matOutput);
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