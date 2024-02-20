using Godot;

public partial class WorldController : Node3D
{
    private PlayerMovement player;

    private bool worldInFocus = false;

    private bool windowInFocus = true;

    public bool canGoInFocus = true;

    public override void _Ready()
    {
        player = this.GetChildByType<PlayerMovement>();
    }

    public override void _Notification(int notification)
    {
        switch (notification)
        {
            case (int)MainLoop.NotificationApplicationFocusIn:
                windowInFocus = true;
                break;

            case (int)MainLoop.NotificationApplicationFocusOut:
                windowInFocus = false;
                worldInFocus = false;
                break;
        }
    }

    public override void _Process(double delta)
    {
        if (Input.IsActionPressed("unlock_mouse"))
        {
            worldInFocus = false;
            Input.MouseMode = Input.MouseModeEnum.Visible;
        }

        if (Input.IsActionJustPressed("lock_mouse"))
        {
            Vector2 mousePos = GetViewport().GetMousePosition();
            Vector2 size = GetViewport().GetVisibleRect().Size;

            if (mousePos.X > 0 && mousePos.X < size.X &&
                mousePos.Y > 0 && mousePos.Y < size.Y &&
                canGoInFocus && windowInFocus)
            {
                Input.MouseMode = Input.MouseModeEnum.Captured;
                worldInFocus = true;
            }
        }

        if (worldInFocus)
        {
            player.ProcessMode = ProcessModeEnum.Inherit;
        }
        else
        {
            player.ProcessMode = ProcessModeEnum.Disabled;
        }
    }
}