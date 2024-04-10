using Godot;

public partial class GameManager : Node
{
    public static DataManager DataManager { get; private set; }
    public static CommandManager CommandManager { get; private set; }
    public static ExportManager ExportManager { get; private set; }
    public static ImportManager ImportManager { get; private set; }
    public static TimerManager TimerManager { get; private set; }

    public static SurfaceMesh SurfaceMesh { get; private set; }
    public static PrefabMesh PrefabMesh { get; private set; }
    public static PlayerMovement Player { get; private set; }
    public static ToolUser ToolUser { get; private set; }
    public static UIController UIController { get; private set; }
    public static PaletteUI PaletteUI { get; private set; }
    public static TopBarUI TopBarUI { get; private set; }
    public static NativeDialog NativeDialog { get; private set; }


    private Manager[] activeManagers;

    public override void _Ready()
    {
        Player = this.GetNodeByType<PlayerMovement>();
        ToolUser = this.GetNodeByType<ToolUser>();
        UIController = this.GetNodeByType<UIController>();
        PaletteUI = this.GetNodeByType<PaletteUI>();
        TopBarUI = this.GetNodeByType<TopBarUI>();
        NativeDialog = this.GetNodeByType<NativeDialog>();
        
        SurfaceMesh = this.GetNodeByType<SurfaceMesh>();
        PrefabMesh = this.GetNodeByType<PrefabMesh>();

        SurfaceMesh.Setup();
        PrefabMesh.Setup();

        SetupManagers();
        SetupInputContext();
    }

    private void SetupManagers()
    {
        DataManager = new DataManager();
        CommandManager = new CommandManager();
        ExportManager = new ExportManager();
        ImportManager = new ImportManager();
        TimerManager = new TimerManager();

        activeManagers = new Manager[] {
            DataManager,
            CommandManager,
            ExportManager,
            ImportManager,
            TimerManager
        };

        foreach (Manager manager in activeManagers)
        {
            manager.Setup();
        }
    }

    private void SetupInputContext()
    {
        EditMenu.OnUndoPressed += CommandManager.StepBack;
        EditMenu.OnRedoPressed += CommandManager.StepForward;

        ProjectMenu.OnSavePressed += DataManager.SaveProject;
        ProjectMenu.OnSaveAsPressed += UIController.ShowSaveProjectAsDialog;
        ProjectMenu.OnNewPressed += UIController.newProjectDialog.Show;
        ProjectMenu.OnOpenPressed += UIController.ShowLoadProjectDialog;

        ProjectMenu.OnExportUnityPrefabPressed += UIController.ShowExportUnityPrefabDialog;
        ProjectMenu.OnExportMeshPressed += UIController.ShowExportMeshDialog;

        ProjectMenu.OnImportSchematicPressed += UIController.ShowImportSchematicDialog;

        PaletteMenu.OnOpenPressed += UIController.ShowLoadPaletteDialog;
        PaletteMenu.OnSaveAsPressed += UIController.ShowSavePaletteAsDialog;
        PaletteMenu.OnNewPressed += UIController.ShowCreateNewPaletteDialog;
        PaletteMenu.OnSavePressed += DataManager.SavePalette;
        PaletteMenu.OnImportFromProjectPressed += UIController.ShowImportPaletteFromProjectDialog;
    }

    public override void _Process(double delta)
    {
        foreach (Manager manager in activeManagers)
        {
            manager.OnUpdate(delta);
        }
    }
}

