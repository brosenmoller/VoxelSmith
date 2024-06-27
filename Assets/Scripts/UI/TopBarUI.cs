using Godot;

public partial class TopBarUI : Control
{
    [Export] private PlayerMode playerMode;
    [Export] private Button hasDisabledCollisionsToggle;
    [Export] private Button hasInfiniteReachToggle;
    [Export] private Button isSelectingInsideToggle;
    [Export] private Slider playerSpeedSlider;
    [Export] private RichTextLabel playerSpeedPercentageText;

    [Export] private float defaultReach = 7;
    [Export] private float infiniteReach = 100;

    public override void _Ready()
    {
        ToggleInfiniteReach(hasInfiniteReachToggle.ButtonPressed);
        ToggleNoColissions(hasDisabledCollisionsToggle.ButtonPressed);

        playerMode.OnPlayerMovementModeSelected += GameManager.Player.ChangeState;

        hasInfiniteReachToggle.Toggled += ToggleInfiniteReach;
        hasDisabledCollisionsToggle.Toggled += ToggleNoColissions;
        isSelectingInsideToggle.Toggled += ToggleSelectingInside;

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
        if (Input.IsActionJustReleased("increase_speed"))
        {
            playerSpeedSlider.Value += playerSpeedSlider.Step;
        }
        else if (Input.IsActionJustReleased("decrease_speed"))
        {
            playerSpeedSlider.Value -= playerSpeedSlider.Step;
        }
    }

    public void ToggleInfiniteReach(bool toggle)
    {
        GameManager.ToolUser.TargetPosition = toggle ? new Vector3(0, 0, -infiniteReach) : new Vector3(0, 0, -defaultReach);
    }

    public void ToggleNoColissions(bool toggle)
    {
        GameManager.SurfaceMesh.SetCollisionLayerValue(3, toggle);
        GameManager.PrefabMesh.SetCollisionLayerValue(3, toggle);
        GameManager.ToolUser.checkForPlayerInside = toggle;
    }

    public void ToggleSelectingInside(bool toggle)
    {
        GameManager.ToolUser.selectInsideEnabled = toggle;
    }
}

