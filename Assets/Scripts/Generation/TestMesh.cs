using Godot;

public partial class TestMesh : MeshInstance3D
{
    private IMeshGenerator<VoxelColor> generator;

    public override void _Ready()
    {
        generator = new ComputeMeshGenerator<VoxelColor>();
        Mesh = generator.CreateMesh(new Vector3I[]
        {
            new(0, 0, 0),
            new(1, 0, 0),
            new(2, 0, 0), 
            new(2, 1, 0),
        });
    }
}
