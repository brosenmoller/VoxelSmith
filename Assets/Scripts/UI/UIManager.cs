using Godot;
using System.Collections.Generic;

public partial class UIManager : Control
{
    [ExportGroup("Window References")]
    [Export] private ConfirmationDialog newProjectDialog;
    [Export] private FileDialog loadProjectDialog;
    [Export] private FileDialog saveProjectAsDialog;
    [Export] private Window startWindow;

    private WorldController worldController;

    private List<Window> windows;

    public override void _Ready()
    {
        worldController = this.GetChildByType<WorldController>();

        windows = new List<Window>
        {
            newProjectDialog,
            loadProjectDialog,
            saveProjectAsDialog,
            startWindow
        };

        for (int i = 0; i < windows.Count; i++)
        {
            windows[i].VisibilityChanged += UpdateFocus;
        }
    }

    private void UpdateFocus()
    {
        bool aWindowIsVisible = false;
        for (int i = 0; i < windows.Count; i++)
        {
            if (windows[i].Visible) 
            {
                aWindowIsVisible = true;
                break;
            }
        }

        worldController.canGoInFocus = !aWindowIsVisible;
    }
}
