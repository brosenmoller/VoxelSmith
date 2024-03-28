using Godot;

public partial class UIController : Control
{
    [ExportGroup("Window References")]
    [Export] public StartWindow startWindow;

    [ExportGroup("Files")]
    [ExportSubgroup("Project")]
    [Export] public ConfirmationDialog newProjectDialog;
    [Export] public FileDialog loadProjectDialog;
    [Export] public FileDialog saveProjectAsDialog;

    [ExportSubgroup("Palette")]
    [Export] public FileDialog newPaletteFileDialog;
    [Export] public FileDialog loadPaletteDialog;
    [Export] public FileDialog savePaletteAsDialog;

    [ExportSubgroup("Export")]
    [Export] public FileDialog exportPrefabDialog;
    [Export] public FileDialog exportMeshDialog;

    [ExportSubgroup("Import")]
    [Export] public FileDialog importSchematicFileDialog;


    private WorldController worldController;

    private Window[] windows;

    public override void _Ready()
    {
        LinkFileDialogEvents();

        worldController = this.GetChildByType<WorldController>();
        windows = this.GetAllChildrenByType<Window>();

        for (int i = 0; i < windows.Length; i++)
        {
            windows[i].VisibilityChanged += UpdateFocus;
        }

        UpdateFocus();
    }

    private void LinkFileDialogEvents()
    {
        loadProjectDialog.Confirmed += () => GameManager.DataManager.LoadProject(loadProjectDialog.CurrentPath);
        saveProjectAsDialog.Confirmed += () => GameManager.DataManager.SaveProjectAs(saveProjectAsDialog.CurrentPath);

        newPaletteFileDialog.Confirmed += () => GameManager.DataManager.CreateNewPalette(newPaletteFileDialog.CurrentPath);
        loadPaletteDialog.Confirmed += () => GameManager.DataManager.LoadPalette(loadPaletteDialog.CurrentPath);
        savePaletteAsDialog.Confirmed += () => GameManager.DataManager.SavePaletteAs(savePaletteAsDialog.CurrentPath);

        exportPrefabDialog.Confirmed += () => GameManager.ExportManager.ExportUnityPrefab(exportPrefabDialog.CurrentDir, exportPrefabDialog.CurrentFile);
        exportMeshDialog.Confirmed += () => GameManager.ExportManager.ExportMesh(exportMeshDialog.CurrentDir, exportMeshDialog.CurrentFile);

        importSchematicFileDialog.Confirmed += () => GameManager.ImportManager.ImportMinecraftSchematic(importSchematicFileDialog.CurrentPath);
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
