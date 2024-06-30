using Godot;
using System.Collections.Generic;

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

    private HashSet<Vector3I> previousSelection;

    public override void Setup()
    {
        CurrentSelection = new HashSet<Vector3I>();
        previousSelection = new HashSet<Vector3I>();
    }

    public void SelectAll()
    {
        previousSelection = CurrentSelection;

        CurrentSelection = new HashSet<Vector3I>(GameManager.DataManager.ProjectData.voxelColors.Keys);
    }

    public void Deselect()
    {
        previousSelection = CurrentSelection;

        CurrentSelection.Clear();
    }

    public void InvertSelection()
    {
        previousSelection = CurrentSelection;

        HashSet<Vector3I> allPositions = new(GameManager.DataManager.ProjectData.voxelColors.Keys);
        allPositions.ExceptWith(CurrentSelection);
        CurrentSelection = allPositions;
    }

    public void Reselect()
    {
        (previousSelection, CurrentSelection) = (CurrentSelection, previousSelection);
    }
}

