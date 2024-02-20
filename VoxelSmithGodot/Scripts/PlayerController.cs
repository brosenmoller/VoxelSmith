using Godot;
using System;

public partial class PlayerController : CharacterBody3D
{
    [ExportGroup("Settings")]
    [Export] private float walkSpeed = 5.0f;
    [Export] private float sprintSpeed = 8.0f;
    [Export] private float jumpVelocity = 4.8f;
    [Export] private float gravity = 9.81f;

    [ExportGroup("Camera")]
    [Export] private float sensitivity = 0.004f;
    [Export] private float baseFOV = 75.0f;
    [Export] private float FOV_Change = 1.5f;

    [ExportGroup("View Bobbing")]
    [Export] private float bobFrequency = 2;
    [Export] private float bobAmplitude = 0.06f;
    [Export] private float tBob = 0;

    private float speed;

    public bool active = false;

    private Node3D cameraPivot;
    private Camera3D camera;

    public override void _Ready()
    {
        camera = this.GetChildByType<Camera3D>();
        cameraPivot = GetNode<Node3D>("HeadPivot");
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventMouseMotion eventMouseButton && active)
        {
            cameraPivot.RotateY(-eventMouseButton.Relative.X * sensitivity);
            camera.RotateX(-eventMouseButton.Relative.Y * sensitivity);
            camera.Rotation = camera.Rotation with { X = Mathf.Clamp(camera.Rotation.X, Mathf.DegToRad(-40), Mathf.DegToRad(60)) };
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        if (!active) { return; }

        Vector3 tempVelocity = Velocity;

        // Add the gravity.
        if (!IsOnFloor())
        {
            tempVelocity.Y -= gravity * (float)delta;
        }
            

        // Handle Jump.
        if (Input.IsActionJustPressed("jump") && IsOnFloor())
        {
            tempVelocity.Y = jumpVelocity;
        }
            
        // Handle Sprint.
        if (Input.IsActionPressed("sprint"))
        {
            speed = sprintSpeed;
        }
        else
        {
            speed = walkSpeed;
        }
            
        // Get the input direction and handle the movement/deceleration.
        Vector2 inputDir = Input.GetVector("left", "right", "up", "down");
        Vector3 direction = (cameraPivot.Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();

        if (IsOnFloor())
        {
            if (direction != Vector3.Zero)
            {
                tempVelocity.X = direction.X * speed;
                tempVelocity.Z = direction.Z * speed;
            }
            else
            {
                tempVelocity.X = (float)Mathf.Lerp(tempVelocity.X, direction.X * speed, delta * 7.0f);
                tempVelocity.Z = (float)Mathf.Lerp(tempVelocity.Z, direction.Z * speed, delta * 7.0f);
            }
        }
        else
        {
            tempVelocity.X = (float)Mathf.Lerp(tempVelocity.X, direction.X * speed, delta * 3.0f);
            tempVelocity.Z = (float)Mathf.Lerp(tempVelocity.Z, direction.Z * speed, delta * 3.0f);
        }

        Velocity = tempVelocity;

        // Head bob
        tBob += (float)delta * Velocity.Length() * Convert.ToSingle(IsOnFloor());
        camera.Transform = camera.Transform with { Origin = Headbob(tBob) };


        // FOV
        float velocityClamped = Mathf.Clamp(Velocity.Length(), 0.5f, sprintSpeed * 2);
        float targetFov = baseFOV + FOV_Change * velocityClamped;
        camera.Fov = (float)Mathf.Lerp(camera.Fov, targetFov, delta * 8.0f);

        MoveAndSlide();
    }

    private Vector3 Headbob(float time)
    {
        Vector3 pos = Vector3.Zero;
        pos.Y = Mathf.Sin(time * bobFrequency) * bobAmplitude;
        pos.X = Mathf.Cos(time * bobFrequency / 2) * bobAmplitude;
        return pos;
    }
}
