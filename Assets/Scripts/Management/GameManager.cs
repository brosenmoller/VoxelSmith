using Godot;

public partial class GameManager : Node
{
    public static DataManager DataManager { get; private set; }
    public static CommandManager CommandManager { get; private set; }

    public static SurfaceMesh SurfaceMesh { get; private set; }
    
    private Manager[] activeManagers;

    public override void _Ready()
    {
        SetupManagers();
        SetupInputContext();

        SurfaceMesh = this.GetChildByType<SurfaceMesh>();
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
    }

    public override void _PhysicsProcess(double delta)
    {
        foreach (Manager manager in activeManagers)
        {
            manager.OnFixedUpdate();
        }
    }
}

