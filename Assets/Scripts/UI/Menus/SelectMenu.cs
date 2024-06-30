using Godot;
using System;

public partial class SelectMenu : PopupMenu
{
    [ExportGroup("Shortcuts")]
    [Export] private Shortcut selectAllShortcut;
    [Export] private Shortcut deselectShortcut;
    [Export] private Shortcut reselectShortcut;
    [Export] private Shortcut invertSelectionShortcut;

    public static event Action OnSelectAllPressed;
    public static event Action OnDeselectPressed;
    public static event Action OnReselectPressed;
    public static event Action OnInvertSelectionPressed;

    public override void _Ready()
    {
        SetupMenu();
        IdPressed += OnMenuItemSelected;
    }

    private void OnMenuItemSelected(long id)
    {
        switch (id)
        {
            case (long)SelectOptions.SELECT_ALL: OnSelectAllPressed?.Invoke(); break;
            case (long)SelectOptions.DESELECT: OnDeselectPressed?.Invoke(); break;
            case (long)SelectOptions.RESELECT: OnReselectPressed?.Invoke(); break;
            case (long)SelectOptions.INVERT_SELECTION: OnInvertSelectionPressed?.Invoke(); break;
        }
    }

    private void SetupMenu()
    {
        // 0
        AddItem("Select All", (int)SelectOptions.SELECT_ALL);
        SetItemShortcut(0, selectAllShortcut, true);

        // 1
        AddItem("Deselect", (int)SelectOptions.DESELECT);
        SetItemShortcut(1, deselectShortcut, true);

        // 2
        AddItem("Reselect", (int)SelectOptions.RESELECT);
        SetItemShortcut(2, reselectShortcut, true);

        // 3
        AddItem("Invert Selection", (int)SelectOptions.INVERT_SELECTION);
        SetItemShortcut(3, invertSelectionShortcut, true);
    }

    private enum SelectOptions
    {
        SELECT_ALL,
        DESELECT,
        RESELECT,
        INVERT_SELECTION,
    }
}
