using Godot;

public partial class Main : Control
{
    [ExportGroup("Buttons")]
    [Export] private Button saveButton;
    [Export] private Button exportButton;

    [ExportGroup("File Dialogs")]
    [Export] private FileDialog openDialog;
    [Export] private FileDialog saveDialog;

    private WorldController worldController;

    public override void _Ready()
    {
        worldController = this.GetChildByType<WorldController>();

        saveButton.Pressed += OnSaveButtonPressed;
        exportButton.Pressed += OnExportButtonPressed;

        openDialog.VisibilityChanged += UpdateFocus;
        saveDialog.VisibilityChanged += UpdateFocus;
    }

    private void OnSaveButtonPressed()
    {
        openDialog.Visible = true;
    }

    private void OnExportButtonPressed()
    {
        saveDialog.Visible = true;
    }

    private void UpdateFocus()
    {
        worldController.canGoInFocus = !(openDialog.Visible || saveDialog.Visible);
    }
}
