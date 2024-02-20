using Godot;
using System;

public partial class FirstPersonCamera : Node3D
{
    [ExportGroup("Camera")]
    [Export] private float sensitivity = 0.004f;
    [Export] private float baseFOV = 75.0f;
    [Export] private float FOVChange = 1.5f;
    [Export] private float maxFOVMultiplier = 16.0f;
    [Export] private float minFOVMultiplier = 0.5f;

    [ExportGroup("View Bobbing")]
    [Export] private float bobFrequency = 2;
    [Export] private float bobAmplitude = 0.06f;
    [Export] private float tBob = 0;

    [ExportGroup("References")]
    [Export] private PlayerMovement playerMovement;

    private Camera3D camera;

    public override void _Ready()
    {
        camera = this.GetChildByType<Camera3D>();
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventMouseMotion eventMouseButton)
        {
            RotateY(-eventMouseButton.Relative.X * sensitivity);
            camera.RotateX(-eventMouseButton.Relative.Y * sensitivity);
            camera.Rotation = camera.Rotation with { X = Mathf.Clamp(camera.Rotation.X, Mathf.DegToRad(-40), Mathf.DegToRad(60)) };
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        tBob += (float)delta * playerMovement.Velocity.Length() * Convert.ToSingle(playerMovement.IsOnFloor());
        camera.Transform = camera.Transform with { Origin = Headbob(tBob) };


        // FOV
        float velocityClamped = Mathf.Clamp(playerMovement.Velocity.Length(), minFOVMultiplier, maxFOVMultiplier);
        float targetFov = baseFOV + FOVChange * velocityClamped;
        camera.Fov = (float)Mathf.Lerp(camera.Fov, targetFov, delta * 8.0f);
    }

    private Vector3 Headbob(float time)
    {
        Vector3 pos = Vector3.Zero;
        pos.Y = Mathf.Sin(time * bobFrequency) * bobAmplitude;
        pos.X = Mathf.Cos(time * bobFrequency / 2) * bobAmplitude;
        return pos;
    }
}

