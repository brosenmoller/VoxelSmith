using Godot;

public partial class ToolSelectUI : Control
{
    [Export] private Button brushButton;
    [Export] private Button speedBrushButton;
    [Export] private Button cubeButton;
    [Export] private Button lineButton;
    [Export] private Button selectionBrushButton;
    [Export] private Button selectionCubeButton;

    public override void _Ready()
    {
        brushButton.Pressed += () => GameManager.ToolUser.ChangeState(typeof(VoxelBrushTool));
        speedBrushButton.Pressed += () => GameManager.ToolUser.ChangeState(typeof(VoxelSpeedBrushTool));
        cubeButton.Pressed += () => GameManager.ToolUser.ChangeState(typeof(VoxelCubeTool));
        lineButton.Pressed += () => GameManager.ToolUser.ChangeState(typeof(VoxelLineTool));
        selectionBrushButton.Pressed += () => GameManager.ToolUser.ChangeState(typeof(SelectionBrushTool));
        selectionCubeButton.Pressed += () => GameManager.ToolUser.ChangeState(typeof(SelectionCubeTool));
    }
}
