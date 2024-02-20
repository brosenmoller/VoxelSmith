using Godot;

public partial class WorldController : Node3D
{
    private PlayerMovement player;

    private bool worldInFocus;
    private bool WorldInFocus { 
        get { return worldInFocus; } 
        set 
        {
            worldInFocus = value;
            UpdatePlayerProcess();
        }
    }

    public bool canGoInFocus = true;

    public override void _Ready()
    {
        player = this.GetChildByType<PlayerMovement>();
        WorldInFocus = false;
    }

    public override void _Process(double delta)
    {
        if (Input.IsActionPressed("unlock_mouse"))
        {
            WorldInFocus = false;
            Input.MouseMode = Input.MouseModeEnum.Visible;
        }

        if (Input.IsActionJustPressed("lock_mouse"))
        {
            Vector2 mousePos = GetViewport().GetMousePosition();
            Vector2 size = GetViewport().GetVisibleRect().Size;

            if (mousePos.X > 0 && mousePos.X < size.X &&
                mousePos.Y > 0 && mousePos.Y < size.Y &&
                canGoInFocus)
            {
                Input.MouseMode = Input.MouseModeEnum.Captured;
                WorldInFocus = true;
            }
        }
    }

    private void UpdatePlayerProcess()
    {
        if (WorldInFocus)
        {
            player.ProcessMode = ProcessModeEnum.Inherit;
        }
        else
        {
            player.ProcessMode = ProcessModeEnum.Disabled;
        }
    }
}