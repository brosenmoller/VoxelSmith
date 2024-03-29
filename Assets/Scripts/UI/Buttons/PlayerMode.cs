using Godot;
using System;

public partial class PlayerMode : HBoxContainer
{
    [Export] private CheckButton walkModeButton;
    [Export] private CheckButton flyModeButton;
    [Export] private CheckButton editModeButton;

    public static event Action<PlayerMovementState> OnPlayerMovmenetModeSelected;

    public override void _Ready()
    {
        walkModeButton.Pressed += () => OnPlayerMovmenetModeSelected?.Invoke(PlayerMovementState.Walk);
        flyModeButton.Pressed += () => OnPlayerMovmenetModeSelected?.Invoke(PlayerMovementState.Fly);
    }
}
