using Godot;

public class PlaceVoxelCommand : VoxelCommand, ICommand
{
    public PlaceVoxelCommand(Vector3I voxelPosition) : base(voxelPosition)
    {
        paletteSwatchID = projectData.selectedPaletteSwatchId;
        paletteType = projectData.selectedPaletteType;
    }

    public void Execute()
    {
        Place();
    }

    public void Undo()
    {
        Break();
    }
}

