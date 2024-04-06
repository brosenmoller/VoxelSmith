using Godot;
using System;
using System.Linq;

public partial class VoxelPlacer : RayCast3D
{
    [Export] private bool enableCollisionHighlight;
    [Export] private bool enableVoxelHighlight;

    [ExportSubgroup("References")]
    [Export] private Node3D voxelHiglight;
    [Export] private Node3D collisionHighlight;

    private bool enabled = true;

    public bool checkForPlayerInside;

    public override void _Ready()
    {
        if (enableCollisionHighlight) { collisionHighlight.Visible = true; }
        if (enableVoxelHighlight) { voxelHiglight.Visible = true; }

        WorldController.WentInFocusLastFrame += () => enabled = true;
        WorldController.WentOutOfFocus += () => enabled = false;
    }

    public override void _Process(double delta)
    {
        if (IsColliding())
        {
            Vector3 point = GetCollisionPoint();
            Vector3 normal = GetCollisionNormal();

            if (enableCollisionHighlight) 
            {
                collisionHighlight.Visible = true;
                collisionHighlight.GlobalPosition = point; 
            }

            point -= normal * 0.1f;

            Vector3I voxelPosition = new(
                Mathf.FloorToInt(point.X),
                Mathf.FloorToInt(point.Y),
                Mathf.FloorToInt(point.Z)
            );

            if (enableVoxelHighlight) 
            {
                voxelHiglight.Visible = true;
                voxelHiglight.GlobalPosition = voxelPosition; 
            }

            if (!enabled) { return; }

            if (Input.IsActionJustPressed("place"))
            {
                PlaceBlock(voxelPosition, normal);
            }
            else if (Input.IsActionJustPressed("break"))
            {
                GameManager.CommandManager.ExecuteCommand(new BreakVoxelCommand(voxelPosition));
            }
            else if (Input.IsActionJustPressed("pick_block"))
            {
                PickBlock(voxelPosition);
            }
        }
        else
        {
            collisionHighlight.Visible = false;
            voxelHiglight.Visible = false;
        }
    }

    private void PlaceBlock(Vector3I voxelPosition, Vector3 normal)
    {
        Vector3I nextVoxel = voxelPosition + (Vector3I)normal.Normalized();

        if ((!IsVoxelInPlayer(nextVoxel) || !checkForPlayerInside) && GameManager.DataManager.ProjectData.SelectedVoxelData != null)
        {
            GameManager.CommandManager.ExecuteCommand(new PlaceVoxelCommand(nextVoxel));
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
