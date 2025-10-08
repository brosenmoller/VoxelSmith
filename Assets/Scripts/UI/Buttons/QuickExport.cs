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
        if (!GameManager.IsInitialized) { return; }
        if (!GameManager.DataManager.EditorData.exportPaths.ContainsKey(GameManager.DataManager.ProjectData.id)) { return; }
        if (GameManager.DataManager.ProjectData.exportSettings == null) { return; }

        confirmationDialog.Show();
    }

    private void OnConfirmed()
    {
        if (!GameManager.IsInitialized) { return; }
        if (!GameManager.DataManager.EditorData.exportPaths.ContainsKey(GameManager.DataManager.ProjectData.id)) { return; }
        if (GameManager.DataManager.ProjectData.exportSettings == null) { return; }

        GameManager.ExportManager.PerformExport();
    }
}
