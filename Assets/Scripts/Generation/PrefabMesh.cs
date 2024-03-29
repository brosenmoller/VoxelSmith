﻿using Godot;

public partial class PrefabMesh : MeshInstance3D
{
    [Export] private Material material;

    private CollisionShape3D collisionShape;
    private MeshGenerator<VoxelPrefab> meshGenerator;

    public void Setup()
    {
        collisionShape = GetParent().GetChildByType<CollisionShape3D>();
        meshGenerator = new MeshGenerator<VoxelPrefab>(material);
    }

    public void UpdateMesh()
    {
        Mesh = meshGenerator.CreateMesh(GameManager.DataManager.ProjectData.voxelPrefabs);
        collisionShape.Shape = Mesh.CreateTrimeshShape();
    }
}

