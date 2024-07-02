using Godot;

public class AddSelectionListCommand : SelectionListCommand, ICommand
{
    public AddSelectionListCommand(params Vector3I[] voxelPositions) : base(voxelPositions) { }

    public void Execute()
    {
        foreach (Vector3I voxelPosition in voxelPositions)
        {
            AddToSelection(voxelPosition);
        }
    }

    public void Undo()
    {
        ReplaceFromMemory();
    }
}
