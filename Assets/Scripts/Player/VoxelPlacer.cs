using Godot;
using System.Linq;

public partial class VoxelPlacer : RayCast3D
{
    [Export] private bool enableCollisionHighlight;
    [Export] private bool enableVoxelHighlight;

    [ExportSubgroup("References")]
    [Export] private Node3D voxelHiglight;
    [Export] private Node3D collisionHighlight;

    public override void _Ready()
    {
        if (enableCollisionHighlight) { collisionHighlight.Visible = true; }
        if (enableVoxelHighlight) { voxelHiglight.Visible = true; }
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

            if (Input.IsActionJustPressed("place"))
            {
                Vector3I nextVoxel = voxelPosition + (Vector3I)normal.Normalized();

                GameManager.CommandManager.ExecuteCommand(new PlaceVoxelCommand(nextVoxel, GameManager.DataManager.ProjectData.SelectedVoxelData));
            }
            else if (Input.IsActionJustPressed("break"))
            {
                GameManager.CommandManager.ExecuteCommand(new BreakVoxelCommand(voxelPosition));
            }
            else if (Input.IsActionJustPressed("pick_block"))
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
        else
        {
            collisionHighlight.Visible = false;
            voxelHiglight.Visible = false;
        }
    }
}
