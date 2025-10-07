using Godot;

public partial class TopBarUI : Control
{
    [Export] private PlayerMode playerMode;
    [Export] private Button floorToggle;
    [Export] private Button disableCollisionsToggle;
    [Export] private Button infiniteReachToggle;
    [Export] private Button selectInsideToggle;
    [Export] private Slider playerSpeedSlider;
    [Export] private RichTextLabel playerSpeedPercentageText;
    [Export] private RichTextLabel toolSizeText;
    [Export] public ProjectMenu ProjectMenu;

    [Export] private float defaultReach = 7;
    [Export] private float infiniteReach = 100;

    public override void _Ready()
    {
        ToggleInfiniteReach(infiniteReachToggle.ButtonPressed);
        ToggleNoColissions(disableCollisionsToggle.ButtonPressed);

        playerMode.OnPlayerMovementModeSelected += GameManager.Player.ChangeState;

        floorToggle.Toggled += ToggleFloor;
        disableCollisionsToggle.Toggled += ToggleNoColissions;
        infiniteReachToggle.Toggled += ToggleInfiniteReach;
        selectInsideToggle.Toggled += ToggleSelectingInside;

        playerSpeedSlider.ValueChanged += SpeedSliderOnValueChange;
        SpeedSliderOnValueChange(playerSpeedSlider.Value);
    }

    public void SpeedSliderOnValueChange(double value)
    {
        playerSpeedPercentageText.Text = Mathf.RoundToInt(value).ToString() + " %";

        GameManager.Player.horizontalFlySpeed = Mathf.Lerp(GameManager.Player.minMaxFlySpeed.X, GameManager.Player.minMaxFlySpeed.Y, (float)(value / 100));
        GameManager.Player.verticalFlySpeed = GameManager.Player.horizontalFlySpeed / 2;
    }

    public override void _Process(double delta)
    {
        if (Input.IsActionJustReleased("mouse_wheel_up"))
        {
            playerSpeedSlider.Value += playerSpeedSlider.Step;
        }
        else if (Input.IsActionJustReleased("mouse_wheel_down"))
        {
            playerSpeedSlider.Value -= playerSpeedSlider.Step;
        }
    }

    public void ToggleFloor(bool toggle)
    {
        GameManager.WorldController.groundNode.Visible = toggle;
        GameManager.WorldController.groundNode.ProcessMode = toggle ? ProcessModeEnum.Inherit : ProcessModeEnum.Disabled;
    }

    public void ToggleInfiniteReach(bool toggle)
    {
        GameManager.ToolUser.TargetPosition = toggle ? new Vector3(0, 0, -infiniteReach) : new Vector3(0, 0, -defaultReach);
    }

    public void ToggleNoColissions(bool toggle)
    {
        GameManager.SurfaceMesh.SetCollisionLayerValue(3, toggle);
        GameManager.PrefabMesh.SetCollisionLayerValue(3, toggle);
        GameManager.ToolUser.ignorePlayerCheck = toggle;
    }

    public void ToggleSelectingInside(bool toggle)
    {
        GameManager.ToolUser.selectInsideEnabled = toggle;
    }

    public void ToggleShowToolSizeText(bool toggle)
    {
        toolSizeText.Visible = toggle;
    }

    public void SetToolSizeText(Vector3 size)
    {
        toolSizeText.Text = $"x {size.X}, y {size.Y}, z {size.Z}";
    }
}