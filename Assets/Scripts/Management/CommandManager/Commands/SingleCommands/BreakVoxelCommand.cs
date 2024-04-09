using Godot;

public class BreakVoxelCommand : VoxelCommand, ICommand
{
    public BreakVoxelCommand(Vector3I voxelPosition) : base(voxelPosition)
    {
        if (projectData.voxelColors.ContainsKey(voxelPosition))
        {
            paletteType = PaletteType.Color;
            paletteSwatchID = projectData.voxelColors[voxelPosition];
        }
        else if (projectData.voxelPrefabs.ContainsKey(voxelPosition))
        {
            paletteType = PaletteType.Prefab;
            paletteSwatchID = projectData.voxelPrefabs[voxelPosition];
        }
        else
        {
            GD.PrintErr("Broke non exsistent block, should never happen");
        }
    }

    public void Execute()
    {
        Break();
    }

    public void Undo()
    {
        Place();
    }
}

