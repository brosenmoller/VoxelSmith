using System;
using System.IO;
using Godot;

public partial class ExportWindow : ConfirmationDialog
{
    [Export] private TextEdit exportFileName;
    [Export] private TextEdit saveDirectoryPath;
    [Export] private Button openProjectDirectoryButton;
    [Export] private OptionButton exportOptionButton;

    [ExportSubgroup("Check Buttons")]
    [Export] private CheckButton barrierBlockCullingButton;
    [Export] private CheckButton greedyMeshingButton;
    [Export] private CheckButton chunkedMeshingButton;
    [Export] private CheckButton seperateFloorAndCeilingButton;
    [Export] private CheckButton vertexMergingButton;

    public override void _Ready()
    {
        Confirmed += OnExportConfirmed;
        openProjectDirectoryButton.Pressed += OnOpenSaveFolderPressed;
        VisibilityChanged += HandleVisibilityChanged;

        SetupExportTypeOptionButton();
    }

    private void HandleVisibilityChanged()
    {
        if (!Visible)
        {
            GameManager.UIController.ClickBlockerLayer.Visible = false;
            return;
        }
        GameManager.UIController.ClickBlockerLayer.Visible = true;

        ExportSettingsData exportSettings = GameManager.DataManager.ProjectData.exportSettings;
        if (exportSettings != null)
        {
            barrierBlockCullingButton.ButtonPressed = exportSettings.enableBarrierBlockCulling;
            greedyMeshingButton.ButtonPressed = exportSettings.enableGreedyMeshing;
            chunkedMeshingButton.ButtonPressed = exportSettings.enableChunkedMeshing;
            seperateFloorAndCeilingButton.ButtonPressed = exportSettings.enableSeparateFloorAndCeiling;
            vertexMergingButton.ButtonPressed = exportSettings.enableVertexMerging;
            exportOptionButton.Selected = (int)exportSettings.exportType;
        }

        SetExportPathIfNeeded();

        EditorData.ExportPathData exportPathData = GameManager.DataManager.EditorData.exportPaths[GameManager.DataManager.ProjectData.id];
        exportFileName.Text = exportPathData.fileName;
        saveDirectoryPath.Text = exportPathData.directoryPath;
    }

    private static void SetExportPathIfNeeded()
    {
        if (GameManager.DataManager.EditorData.exportPaths.ContainsKey(GameManager.DataManager.ProjectData.id)) { return; }

        string path = GameManager.DataManager.EditorData.savePaths[GameManager.DataManager.ProjectData.id];
        string directoryPath = Path.GetDirectoryName(path);

        GameManager.DataManager.EditorData.exportPaths[GameManager.DataManager.ProjectData.id] = new()
        {
            fileName = GameManager.DataManager.ProjectData.name,
            directoryPath = directoryPath
        };
        GameManager.DataManager.SaveEditorData();
    }

    private void SetupExportTypeOptionButton()
    {
        foreach (ExportSettingsData.ExportType item in EnumUtil.GetValues<ExportSettingsData.ExportType>())
        {
            // Godot TSCN Unsupported right now
            if (item == ExportSettingsData.ExportType.GodotScene) { continue; }

            exportOptionButton.AddItem(ExportSettingsData.GetExportMessage(item), (int)item);
        }
    }

    private void OnExportConfirmed()
    {
        if (exportFileName.Text.Length <= 0 || saveDirectoryPath.Text.Length <= 0)
        {
            // TODO: Show Error
            return;
        }

        GameManager.DataManager.ProjectData.exportSettings = new()
        {
            exportType = (ExportSettingsData.ExportType)exportOptionButton.Selected,
            enableBarrierBlockCulling = barrierBlockCullingButton.ButtonPressed,
            enableGreedyMeshing = greedyMeshingButton.ButtonPressed,
            enableChunkedMeshing = chunkedMeshingButton.ButtonPressed,
            enableSeparateFloorAndCeiling = seperateFloorAndCeilingButton.ButtonPressed,
            enableVertexMerging = vertexMergingButton.ButtonPressed,
        };
        GameManager.DataManager.SaveProject();

        EditorData.ExportPathData exportPathData = new()
        {
            directoryPath = saveDirectoryPath.Text,
            fileName = exportFileName.Text,
        };

        GameManager.DataManager.EditorData.exportPaths[GameManager.DataManager.ProjectData.id] = exportPathData;
        GameManager.DataManager.SaveEditorData();

        GameManager.ExportManager.PerformExport();
        Hide();
    }

    private void OnOpenSaveFolderPressed()
    {
        SetExportPathIfNeeded();
        string path = GameManager.DataManager.EditorData.savePaths[GameManager.DataManager.ProjectData.id];
        string directoryPath = Path.GetDirectoryName(path);

        GameManager.NativeDialog.ShowFileDialog(
            "Select Export Directory",
            DisplayServer.FileDialogMode.OpenDir,
            [],
            info => OnDirectorySelected(info.path),
            directoryPath
        );
    }

    private void OnDirectorySelected(string path)
    {
        saveDirectoryPath.Text = path;
    }
}