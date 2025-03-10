using System;
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
        if (!Visible) { return; }

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

        if (GameManager.DataManager.EditorData.exportPaths.ContainsKey(GameManager.DataManager.ProjectData.id))
        {
            EditorData.ExportPathData pathData = GameManager.DataManager.EditorData.exportPaths[GameManager.DataManager.ProjectData.id];

            exportFileName.Text = pathData.fileName;
            saveDirectoryPath.Text = pathData.directoryPath;
        }
    }

    private void SetupExportTypeOptionButton()
    {
        foreach (ExportSettingsData.ExportType item in EnumUtil.GetValues<ExportSettingsData.ExportType>())
        {
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

        GameManager.ExportManager.PerformExport();
        Hide();
    }

    private void OnOpenSaveFolderPressed()
    {
        GameManager.NativeDialog.ShowFileDialog("Select Export Directory", DisplayServer.FileDialogMode.OpenDir, Array.Empty<string>(), (NativeDialog.Info info) =>
        {
            OnDirectorySelected(info.path);
        });
    }

    private void OnDirectorySelected(string path)
    {
        saveDirectoryPath.Text = path;
    }
}