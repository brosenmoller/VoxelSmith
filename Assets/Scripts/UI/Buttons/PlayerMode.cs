using Godot;
using System;

public partial class PlayerMode : HBoxContainer
{
    [Export] private CheckButton walkModeButton;
    [Export] private CheckButton flyModeButton;
    [Export] private CheckButton editModeButton;

    public static event Action OnWalkModePressed;
    public static event Action OnFlyModePressed;
    public static event Action OnEditModePressed;

    public override void _Ready()
    {
        walkModeButton.Pressed += () => OnWalkModePressed?.Invoke();
        flyModeButton.Pressed += () => OnFlyModePressed?.Invoke();
    }
}
