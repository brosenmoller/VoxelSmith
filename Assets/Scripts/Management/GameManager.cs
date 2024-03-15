using Godot;

public partial class GameManager : Node
{
    public static DataManager DataManager { get; private set; }
    public static CommandManager CommandManager { get; private set; }

    public static SurfaceMesh SurfaceMesh { get; private set; }
    public static PlayerMovement Player { get; private set; }
    public static UIController UIController { get; private set; }


    private Manager[] activeManagers;

    public override void _Ready()
    {
        SurfaceMesh = this.GetNodeByType<SurfaceMesh>();
        Player = this.GetNodeByType<PlayerMovement>();
        UIController = this.GetNodeByType<UIController>();

        SetupManagers();
        SetupInputContext();
    }

    private void SetupManagers()
    {
        DataManager = new DataManager();
        CommandManager = new CommandManager();

        activeManagers = new Manager[] {
            DataManager,
            CommandManager,
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
    }

    public override void _PhysicsProcess(double delta)
    {
        foreach (Manager manager in activeManagers)
        {
            manager.OnFixedUpdate();
        }
    }
}

