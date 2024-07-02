using Godot;
using System;

public partial class WorldController : Node3D
{
    public static WorldController Instance { get; private set; }
    public static event Action WentInFocusLastFrame;
    public static event Action WentOutOfFocus;

    private PlayerMovement player;
    private VoxelSmith.Timer wentInFocusEventTimer;

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

    public override void _Ready()
    {
        Instance = this;
        wentInFocusEventTimer = new(0.01f, SendWentInFocusEvent, false);
        
        player = this.GetChildByType<PlayerMovement>();
        WorldInFocus = false;
    }

    public override void _Notification(int notificationID)
    {
        switch (notificationID)
        {
            case (int)NotificationApplicationFocusOut:
                WorldInFocus = false;
                break;
            
            case (int)NotificationApplicationFocusIn:
                
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