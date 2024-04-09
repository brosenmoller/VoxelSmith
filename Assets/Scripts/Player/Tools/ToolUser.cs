using Godot;
using System;
using System.Linq;

public partial class ToolUser : RayCast3D
{
    [Export] private bool enableCollisionHighlight;
    [Export] private bool enableVoxelHighlight;

    [ExportSubgroup("References")]
    [Export] private Node3D voxelHiglight;
    [Export] private Node3D collisionHighlight;

    private bool enabled = true;
    public bool checkForPlayerInside;

    private StateMachine<ToolUser> stateMachine;

    public bool HasHit { get; private set; }
    public Vector3 Point { get; private set; }
    public Vector3I VoxelPosition { get; private set; }
    public Vector3 Normal { get; private set; }

    public override void _Ready()
    {
        if (enableCollisionHighlight) { collisionHighlight.Visible = true; }
        if (enableVoxelHighlight) { voxelHiglight.Visible = true; }

        WorldController.WentInFocusLastFrame += () => enabled = true;
        WorldController.WentOutOfFocus += () => enabled = false;

        stateMachine = new StateMachine<ToolUser>(
            this,
            new BrushTool(),
            new CubeTool(),
            new LineTool()
        );
        stateMachine.ChangeState(typeof(BrushTool));
    }

    public override void _Process(double delta)
    {
        DefaultBehaiviour();
        if (enabled)
        {
            stateMachine.OnUpdate(delta);
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        if (enabled)
        {
            stateMachine.OnPhysicsUpdate(delta);
        }
    }

    public void ChangeState(Type stateType)
    {
        if (stateMachine.stateDictionary.ContainsKey(stateType))
        {
            stateMachine.ChangeState(stateType);
        }
    }

    private void DefaultBehaiviour()
    {
        if (IsColliding())
        {
            HasHit = true;
            Point = GetCollisionPoint();
            Normal = GetCollisionNormal();

            if (enableCollisionHighlight)
            {
                collisionHighlight.Visible = true;
                collisionHighlight.GlobalPosition = Point;
            }

            Vector3 insetPoint = Point - (Normal * 0.1f);

            VoxelPosition = new(
                Mathf.FloorToInt(insetPoint.X),
                Mathf.FloorToInt(insetPoint.Y),
                Mathf.FloorToInt(insetPoint.Z)
            );

            if (enableVoxelHighlight)
            {
                voxelHiglight.Visible = true;
                voxelHiglight.GlobalPosition = VoxelPosition;
            }

            if (!enabled) { return; }

            if (Input.IsActionJustPressed("pick_block"))
            {
                PickBlock(VoxelPosition);
            }
        }
        else
        {
            HasHit = false;
            collisionHighlight.Visible = false;
            voxelHiglight.Visible = false;
        }
    }

    public bool IsVoxelInPlayer(Vector3I voxelPosition)
    {
        Vector3I[] playerVoxels = new Vector3I[2];
        playerVoxels[0] = new Vector3I(
            Mathf.FloorToInt(GlobalPosition.X),
            Mathf.FloorToInt(GlobalPosition.Y),
            Mathf.FloorToInt(GlobalPosition.Z)
        );
        playerVoxels[1] = playerVoxels[0] + Vector3I.Down;

        return playerVoxels.Contains(voxelPosition);
    }

    private void PickBlock(Vector3I voxelPosition)
    {
        if (GameManager.DataManager.ProjectData.voxelColors.ContainsKey(voxelPosition))
        {
            Guid paletteId = GameManager.DataManager.ProjectData.voxelColors[voxelPosition];

            if (GameManager.DataManager.PaletteData.paletteColors.ContainsKey(paletteId))
            {
                GameManager.DataManager.ProjectData.selectedPaletteType = PaletteType.Color;
                GameManager.DataManager.ProjectData.selectedPaletteSwatchId = paletteId;

                GameManager.PaletteUI.Update();
            }
        }
        else if (GameManager.DataManager.ProjectData.voxelPrefabs.ContainsKey(voxelPosition))
        {
            Guid paletteId = GameManager.DataManager.ProjectData.voxelPrefabs[voxelPosition];

            if (GameManager.DataManager.PaletteData.palletePrefabs.ContainsKey(paletteId))
            {
                GameManager.DataManager.ProjectData.selectedPaletteType = PaletteType.Prefab;
                GameManager.DataManager.ProjectData.selectedPaletteSwatchId = paletteId;

                GameManager.PaletteUI.Update();
            }
        }
    }
}
