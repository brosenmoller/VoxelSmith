using Godot;

public partial class GameManager : Node
{
    public static GameManager Instance { get; private set; }

    public static DataManager DataManager { get; private set; }

    private Manager[] activeManagers;

    public override void _Ready()
    {
        SingletonSetup();
        ManagerSetup();
    }

    private void SingletonSetup()
    {
        if (Instance == null) { Instance = this; }
        else { QueueFree(); }
    }

    private void ManagerSetup()
    {
        DataManager = new DataManager();

        activeManagers = new Manager[] {
            DataManager
        };

        foreach (Manager manager in activeManagers)
        {
            manager.Setup();
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        foreach (Manager manager in activeManagers)
        {
            manager.OnFixedUpdate();
        }
    }
}

