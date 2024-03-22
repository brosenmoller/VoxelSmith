using Godot;

public partial class PrefabMesh : MeshInstance3D
{
    private CollisionShape3D collisionShape;
    private MeshGenerator<VoxelPrefab> meshGenerator;

    public void Setup()
    {
        collisionShape = GetParent().GetChildByType<CollisionShape3D>();
        meshGenerator = new MeshGenerator<VoxelPrefab>();
    }

    public void UpdateMesh()
    {
        Mesh = meshGenerator.CreateMesh(GameManager.DataManager.ProjectData.voxelPrefabs);
        collisionShape.Shape = Mesh.CreateTrimeshShape();
    }
}

