using Godot;

public partial class TopBarUI : Control
{
    [Export] private PlayerMode playerMode;
    [Export] private Button hasDisabledCollisionsToggle;
    [Export] private Button hasInfiniteReachToggle;
    [Export] private Slider playerSpeedSlider;

    [Export] private float defaultReach = 7;
    [Export] private float infiniteReach = 100;

    private VoxelPlacer playerReach;

    private StaticBody3D surfaceBody;
    private StaticBody3D prefabBody;

    public override void _Ready()
    {
        playerReach = GameManager.Player.GetChildByType<VoxelPlacer>();

        surfaceBody = GameManager.SurfaceMesh.GetParent<StaticBody3D>();
        prefabBody = GameManager.PrefabMesh.GetParent<StaticBody3D>();
        
        ToggleInfiniteReach(hasInfiniteReachToggle.ButtonPressed);
        ToggleNoColissions(hasDisabledCollisionsToggle.ButtonPressed);

        playerMode.OnPlayerMovmenetModeSelected += GameManager.Player.ChangeState;
        playerMode.OnPlayerMovmenetModeSelected += (PlayerMovementState state) => GD.Print(state.ToString());

        hasInfiniteReachToggle.Toggled += ToggleInfiniteReach;
        hasDisabledCollisionsToggle.Toggled += ToggleNoColissions;

        //playerSpeedSlider.MinValue = GameManager.Player.minMaxFlySpeed.X;
        //playerSpeedSlider.MaxValue = GameManager.Player.minMaxFlySpeed.Y;
        //playerSpeedSlider.Step = GameManager.Player.flySpeedChangeStep;
    }

    public void ToggleInfiniteReach(bool toggle)
    {
        playerReach.TargetPosition = toggle ? new Vector3(0, 0, -infiniteReach) : new Vector3(0, 0, -defaultReach);
    }

    public void ToggleNoColissions(bool toggle)
    {
        if (toggle)
        {
            surfaceBody.SetCollisionLayerValue(3, false);
            prefabBody.SetCollisionLayerValue(3, false);
            playerReach.checkForPlayerInside = false;
        }
        else
        {
            surfaceBody.SetCollisionLayerValue(3, true);
            prefabBody.SetCollisionLayerValue(3, true);
            playerReach.checkForPlayerInside = true;
        }
    }
}

