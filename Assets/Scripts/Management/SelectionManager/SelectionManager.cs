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

    private PaletteData oldPaletteData = null;

    public SelectionManager()
    {
        DataManager.BeforeProjectLoad += BeforeProjectLoad;
        DataManager.OnProjectLoad += OnProjectLoad;
    }

    private void BeforeProjectLoad()
    {
        if (GameManager.DataManager.ProjectData != null) 
        { 
            oldPaletteData = GameManager.DataManager.PaletteData;
        }
    }

    private void OnProjectLoad()
    {
        CurrentSelection = new HashSet<Vector3I>();
        if (currentClipBoardItem != null && oldPaletteData != null) 
        {
            currentClipBoardItem.ChangeGUIDsToNewProject(oldPaletteData);
        }
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

    public void RotateClipboardClockWise()
    {
        if (currentClipBoardItem == null) { return; }

        currentClipBoardItem.RotateClockWise();
    }

    public void RotateClipboardAntiClockwise()
    {
        if (currentClipBoardItem == null) { return; }

        currentClipBoardItem.RotateAntiClockWise();
    }

    public void FlipClipboard()
    {
        if (currentClipBoardItem == null) { return; }

        currentClipBoardItem.Flip();
    }
}
