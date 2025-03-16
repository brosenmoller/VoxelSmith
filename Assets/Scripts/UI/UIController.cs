using Godot;

public partial class UIController : Control
{
    [ExportGroup("Window References")]
    [Export] public StartWindow startWindow;

    [ExportGroup("Dialogs")]
    [Export] public ConfirmationDialog newProjectDialog;
    [Export] public ConfirmationDialog importConfirmationDialog;
    [Export] public ExportWindow exportWindow;

    private string importPath;

    public WorldController worldController;

    private Window[] windows;

    public override void _Ready()
    {
        importConfirmationDialog.Confirmed += () => GameManager.ImportManager.ImportMinecraftSchematic(importPath);
        importConfirmationDialog.GetLabel().HorizontalAlignment = HorizontalAlignment.Center;

        worldController = this.GetChildByType<WorldController>();
        windows = this.GetAllChildrenByType<Window>();

        for (int i = 0; i < windows.Length; i++)
        {
            windows[i].VisibilityChanged += UpdateFocus;
        }

        UpdateFocus();
    }

    public void ShowLoadProjectDialog()
    {
        GameManager.NativeDialog.ShowFileDialog("Open a Project File", DisplayServer.FileDialogMode.OpenFile, new string[] { "*.vxsProject" }, (NativeDialog.Info info) =>
        {
            GameManager.DataManager.LoadProject(info.path);
        });
    }

    public void ShowSaveProjectAsDialog()
    {
        GameManager.NativeDialog.ShowFileDialog("Save Project As", DisplayServer.FileDialogMode.SaveFile, new string[] { "*.vxsProject" }, (NativeDialog.Info info) =>
        {
            GameManager.DataManager.SaveProjectAs(info.path);
        });
    }

    public void ShowCreateNewPaletteDialog()
    {
        GameManager.NativeDialog.ShowFileDialog("Create New Palette", DisplayServer.FileDialogMode.SaveFile, new string[] { "*.vxsPalette" }, (NativeDialog.Info info) =>
        {
            GameManager.DataManager.CreateNewPalette(info.path);
        });
    }
    public void ShowLoadPaletteDialog()
    {
        GameManager.NativeDialog.ShowFileDialog("Load Palette", DisplayServer.FileDialogMode.OpenFile, new string[] { "*.vxsPalette" }, (NativeDialog.Info info) =>
        {
            GameManager.DataManager.LoadPalette(info.path);
        });
    }

    public void ShowSavePaletteAsDialog()
    {
        GameManager.NativeDialog.ShowFileDialog("Save Palette As", DisplayServer.FileDialogMode.SaveFile, new string[] { "*.vxsPalette" }, (NativeDialog.Info info) =>
        {
            GameManager.DataManager.SavePaletteAs(info.path);
        });
    }

    public void ShowImportPaletteFromProjectDialog()
    {
        GameManager.NativeDialog.ShowFileDialog("Import Palette From Project", DisplayServer.FileDialogMode.OpenFile, new string[] { "*.vxsProject" }, (NativeDialog.Info info) =>
        {
            GameManager.DataManager.ImportPaletteFromProject(info.path);
        });
    }

    public void ShowImportSchematicDialog()
    {
        GameManager.NativeDialog.ShowFileDialog("Import Minecraft Schematic", DisplayServer.FileDialogMode.OpenFile, new string[] { "*.schem", "*.schematic" }, (NativeDialog.Info info) =>
        {
            ImportPath(info.path);
        });
    }


    public void ImportPath(string path)
    {
        importPath = path;

        if (GameManager.DataManager.ProjectData.voxelColors.Count <= 0 &&
            GameManager.DataManager.ProjectData.voxelPrefabs.Count <= 0)
        {
            GameManager.ImportManager.ImportMinecraftSchematic(path);
        }
        else
        {
            importConfirmationDialog.Show();
        }
    }

    private void UpdateFocus()
    {
        bool aWindowIsVisible = false;

        if (GameManager.NativeDialog.visible)
        {
            aWindowIsVisible = true;
        }
        else
        {
            for (int i = 0; i < windows.Length; i++)
            {
                if (windows[i].Visible)
                {
                    aWindowIsVisible = true;
                    break;
                }
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
