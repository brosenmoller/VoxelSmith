using Godot;

public partial class WorldController : Node3D
{
    public static WorldController Instance { get; private set; }

    private PlayerMovement player;

    private bool worldInFocus;
    public bool WorldInFocus { 
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
        Instance = this;

        player = this.GetChildByType<PlayerMovement>();
        WorldInFocus = false;
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