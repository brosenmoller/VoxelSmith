using Godot;
using System.Linq;

public partial class VoxelPlacer : RayCast3D
{
    [Export] private bool enableCollisionHighlight;
    [Export] private bool enableVoxelHighlight;

    [ExportSubgroup("References")]
    [Export] private Node3D voxelHiglight;
    [Export] private Node3D collisionHighlight;

    private bool enabled = true;

    public override void _Ready()
    {
        if (enableCollisionHighlight) { collisionHighlight.Visible = true; }
        if (enableVoxelHighlight) { voxelHiglight.Visible = true; }

        WorldController.WentInFocusLastFrame += () => enabled = true;
    }

    public override void _Process(double delta)
    {
        if (!WorldController.Instance.WorldInFocus) { enabled = false; }

        if (IsColliding() && enabled)
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
        Vector3I[] playerVoxels = new Vector3I[2];
        playerVoxels[0] = new Vector3I(
            Mathf.FloorToInt(GlobalPosition.X),
            Mathf.FloorToInt(GlobalPosition.Y),
            Mathf.FloorToInt(GlobalPosition.Z)
        );
        playerVoxels[1] = playerVoxels[0] + Vector3I.Down;

        Vector3I nextVoxel = voxelPosition + (Vector3I)normal.Normalized();

        if (!playerVoxels.Contains(nextVoxel))
        {
            GameManager.CommandManager.ExecuteCommand(new PlaceVoxelCommand(nextVoxel, GameManager.DataManager.ProjectData.SelectedVoxelData));
        }
    }

    private void PickBlock(Vector3I voxelPosition)
    {
        if (GameManager.DataManager.ProjectData.voxelColors.ContainsKey(voxelPosition))
        {
            VoxelColor voxelColor = GameManager.DataManager.ProjectData.voxelColors[voxelPosition];

            if (GameManager.DataManager.PaletteData.palleteColors.Contains(voxelColor))
            {
                GameManager.DataManager.ProjectData.selectedPaletteIndex = (int)ProjectDataPalleteIndex.COLORS;
                GameManager.DataManager.ProjectData.selectedPaletteSwatchIndex = GameManager.DataManager.PaletteData.palleteColors.IndexOf(voxelColor);

                GameManager.PaletteUI.Update();
            }
        }
        else if (GameManager.DataManager.ProjectData.voxelPrefabs.ContainsKey(voxelPosition))
        {
            VoxelPrefab voxelPrefab = GameManager.DataManager.ProjectData.voxelPrefabs[voxelPosition];

            if (GameManager.DataManager.PaletteData.palletePrefabs.Contains(voxelPrefab))
            {
                GameManager.DataManager.ProjectData.selectedPaletteIndex = (int)ProjectDataPalleteIndex.PREFABS;
                GameManager.DataManager.ProjectData.selectedPaletteSwatchIndex = GameManager.DataManager.PaletteData.palletePrefabs.IndexOf(voxelPrefab);

                GameManager.PaletteUI.Update();
            }
        }
    }
}
