using System.Collections.Generic;
using System.Text.RegularExpressions;

public class ExportSettingsData
{
    public enum ExportType
    {
        Mesh = 0,
        UnityPrefab = 1,
        GodotScene = 2,
    }

    private static readonly Dictionary<ExportType, string> fileExtensions = new()
    {
        { ExportType.Mesh, ".obj" },
        { ExportType.UnityPrefab, ".prefab" },
        { ExportType.GodotScene, ".tscn" },
    };

    public ExportType exportType = ExportType.UnityPrefab;

    public bool enableBarrierBlockCulling = true;
    public bool enableGreedyMeshing = true;
    public bool enableChunkedMeshing = true;
    public bool enableSeparateFloorAndCeiling = true;
    public bool enableVertexMerging = true;

    public string ExportMessage => GetExportMessage(exportType);
    public static string GetExportMessage(ExportType type) => $"{FormatExportType(type)} ({fileExtensions[type]})";
    private static string FormatExportType(ExportType type) => Regex.Replace(type.ToString(), "(?<!^)([A-Z])", " $1");
}