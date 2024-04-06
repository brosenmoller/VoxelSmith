using Godot;
using System;

[Serializable]
public class TopBarSettings
{
    public int playerMovementState;
    public int hasDisabledCollisions;
    public int hasInfiniteReach;
    public float playerSpeed;
}

public partial class TopBarUI : Control
{
    [Export] private PlayerMode playerMode;
    [Export] private Button hasDisabledCollisionsToggle;
    [Export] private Button hasInfiniteReachToggle;
    [Export] private Slider playerSpeedSlider;

    public override void _Ready()
    {
        playerMode.OnPlayerMovmenetModeSelected += GameManager.Player.ChangeState;

        playerSpeedSlider.MinValue = GameManager.Player.minMaxFlySpeed.X;
        playerSpeedSlider.MaxValue = GameManager.Player.minMaxFlySpeed.Y;
        playerSpeedSlider.Step = GameManager.Player.flySpeedChangeStep;
    }

    public void LoadSettings(TopBarSettings settings)
    {
        playerMode.Update((PlayerMovementState)settings.playerMovementState);
        GameManager.Player.ChangeState((PlayerMovementState)settings.playerMovementState);

        hasDisabledCollisionsToggle.ButtonPressed = settings.hasDisabledCollisions > 0;
        

        hasInfiniteReachToggle.ButtonPressed = settings.hasInfiniteReach > 0;

    }

    public TopBarSettings GetSettings()
    {
        return new TopBarSettings()
        {
            playerMovementState = (int)GameManager.Player.currentState,
            hasDisabledCollisions = hasDisabledCollisionsToggle.ButtonPressed ? 1 : 0,
            hasInfiniteReach = hasInfiniteReachToggle.ButtonPressed ? 1 : 0,
            playerSpeed = (float)playerSpeedSlider.Value
        };
    }
}

