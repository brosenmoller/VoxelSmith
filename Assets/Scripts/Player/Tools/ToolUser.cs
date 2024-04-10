using Godot;
using System;
using System.Linq;

public partial class ToolUser : RayCast3D
{
    [Export] public bool enableCollisionHighlight;

    [ExportSubgroup("References")]
    [Export] public Node3D voxelHiglight;
    [Export] public Node3D collisionHighlight;

    [ExportSubgroup("Cube")]
    [Export] public Node3D cornerHighlight1;
    [Export] public Node3D cornerHighlight2;
    [Export] public Node3D cubeHighlight;

    private bool enabled = true;
    public bool checkForPlayerInside;

    private StateMachine<ToolUser> stateMachine;

    public bool HasHit { get; private set; }
    public Vector3 Point { get; private set; }
    public Vector3I VoxelPosition { get; private set; }
    public Vector3 Normal { get; private set; }

    public override void _Ready()
    {
        if (enableCollisionHighlight) { collisionHighlight.Show(); }

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
                collisionHighlight.Show();
                collisionHighlight.GlobalPosition = Point;
            }

            VoxelPosition = GetGridPositionFromHitPoint(Point, Normal);

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

    public Vector3I GetGridPositionFromHitPoint(Vector3 hitPoint, Vector3 normal)
    {
        Vector3 insetPoint = hitPoint - (normal * 0.1f);

        Vector3I voxelPosition = new(
            Mathf.FloorToInt(insetPoint.X),
            Mathf.FloorToInt(insetPoint.Y),
            Mathf.FloorToInt(insetPoint.Z)
        );

        return voxelPosition;
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
