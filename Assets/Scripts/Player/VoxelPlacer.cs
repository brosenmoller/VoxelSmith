using Godot;
using System.Collections.Generic;

public partial class VoxelPlacer : RayCast3D
{
    [Export] private bool enableCollisionHighlight;
    [Export] private bool enableVoxelHighlight;

    [ExportSubgroup("References")]
    [Export] private Node3D voxelHiglight;
    [Export] private Node3D collisionHighlight;

    private SurfaceMesh levelMesh;

    private Dictionary<Vector3I, VoxelData> Voxels => GameManager.DataManager.ProjectData.voxels;

    public override void _Ready()
    {
        levelMesh = this.GetNodeByType<SurfaceMesh>();

        if (enableCollisionHighlight) { collisionHighlight.Visible = true; }
        if (enableVoxelHighlight) { voxelHiglight.Visible = true; }
    }

    public override void _Process(double delta)
    {
        if (IsColliding())
        {
            Vector3 point = GetCollisionPoint();
            Vector3 normal = GetCollisionNormal();

            if (enableCollisionHighlight) { collisionHighlight.GlobalPosition = point; }

            point -= normal * 0.1f;

            Vector3I voxelPosition = new(
                Mathf.FloorToInt(point.X),
                Mathf.FloorToInt(point.Y),
                Mathf.FloorToInt(point.Z)
            );

            if (enableVoxelHighlight) { voxelHiglight.GlobalPosition = voxelPosition; }

            if (Input.IsActionJustPressed("place"))
            {
                Vector3I nextVoxel = voxelPosition + (Vector3I)normal.Normalized();

                GameManager.CommandManager.ExecuteCommand(new PlaceVoxelCommand(nextVoxel, new VoxelData(), levelMesh));
            }
            else if (Input.IsActionJustPressed("break"))
            {
                GameManager.CommandManager.ExecuteCommand(new BreakVoxelCommand(voxelPosition, new VoxelData(), levelMesh));
            }
        }
    }
}
