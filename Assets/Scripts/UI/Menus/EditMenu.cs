using Godot;
using System;

public partial class EditMenu : PopupMenu
{
    [ExportGroup("Shortcuts")]
    [Export] private Shortcut undoShortcut;
    [Export] private Shortcut redoShortcut;

    public static event Action OnUndoPressed;
    public static event Action OnRedoPressed;

    public override void _Ready()
    {
        SetupMenu();
        IdPressed += OnMenuItemSelected;
    }

    private void OnMenuItemSelected(long id)
    {
        switch (id)
        {
            case (long)EditOptions.UNDO: OnUndoPressed?.Invoke(); break;
            case (long)EditOptions.REDO: OnRedoPressed?.Invoke(); break;
        }
    }

    private void SetupMenu()
    {
        // 0
        AddItem("Undo", (int)EditOptions.UNDO);
        SetItemShortcut(0, undoShortcut, true);

        // 1
        AddItem("Redo", (int)EditOptions.REDO);
        SetItemShortcut(1, redoShortcut, true);
    }

    private enum EditOptions
    {
        UNDO,
        REDO,
        FIND_AND_REPLACE,
        DESELECT_ALL,
        SELECT_ALL,
        INVERT_SELECTION,
    }
}
