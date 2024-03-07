using Godot;

public partial class VoxelPlacer : RayCast3D
{
    [ExportSubgroup("References")]
    [Export] private PackedScene voxelScene;

    public override void _Process(double delta)
    {
        if (IsColliding() && Input.IsActionJustPressed("place"))
        {
            Voxel voxel = voxelScene.Instantiate<Voxel>();
            WorldController.Instance.AddChild(voxel);

            Vector3 location = GetCollisionPoint();
            location.X = Mathf.RoundToInt(location.X);
            location.Y = Mathf.RoundToInt(location.Y);
            location.Z = Mathf.RoundToInt(location.Z);

            voxel.Transform = voxel.Transform with { Origin = location };
        }
    }
}
