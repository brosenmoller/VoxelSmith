using Godot;

public partial class Main : Control
{
    [ExportGroup("Buttons")]
    [Export] private Button exportButton;
    [Export] private Button saveButton;

    [ExportGroup("File Dialogs")]
    [Export] private FileDialog openDialog;
    [Export] private FileDialog saveDialog;

    private WorldController worldController;

    public override void _Ready()
    {
        worldController = this.GetChildByType<WorldController>();

        exportButton.Pressed += OnExportButtonPressed;
        saveButton.Pressed += OnSaveButtonPressed;
    }

    private void OnSaveButtonPressed()
    {
        openDialog.Visible = true;
        worldController.canGoInFocus = false;
    }

    private void OnExportButtonPressed()
    {
        saveDialog.Visible = true;
        worldController.canGoInFocus = false;
    }
}
