using Godot;

public class FlyState : State<PlayerMovement>
{
    public override void OnPhysicsUpdate(double delta)
    {
        Vector3 tempVelocity = ctx.Velocity;

        if (Input.IsActionPressed("ascend") && !Input.IsActionPressed("descend"))
        {
            tempVelocity.Y = ctx.verticalFlySpeed;
        }
        else if (Input.IsActionPressed("descend") && !Input.IsActionPressed("ascend"))
        {
            tempVelocity.Y = -ctx.verticalFlySpeed;
        }
        else
        {
            tempVelocity.Y = 0;
        }

        // Get the input direction and handle the movement/deceleration.
        Vector2 inputDir = Input.GetVector("left", "right", "up", "down");
        Vector3 direction = (ctx.pivot.Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();

        tempVelocity.X = direction.X * ctx.horizontalFlySpeed;
        tempVelocity.Z = direction.Z * ctx.horizontalFlySpeed;

        ctx.Velocity = tempVelocity;

        ctx.MoveAndSlide();
    }
}

