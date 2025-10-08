using Godot;

public enum PlayerMovementState
{
    Walk = 0,
    Fly = 1
}

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

    [ExportGroup("References")]
    [Export] public Node3D pivot;

    private bool active = false;

    public float horizontalFlySpeed;
    public float verticalFlySpeed;

    private StateMachine<PlayerMovement> stateMachine;
    
    public PlayerMovementState currentState = PlayerMovementState.Fly;

    public void ChangeState(PlayerMovementState playerMovementState)
    {
        currentState = playerMovementState;

        switch (playerMovementState)
        {
            case PlayerMovementState.Walk: stateMachine.ChangeState(typeof(WalkState)); break;
            case PlayerMovementState.Fly: stateMachine.ChangeState(typeof(FlyState)); break;
        }
    }

    public override void _Ready()
    {
        stateMachine = new StateMachine<PlayerMovement>(this, new WalkState(), new FlyState());
        ChangeState(currentState);
    }

    public override void _Process(double delta)
    {
        if (!GameManager.IsInitialized) { return; }

        if (Input.IsKeyPressed(Key.Ctrl)) { active = false; }
        else 
        {
            stateMachine.OnUpdate(delta);
            active = true;
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        if (!GameManager.IsInitialized) { return; }

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