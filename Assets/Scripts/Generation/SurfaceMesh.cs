using Godot;

public partial class SurfaceMesh : MeshInstance3D
{ 
    private CollisionShape3D collisionShape;
    private MeshGenerator<VoxelColor> meshGenerator;

    public void Setup()
    {
        collisionShape ??= GetParent().GetChildByType<CollisionShape3D>();
        meshGenerator = new MeshGenerator<VoxelColor>();
    }

    public void UpdateMesh()
    {
        Mesh = meshGenerator.CreateMesh(GameManager.DataManager.ProjectData.voxelColors);
        collisionShape.Shape = Mesh.CreateTrimeshShape();
    }
}
