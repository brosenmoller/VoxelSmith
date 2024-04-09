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
    public static UIController UIController { get; private set; }
    public static PaletteUI PaletteUI { get; private set; }
    public static TopBarUI TopBarUI { get; private set; }


    private Manager[] activeManagers;

    public override void _Ready()
    {
        Player = this.GetNodeByType<PlayerMovement>();
        UIController = this.GetNodeByType<UIController>();
        PaletteUI = this.GetNodeByType<PaletteUI>();
        TopBarUI = this.GetNodeByType<TopBarUI>();
        
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
        ProjectMenu.OnSaveAsPressed += UIController.saveProjectAsDialog.Show;
        ProjectMenu.OnNewPressed += UIController.newProjectDialog.Show;
        ProjectMenu.OnOpenPressed += UIController.loadProjectDialog.Show;

        ProjectMenu.OnExportUnityPrefabPressed += UIController.exportPrefabDialog.Show;
        ProjectMenu.OnExportMeshPressed += UIController.exportMeshDialog.Show;

        ProjectMenu.OnImportSchematicPressed += UIController.importSchematicFileDialog.Show;

        PaletteMenu.OnOpenPressed += UIController.loadPaletteDialog.Show;
        PaletteMenu.OnSaveAsPressed += UIController.savePaletteAsDialog.Show;
        PaletteMenu.OnNewPressed += UIController.newPaletteFileDialog.Show;
        PaletteMenu.OnSavePressed += DataManager.SavePalette;
        PaletteMenu.OnImportFromProjectPressed += UIController.importPaletteFromProjectFileDialog.Show;
    }

    public override void _Process(double delta)
    {
        foreach (Manager manager in activeManagers)
        {
            manager.OnUpdate(delta);
        }
    }
}

