using Godot;

public partial class ToolSelectUI : Control
{
    [Export] private Button brushButton;
    [Export] private Button speedBrushButton;
    [Export] private Button cubeButton;
    [Export] private Button lineButton;
    [Export] private Button selectionBrushButton;

    public override void _Ready()
    {
        brushButton.Pressed += () => GameManager.ToolUser.ChangeState(typeof(VoxelBrushTool));
        speedBrushButton.Pressed += () => GameManager.ToolUser.ChangeState(typeof(SpeedBrushTool));
        cubeButton.Pressed += () => GameManager.ToolUser.ChangeState(typeof(CubeTool));
        lineButton.Pressed += () => GameManager.ToolUser.ChangeState(typeof(LineTool));
        selectionBrushButton.Pressed += () => GameManager.ToolUser.ChangeState(typeof(SelectionBrushTool));
    }
}

