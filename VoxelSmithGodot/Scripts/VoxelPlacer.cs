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
            voxel.Transform = voxel.Transform with { Origin = GetCollisionPoint() };
        }
    }
}
