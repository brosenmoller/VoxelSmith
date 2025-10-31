using Godot;
using System;

public partial class FirstPersonCamera : Node3D
{
    [ExportGroup("Base Settings")]
    [Export] private float sensitivity = 0.004f;
    [Export] private float minXRotation = -89;
    [Export] private float maxXRotation = 89;

    [ExportGroup("FOV")]
    [Export] private float baseFOV = 75.0f;
    [Export] private float FOVChange = 1.5f;
    [Export] private float maxFOVMultiplier = 16.0f;
    [Export] private float minFOVMultiplier = 0.5f;
    [Export] private bool fovChangeEnabled = false;

    [ExportGroup("View Bobbing")]
    [Export] private float bobFrequency = 2;
    [Export] private float bobAmplitude = 0.06f;
    [Export] private bool viewBobbingEnabled = false;

    [ExportGroup("References")]
    [Export] private CharacterBody3D playerMovement;

    private Camera3D camera;
    private float bobTime = 0;

    public override void _Ready()
    {
        camera = this.GetChildByType<Camera3D>();
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventMouseMotion mouseDelta)
        {
            RotateY(-mouseDelta.Relative.X * sensitivity);
            camera.RotateX(-mouseDelta.Relative.Y * sensitivity);
            camera.Rotation = camera.Rotation with { X = Mathf.Clamp(camera.Rotation.X, Mathf.DegToRad(minXRotation), Mathf.DegToRad(maxXRotation)) };
        }
    }

    public override void _Process(double delta)
    {
        if (Input.IsKeyPressed(Key.Bracketleft))
        {
            sensitivity -= 0.00005f;
            if (sensitivity < 0) { sensitivity = 0.00001f; }
        }
        else if (Input.IsKeyPressed(Key.Bracketright))
        {
            sensitivity += 0.00005f;
        }
        else if (Input.IsKeyPressed(Key.Backslash))
        {
            sensitivity = 0.004f;
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        // View Bobbing
        if (viewBobbingEnabled)
        {
            bobTime += (float)delta * playerMovement.Velocity.Length() * Convert.ToSingle(playerMovement.IsOnFloor());
            camera.Transform = camera.Transform with { Origin = Headbob(bobTime) };
        }

        // FOV
        if (fovChangeEnabled)
        {
            float velocityClamped = Mathf.Clamp(playerMovement.Velocity.Length(), minFOVMultiplier, maxFOVMultiplier);
            float targetFov = baseFOV + FOVChange * velocityClamped;
            camera.Fov = (float)Mathf.Lerp(camera.Fov, targetFov, delta * 8.0f);
        }
    }

    private Vector3 Headbob(float time)
    {
        Vector3 pos = Vector3.Zero;
        pos.Y = Mathf.Sin(time * bobFrequency) * bobAmplitude;
        pos.X = Mathf.Cos(time * bobFrequency / 2) * bobAmplitude;
        return pos;
    }
}