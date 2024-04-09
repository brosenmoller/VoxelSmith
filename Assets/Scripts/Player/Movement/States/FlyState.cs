using Godot;

public class FlyState : State<PlayerMovement>
{
    private float speed;
    private float ySpeed;

    public override void OnEnter()
    {
        speed = ctx.startFlySpeed;
        ySpeed = ctx.ySpeed;
    }

    public override void UnHandledInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseEvent)
        {
            if (mouseEvent.IsPressed())
            {
                if (mouseEvent.ButtonIndex == MouseButton.WheelUp)
                {
                    speed += ctx.flySpeedChangeStep;
                    ySpeed += ctx.flySpeedChangeStep / 2; 
                }
                if (mouseEvent.ButtonIndex == MouseButton.WheelDown)
                {
                    speed -= ctx.flySpeedChangeStep;
                    ySpeed -= ctx.flySpeedChangeStep / 2;
                }

                speed = Mathf.Clamp(speed, ctx.minMaxFlySpeed.X, ctx.minMaxFlySpeed.Y);
                ySpeed = Mathf.Clamp(ySpeed, ctx.minMaxFlySpeed.X * 2, ctx.minMaxFlySpeed.Y / 2);
            }
        }
    }

    public override void OnPhysicsUpdate(double delta)
    {
        Vector3 tempVelocity = ctx.Velocity;

        // Handle ascend and descend
        if (Input.IsActionPressed("ascend") && !Input.IsActionPressed("descend"))
        {

            tempVelocity.Y = ySpeed;
        }
        else if (Input.IsActionPressed("descend") && !Input.IsActionPressed("ascend"))
        {
            tempVelocity.Y = -ySpeed;
        }
        else
        {
            tempVelocity.Y = 0;
        }

        // Get the input direction and handle the movement/deceleration.
        Vector2 inputDir = Input.GetVector("left", "right", "up", "down");
        Vector3 direction = (ctx.pivot.Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();

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

        ctx.Velocity = tempVelocity;

        ctx.MoveAndSlide();
    }
}

