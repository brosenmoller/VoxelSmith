using Godot;

public class PasteClipboardItemListCommand : VoxelListCommand, ICommand
{
    private readonly ClipBoardItem clipBoardItem;
    private readonly Vector3I currentPlayerPosition;

    public PasteClipboardItemListCommand(ClipBoardItem clipBoardItem) : base(GetAllPositions(clipBoardItem))
    {
        this.clipBoardItem = clipBoardItem;
        currentPlayerPosition = VoxelHelper.GetGridVoxel(GameManager.Player.GlobalPosition);
    }

    private static Vector3I[] GetAllPositions(ClipBoardItem clipBoardItem)
    {
        Vector3I[] positions = new Vector3I[clipBoardItem.voxelMemory.Length];
        Vector3I currentPlayerPosition = VoxelHelper.GetGridVoxel(GameManager.Player.GlobalPosition);

        for (int i = 0; i < clipBoardItem.voxelMemory.Length; i++)
        {
            positions[i] = clipBoardItem.voxelMemory[i].position + currentPlayerPosition;
        }

        return positions;
    }

    public void Execute()
    {
        Place();
    }

    public void Undo()
    {
        VoxelMemoryItem.ReplaceFromMemory(voxelMemory);
    }

    public void Place()
    {
        VoxelMemoryItem.ReplaceFromMemory(
            clipBoardItem.voxelMemory,
            pos => pos + currentPlayerPosition
        );
    }
}
