using Godot;

public partial class QuickExport : MarginContainer
{
    private Button button;
    private ConfirmationDialog confirmationDialog;

    public override void _Ready()
    {
        button = this.GetChildByType<Button>();
        confirmationDialog = this.GetChildByType<ConfirmationDialog>();

        confirmationDialog.GetLabel().HorizontalAlignment = HorizontalAlignment.Center;

        button.Pressed += OnButtonPress;
        confirmationDialog.Confirmed += OnConfirmed;
    }

    private void OnButtonPress()
    {
        confirmationDialog.Show();
    }

    private void OnConfirmed()
    {
        if (GameManager.DataManager.EditorData.exportPaths.ContainsKey(GameManager.DataManager.ProjectData.projectID))
        {
            EditorData.ExportSettings exportSettings = GameManager.DataManager.EditorData.exportPaths[GameManager.DataManager.ProjectData.projectID];

            if (exportSettings.exportType == (int)ProjectMenu.ExportOptions.UNITY)
            {
                GameManager.ExportManager.ExportUnityPrefab(exportSettings.directoryPath, exportSettings.fileName, false);
            }
            else if (exportSettings.exportType == (int)ProjectMenu.ExportOptions.MESH)
            {
                GameManager.ExportManager.ExportMesh(exportSettings.directoryPath, exportSettings.fileName, false);
            }
        }
    }
}
