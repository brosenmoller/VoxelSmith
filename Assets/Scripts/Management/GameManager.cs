using Godot;

public partial class GameManager : Node
{
    public static DataManager DataManager { get; private set; }
    public static CommandManager CommandManager { get; private set; }
    public static ExportManager ExportManager { get; private set; }

    public static SurfaceMesh SurfaceMesh { get; private set; }
    public static PrefabMesh PrefabMesh { get; private set; }
    public static PlayerMovement Player { get; private set; }
    public static UIController UIController { get; private set; }
    public static PaletteUI PaletteUI { get; private set; }


    private Manager[] activeManagers;

    public override void _Ready()
    {
        Player = this.GetNodeByType<PlayerMovement>();
        UIController = this.GetNodeByType<UIController>();
        PaletteUI = this.GetNodeByType<PaletteUI>();
        
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

        activeManagers = new Manager[] {
            DataManager,
            CommandManager,
            ExportManager,
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
    }

    public override void _PhysicsProcess(double delta)
    {
        foreach (Manager manager in activeManagers)
        {
            manager.OnFixedUpdate();
        }
    }
}

