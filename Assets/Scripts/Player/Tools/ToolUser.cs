using Godot;
using System;
using System.Linq;

public partial class ToolUser : RayCast3D
{
    [Export] public bool enableCollisionHighlight;

    [ExportSubgroup("References")]
    [Export] public Node3D voxelHiglight;
    [Export] public Material whiteMaterial;
    [Export] public Material redMaterial;
    [Export] public Node3D meshHighlight;
    public MeshInstance3D meshHighlightMeshInstance;

    [ExportSubgroup("Cube")]
    [Export] public Node3D cornerHighlight1;
    [Export] public Node3D cornerHighlight2;

    private bool enabled = true;
    public bool ignorePlayerCheck;
    public bool selectInsideEnabled = false;

    private StateMachine<ToolUser> stateMachine;

    public bool HasHit { get; private set; }
    public Vector3 Point { get; private set; }
    public Vector3I VoxelPosition { get; private set; }
    public Vector3 Normal { get; private set; }

    private IMeshGenerator<VoxelData> meshGenerator;

    public override void _Ready()
    {
        if (enableCollisionHighlight) { meshHighlight.Show(); }

        meshGenerator = new BasicMeshGenerator<VoxelData>();
        meshHighlightMeshInstance = meshHighlight.GetChildByType<MeshInstance3D>();

        WorldController.WentInFocusLastFrame += () => enabled = true;
        WorldController.WentOutOfFocus += () => enabled = false;

        Tool[] tools = new Tool[]
        {
            new VoxelBrushTool(),
            new VoxelSpeedBrushTool(),
            new VoxelCubeTool(),
            new VoxelLineTool(),
            new SelectionBrushTool(),
            new SelectionCubeTool()
        };

        stateMachine = new StateMachine<ToolUser>(this, tools);
        ChangeState(typeof(VoxelBrushTool));
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
            
            Tool tool = (Tool)stateMachine.CurrentState;
            IToolOptions toolOptions = tool.GetToolOptions();

            if (toolOptions != null)
            {
                GameManager.ToolOptionsUI.SetToolOptions(toolOptions);
            }
            else
            {
                GameManager.ToolOptionsUI.ClearToolOptions();
            }
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
                meshHighlight.Show();
                meshHighlight.GlobalPosition = Point;
            }

            VoxelPosition = VoxelHelper.GetGridVoxelFromHitPoint(Point, Normal);

            if (!enabled) { return; }

            if (Input.IsActionJustPressed("pick_block"))
            {
                PickBlock(VoxelPosition);
            }
        }
        else
        {
            HasHit = false;
            voxelHiglight.Hide();
        }
    }

    public void GenerateVoxelBasedMeshHighlight(Vector3I[] voxelPositions)
    {
        meshHighlightMeshInstance.Mesh = meshGenerator.CreateMesh(voxelPositions);
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

    public Vector3I GetVoxelPositionFromLook(float checkLength, float emptyDistance, bool selectInside = false)
    {
        Vector3 normalizedGlobalDirection = (-1 * GlobalTransform.Basis.Z).Normalized();

        Vector3 checkEndPoint = normalizedGlobalDirection * checkLength + GlobalPosition;

        if (this.RayCast3D(GlobalPosition, checkEndPoint, out var hitInfo, CollisionMask, false, true))
        {
            Vector3I voxelPostion = VoxelHelper.GetGridVoxelFromHitPoint(hitInfo.position, hitInfo.normal);
            if (selectInside)
            {
                return voxelPostion;
            }
            else
            {
                Vector3I nextVoxel = voxelPostion + (Vector3I)hitInfo.normal.Normalized();
                return nextVoxel;
            }
        }

        Vector3 emptySpacePoint = normalizedGlobalDirection * emptyDistance + GlobalPosition;
        return VoxelHelper.GetGridVoxelFromHitPoint(emptySpacePoint, normalizedGlobalDirection);
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
