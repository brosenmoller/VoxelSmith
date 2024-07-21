using Godot;
using System.Collections.Generic;
using System.Linq;

public class SelectionManager : Manager
{
    public ClipBoardItem currentClipBoardItem;

    private HashSet<Vector3I> _currentSelection;
    public HashSet<Vector3I> CurrentSelection 
    {  
        get { return _currentSelection; }
        private set
        {
            _currentSelection = value;
        }
    }

    public override void Setup()
    {
        CurrentSelection = new HashSet<Vector3I>
        {
            new(0, 0, 0),
            new(0, 1, 0),
            new(1, 1, 0),
            new(0, 1, 1),
            new(1, 1, 1),
            new(2, 1, 1),
            new(2, 1, 2),
            new(2, 5, 2)
        };
    }

    public void SelectAll()
    {
        HashSet<Vector3I> selection = new(GameManager.DataManager.ProjectData.voxelColors.Keys);
        selection.UnionWith(new HashSet<Vector3I>(GameManager.DataManager.ProjectData.voxelPrefabs.Keys));

        if (selection.Count <= CurrentSelection.Count) { return; }

        GameManager.CommandManager.ExecuteCommand(new AddSelectionListCommand(selection.ToArray()));
    }

    public void DeselectAll()
    {
        if (CurrentSelection.Count <= 0) { return; }

        GameManager.CommandManager.ExecuteCommand(new ClearSelectionListCommand(CurrentSelection.ToArray()));
    }

    public void InvertSelection()
    {
        if (CurrentSelection.Count <= 0) { return; }

        HashSet<Vector3I> selection = new(GameManager.DataManager.ProjectData.voxelColors.Keys);
        selection.UnionWith(new HashSet<Vector3I>(GameManager.DataManager.ProjectData.voxelPrefabs.Keys));
        selection.ExceptWith(CurrentSelection);

        DeselectAll();
        GameManager.CommandManager.ExecuteCommand(new AddSelectionListCommand(selection.ToArray()));
    }

    public void DeleteSelection()
    {
        if (CurrentSelection.Count <= 0) { return; }

        GameManager.CommandManager.ExecuteCommand(new BreakListCommand(CurrentSelection.ToArray()));
    }

    public void CopySelection()
    {
        if (CurrentSelection.Count <= 0) { return; }

        Vector3I playerPosition = VoxelHelper.GetGridVoxel(GameManager.Player.GlobalPosition);
        currentClipBoardItem = new ClipBoardItem(
            VoxelMemoryItem.CreateVoxelMemory(
                CurrentSelection.ToArray(),
                pos => pos - playerPosition
            )
        );
    }

    public void CutSelection()
    {
        if (CurrentSelection.Count <= 0) { return; }

        CopySelection();
        DeleteSelection();
    }

    public void PasteClipboardItem()
    {
        if (currentClipBoardItem == null) { return; }

        GameManager.CommandManager.ExecuteCommand(new PasteClipboardItemListCommand(currentClipBoardItem));
    }

    public void RotateSelectionClockWise()
    {
        if (CurrentSelection.Count <= 0) { return; }

    }

    public void RotateSelectionAntiClockwise()
    {
        if (CurrentSelection.Count <= 0) { return; }

    }

    public void FlipSelection()
    {
        if (CurrentSelection.Count <= 0) { return; }

    }
}
