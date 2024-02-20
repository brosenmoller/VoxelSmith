using Godot;

public partial class PlayerMovement : CharacterBody3D
{
    [ExportGroup("Settings")]
    [Export] private float walkSpeed = 5.0f;
    [Export] private float sprintSpeed = 8.0f;
    [Export] private float jumpVelocity = 4.8f;
    [Export] private float gravity = 9.81f;

    [ExportGroup("References")]
    [Export] private Node3D pivot;

    private float speed;

    public bool active = false;

    public override void _PhysicsProcess(double delta)
    {
        //if (!active) { return; }

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
        Vector3 direction = (pivot.Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();

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

        MoveAndSlide();
    }
}
