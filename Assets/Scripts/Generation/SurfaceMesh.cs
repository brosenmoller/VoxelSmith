using Godot;
using System.Collections.Generic;

public partial class SurfaceMesh : WorldMesh<VoxelColor>
{
    public override void SetupMeshGenerator()
    {
        meshGenerator = new ChunkedMeshGenerator<VoxelColor>(8);
    }

    public override void UpdateMesh()
    {
        Mesh = meshGenerator.CreateColorMesh(
            new HashSet<Vector3I>(GameManager.DataManager.ProjectData.voxelColors.Keys), 
            GameManager.DataManager.PaletteData.paletteColors
        );

        collisionShape.Shape = Mesh.CreateTrimeshShape();
    }
}
