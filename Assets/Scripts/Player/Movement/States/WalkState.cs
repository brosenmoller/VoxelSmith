using Godot;

public class WalkState : State<PlayerMovement>
{
    private float speed;

    public override void OnPhysicsUpdate(double delta)
    {
        Vector3 tempVelocity = stateOwner.Controller.Velocity;

        // Add the gravity.
        if (!stateOwner.Controller.IsOnFloor())
        {
            tempVelocity.Y -= stateOwner.Controller.gravity * (float)delta;
        }

        // Handle Jump.
        if (Input.IsActionJustPressed("jump") && stateOwner.Controller.IsOnFloor())
        {
            tempVelocity.Y = stateOwner.Controller.jumpVelocity;
        }

        // Handle Sprint.
        if (Input.IsActionPressed("sprint"))
        {
            speed = stateOwner.Controller.sprintSpeed;
        }
        else
        {
            speed = stateOwner.Controller.walkSpeed;
        }

        // Get the input direction and handle the movement/deceleration.
        Vector2 inputDir = Input.GetVector("left", "right", "up", "down");
        Vector3 direction = (stateOwner.Controller.pivot.Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();

        if (stateOwner.Controller.IsOnFloor())
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

        stateOwner.Controller.Velocity = tempVelocity;

        stateOwner.Controller.MoveAndSlide();
    }
}

