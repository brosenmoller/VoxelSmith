using Godot;

public class ClearSelectionListCommand : SelectionListCommand, ICommand
{
    public ClearSelectionListCommand(params Vector3I[] voxelPositions) : base(voxelPositions) { }

    public void Execute()
    {
        foreach (Vector3I voxelPosition in voxelPositions)
        {
            RemoveFromSelection(voxelPosition);
        }
    }

    public void Undo()
    {
        ReplaceFromMemory();
    }
}
