using Godot;
using System.Collections.Generic;

public class ExportManager : Manager
{
    private Dictionary<ExportSettingsData.ExportType, IExporter> exporters;

    public void PerformExport()
    {
        ExportSettingsData exportSettings = GameManager.DataManager.ProjectData.exportSettings;
        EditorData.ExportPathData exportPath = GameManager.DataManager.EditorData.exportPaths[GameManager.DataManager.ProjectData.id];

        if (exportSettings == null || exportPath == null)
        {
            // TODO: Show Error
            GD.Print($"Export Failed: ExportSettingsData == null or ExportPathData == null");
            return;
        }

        exporters[exportSettings.exportType].Export(exportSettings, exportPath);
    }

    public override void Setup()
    {
        exporters = new();
        MeshObjExporter meshExporter = new();
        exporters.Add(ExportSettingsData.ExportType.Mesh, meshExporter);
        exporters.Add(ExportSettingsData.ExportType.UnityPrefab, new UnityPrefabExporter(meshExporter));
    }
}