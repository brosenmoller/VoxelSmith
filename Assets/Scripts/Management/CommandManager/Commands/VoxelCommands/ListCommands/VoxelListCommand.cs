using Godot;

public class VoxelListCommand
{
    protected Vector3I[] voxelPositions;
    protected VoxelMemoryItem[] voxelMemory;

    public VoxelListCommand(params Vector3I[] voxelPositions)
    {
        this.voxelPositions = voxelPositions;
        voxelMemory = VoxelMemoryItem.CreateVoxelMemory(voxelPositions);
    }
}
