using Godot;

public class FlyState : State<PlayerMovement>
{
    private float speed;

    public override void OnEnter()
    {
        speed = stateOwner.Controller.startFlySpeed;
    }

    public override void UnHandledInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseEvent)
        {
            if (mouseEvent.IsPressed())
            {
                if (mouseEvent.ButtonIndex == MouseButton.WheelUp)
                {
                    speed += stateOwner.Controller.flySpeedChangeStep;
                }
                if (mouseEvent.ButtonIndex == MouseButton.WheelDown)
                {
                    speed -= stateOwner.Controller.flySpeedChangeStep;
                }

                speed = Mathf.Clamp(speed, stateOwner.Controller.minMaxFlySpeed.X, stateOwner.Controller.minMaxFlySpeed.Y);
            }
        }
    }

    public override void OnPhysicsUpdate(double delta)
    {
        Vector3 tempVelocity = stateOwner.Controller.Velocity;

        // Handle ascend and descend
        if (Input.IsActionPressed("ascend") && !Input.IsActionPressed("descend"))
        {

            tempVelocity.Y = stateOwner.Controller.ySpeed;
        }
        else if (Input.IsActionPressed("descend") && !Input.IsActionPressed("ascend"))
        {
            tempVelocity.Y = -stateOwner.Controller.ySpeed;
        }
        else
        {
            tempVelocity.Y = 0;
        }

        // Get the input direction and handle the movement/deceleration.
        Vector2 inputDir = Input.GetVector("left", "right", "up", "down");
        Vector3 direction = (stateOwner.Controller.pivot.Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();

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

        stateOwner.Controller.Velocity = tempVelocity;

        stateOwner.Controller.MoveAndSlide();
    }
}

