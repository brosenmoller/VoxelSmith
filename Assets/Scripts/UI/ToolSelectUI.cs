using Godot;

public partial class ToolSelectUI : Control
{
    [Export] private Button brushButton;
    [Export] private Button speedBrushButton;
    [Export] private Button cubeButton;
    [Export] private Button lineButton;
    [Export] private Button coverButton;
    [Export] private Button sphereButton;
    [Export] private Button bucketButton;
    [Export] private Button selectionBrushButton;
    [Export] private Button selectionCubeButton;

    public override void _Ready()
    {
        brushButton.Pressed += () => GameManager.ToolUser.ChangeState(typeof(VoxelBrushTool));
        speedBrushButton.Pressed += () => GameManager.ToolUser.ChangeState(typeof(VoxelSpeedBrushTool));
        cubeButton.Pressed += () => GameManager.ToolUser.ChangeState(typeof(VoxelCubeTool));
        lineButton.Pressed += () => GameManager.ToolUser.ChangeState(typeof(VoxelLineTool));
        coverButton.Pressed += () => GameManager.ToolUser.ChangeState(typeof(VoxelCoverTool));
        sphereButton.Pressed += () => GameManager.ToolUser.ChangeState(typeof(VoxelSphereTool));
        bucketButton.Pressed += () => GameManager.ToolUser.ChangeState(typeof(VoxelBucketTool));
        selectionBrushButton.Pressed += () => GameManager.ToolUser.ChangeState(typeof(SelectionBrushTool));
        selectionCubeButton.Pressed += () => GameManager.ToolUser.ChangeState(typeof(SelectionCubeTool));
    }
}