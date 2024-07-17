using Godot;
using System.Collections.Generic;
using System.Linq;

public class SelectionManager : Manager
{
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

        GameManager.CommandManager.ExecuteCommand(new AddSelectionListCommand(selection.ToArray()));
    }

    public void DeselectAll()
    {
        GameManager.CommandManager.ExecuteCommand(new ClearSelectionListCommand(CurrentSelection.ToArray()));
    }

    public void InvertSelection()
    {
        HashSet<Vector3I> selection = new(GameManager.DataManager.ProjectData.voxelColors.Keys);
        selection.UnionWith(new HashSet<Vector3I>(GameManager.DataManager.ProjectData.voxelPrefabs.Keys));
        selection.ExceptWith(CurrentSelection);

        DeselectAll();
        GameManager.CommandManager.ExecuteCommand(new AddSelectionListCommand(selection.ToArray()));
    }

    public void Reselect()
    {
        GD.Print("Not Supported: Reselect");
    }
}
