using Godot;
using System;

public partial class PlayerMode : HBoxContainer
{
    [Export] private CheckButton walkModeButton;
    [Export] private CheckButton flyModeButton;
    [Export] private CheckButton editModeButton;

    public event Action<PlayerMovementState> OnPlayerMovementModeSelected;

    public void Update(PlayerMovementState state)
    {
        switch (state)
        {
            case PlayerMovementState.Walk:
                walkModeButton.ButtonPressed = true;
                break;
            case PlayerMovementState.Fly:
                flyModeButton.ButtonPressed = true;
                break;
        }
    }

    public override void _Ready()
    {
        walkModeButton.Pressed += () => OnPlayerMovementModeSelected?.Invoke(PlayerMovementState.Walk);
        flyModeButton.Pressed += () => OnPlayerMovementModeSelected?.Invoke(PlayerMovementState.Fly);
    }
}
