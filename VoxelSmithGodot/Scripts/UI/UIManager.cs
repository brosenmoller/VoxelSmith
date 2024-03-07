using Godot;

public partial class UIManager : Control
{
    [ExportGroup("File Dialogs")]
    [Export] private ConfirmationDialog newProjectDialog;
    [Export] private FileDialog openProjectDialog;
    [Export] private FileDialog saveAsDialog;

    private WorldController worldController;

    public override void _Ready()
    {
        worldController = this.GetChildByType<WorldController>();

        newProjectDialog.VisibilityChanged += UpdateFocus;
        openProjectDialog.VisibilityChanged += UpdateFocus;
        saveAsDialog.VisibilityChanged += UpdateFocus;
    }

    private void OnSaveButtonPressed()
    {
        //openDialog.Visible = true;
    }

    private void OnExportButtonPressed()
    {
        //saveDialog.Visible = true;
    }

    private void UpdateFocus()
    {
        //worldController.canGoInFocus = !(openDialog.Visible || saveDialog.Visible);
    }
}
