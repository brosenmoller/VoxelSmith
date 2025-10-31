using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class WorldController : Node3D
{
    [Export] public Node3D groundNode;

    [ExportGroup("Snapping Points")]
    [Export] public Node3D snappingPointParent;
    [Export] public PackedScene snappingPointScene;
    [Export] public Material lineMaterial;
    [Export] public Color lineColor;

    public static event Action WentInFocusLastFrame;
    public static event Action WentOutOfFocus;

    private PlayerMovement player;
    private VoxelSmith.Timer wentInFocusEventTimer;

    private LineMeshInstance lineMeshInstance;

    private bool worldInFocus;
    public bool WorldInFocus { 
        get { return worldInFocus; } 
        set 
        {
            if (value) { wentInFocusEventTimer.Restart(); }
            else { WentOutOfFocus?.Invoke(); }

            worldInFocus = value;
            UpdatePlayerProcess();
        }
    }

    public bool canGoInFocus = true;

    public override void _EnterTree()
    {
        DataManager.OnProjectLoad += HandleProjectLoad;
    }

    public override void _Ready()
    {
        wentInFocusEventTimer = new(0.01f, SendWentInFocusEvent, false);
        
        player = this.GetChildByType<PlayerMovement>();
        WorldInFocus = false;
    }

    private void HandleProjectLoad()
    {
        foreach (Node child in snappingPointParent.GetChildren())
        {
            child.QueueFree();
        }

        if (GameManager.DataManager.ProjectData.snappingPoints == null) { return; }

        if (lineMeshInstance == null) 
        {
            lineMeshInstance = new LineMeshInstance();
            snappingPointParent.AddChild(lineMeshInstance);
        }

        foreach (Vector3 position in GameManager.DataManager.ProjectData.snappingPoints)
        {
            Node3D instance = snappingPointScene.Instantiate() as Node3D;
            instance.Position = position;
            snappingPointParent.AddChild(instance);
        }
    }

    public override void _Notification(int notificationID)
    {
        switch (notificationID)
        {
            case (int)NotificationApplicationFocusOut:
                WorldInFocus = false;
                break;
        }
    }

    public override void _Process(double delta)
    {
        if (Input.IsActionPressed("unlock_mouse"))
        {
            WorldInFocus = false;
        }

        if (Input.IsActionJustPressed("lock_mouse"))
        {
            Vector2 mousePos = GetViewport().GetMousePosition();
            Vector2 size = GetViewport().GetVisibleRect().Size;

            if (mousePos.X > 0 && mousePos.X < size.X &&
                mousePos.Y > 0 && mousePos.Y < size.Y &&
                canGoInFocus)
            {
                WorldInFocus = true;
            }
        }

        if (lineMeshInstance == null) { return; }
        if (!Input.IsMouseButtonPressed(MouseButton.Middle)) 
        {
            lineMeshInstance.Visible = false;
            return; 
        }

        lineMeshInstance.Visible = true;

        List<(Vector3, Vector3)> segements = [];
        foreach (Node3D child in snappingPointParent.GetChildren().Cast<Node3D>())
        {
            segements.Add((child.Position, GameManager.Player.Position));
        }

        lineMeshInstance.DrawLines(segements, lineColor);
    }

    private void SendWentInFocusEvent()
    {
        WentInFocusLastFrame?.Invoke();
    }

    private void UpdatePlayerProcess()
    {
        if (WorldInFocus)
        {
            Input.MouseMode = Input.MouseModeEnum.Captured;
            player.ProcessMode = ProcessModeEnum.Inherit;
        }
        else
        {
            Input.MouseMode = Input.MouseModeEnum.Visible;
            player.ProcessMode = ProcessModeEnum.Disabled;
        }
    }
}