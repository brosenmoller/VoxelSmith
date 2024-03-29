using Godot;

public partial class PlayerMovement : CharacterBody3D
{
    [ExportGroup("Walk Mode")]
    [Export] public float walkSpeed = 5.0f;
    [Export] public float sprintSpeed = 8.0f;
    [Export] public float jumpVelocity = 4.8f;
    [Export] public float gravity = 9.81f;

    [ExportGroup("Fly Mode")]
    [Export] public Vector2 minMaxFlySpeed;
    [Export] public float flySpeedChangeStep;
    [Export] public float startFlySpeed;
    [Export] public float ySpeed;

    [ExportGroup("References")]
    [Export] public Node3D pivot;

    private bool active = false;

    private StateMachine<PlayerMovement> stateMachine;

    public void ChangeState<T>() where T : State<PlayerMovement>
    {
        stateMachine.ChangeState(typeof(T));
    }

    public override void _Ready()
    {
        stateMachine = new StateMachine<PlayerMovement>(this, new WalkState(), new FlyState());
        stateMachine.ChangeState(typeof(WalkState));
    }

    public override void _Process(double delta)
    {
        if (Input.IsKeyPressed(Key.Ctrl)) { active = false; }
        else 
        {
            stateMachine.OnUpdate(delta);
            active = true;
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        if (active)
        {
            stateMachine.OnPhysicsUpdate(delta);
        }
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (active)
        {
            stateMachine.CurrentState.UnHandledInput(@event);
        }
    }
}
