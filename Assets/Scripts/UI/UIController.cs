using Godot;

public partial class UIController : Control
{
    [ExportGroup("Window References")]
    [Export] public ConfirmationDialog newProjectDialog;
    [Export] public FileDialog loadProjectDialog;
    [Export] public FileDialog saveProjectAsDialog;
    [Export] public FileDialog exportPrefabDialog;
    [Export] public StartWindow startWindow;

    private WorldController worldController;

    private Window[] windows;

    public override void _Ready()
    {
        worldController = this.GetChildByType<WorldController>();

        windows = this.GetAllChildrenByType<Window>();

        for (int i = 0; i < windows.Length; i++)
        {
            windows[i].VisibilityChanged += UpdateFocus;
        }

        UpdateFocus();
    }

    private void UpdateFocus()
    {
        bool aWindowIsVisible = false;
        for (int i = 0; i < windows.Length; i++)
        {
            if (windows[i].Visible) 
            {
                aWindowIsVisible = true;
                break;
            }
        }

        if (!aWindowIsVisible && GameManager.DataManager.ProjectData == null)
        {
            startWindow.Show();
            aWindowIsVisible = true;
        }

        if (aWindowIsVisible)
        {
            worldController.WorldInFocus = false;
        }

        worldController.canGoInFocus = !aWindowIsVisible;
    }
}
