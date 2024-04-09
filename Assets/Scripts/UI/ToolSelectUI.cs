using Godot;

public partial class ToolSelectUI : Control
{
    [Export] private Button brushButton;
    [Export] private Button cubeButton;
    [Export] private Button lineButton;

    public override void _Ready()
    {
        brushButton.Pressed += () => GameManager.ToolUser.ChangeState(typeof(BrushTool));
        cubeButton.Pressed += () => GameManager.ToolUser.ChangeState(typeof(CubeTool));
        lineButton.Pressed += () => GameManager.ToolUser.ChangeState(typeof(LineTool));
    }
}

