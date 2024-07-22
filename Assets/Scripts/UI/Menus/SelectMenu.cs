using Godot;
using System;

public partial class SelectMenu : PopupMenu
{
    [ExportGroup("Shortcuts")]
    [Export] private Shortcut selectAllShortcut;
    [Export] private Shortcut deselectShortcut;
    [Export] private Shortcut invertSelectionShortcut;

    [Export] private Shortcut copyShortCut;
    [Export] private Shortcut cutShortCut;
    [Export] private Shortcut pasteShortCut;
    [Export] private Shortcut deleteShortCut;

    [Export] private Shortcut rotateClockWiseShortCut;
    [Export] private Shortcut rotateAntiClockWiseShortCut;
    [Export] private Shortcut flipShortCut;

    public static event Action OnSelectAllPressed;
    public static event Action OnDeselectPressed;
    public static event Action OnInvertSelectionPressed;

    public static event Action OnCopySelectionPressed;
    public static event Action OnCutSelectionPressed;
    public static event Action OnPastePressed;
    public static event Action OnDeleteSelectionPressed;

    public static event Action OnRotateClockWisePressed;
    public static event Action OnRotateAntiClockWisePressed;
    public static event Action OnFlipPressed;

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
            case (long)SelectOptions.INVERT_SELECTION: OnInvertSelectionPressed?.Invoke(); break;

            case (long)SelectOptions.COPY_SELECTION: OnCopySelectionPressed?.Invoke(); break;
            case (long)SelectOptions.CUT_SELECTION: OnCutSelectionPressed?.Invoke(); break;
            case (long)SelectOptions.PASTE: OnPastePressed?.Invoke(); break;
            case (long)SelectOptions.DELETE_SELECTION: OnDeleteSelectionPressed?.Invoke(); break;

            case (long)SelectOptions.ROTATE_CLOCKWISE: OnRotateClockWisePressed?.Invoke(); break;
            case (long)SelectOptions.ROTATE_ANTI_CLOCKWISE: OnRotateAntiClockWisePressed?.Invoke(); break;
            case (long)SelectOptions.FLIP: OnFlipPressed?.Invoke(); break;
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
        AddItem("Invert Selection", (int)SelectOptions.INVERT_SELECTION);
        SetItemShortcut(2, invertSelectionShortcut, true);

        // 3
        AddSeparator("Actions");

        // 4
        AddItem("Copy", (int)SelectOptions.COPY_SELECTION);
        SetItemShortcut(4, copyShortCut, true);

        // 5
        AddItem("Cut", (int)SelectOptions.CUT_SELECTION);
        SetItemShortcut(5, cutShortCut, true);

        // 6
        AddItem("Paste", (int)SelectOptions.PASTE);
        SetItemShortcut(6, pasteShortCut, true);

        // 7
        AddItem("Delete", (int)SelectOptions.DELETE_SELECTION);
        SetItemShortcut(7, deleteShortCut, true);

        //// 8
        //AddItem("Rotate Clockwise", (int)SelectOptions.ROTATE_CLOCKWISE);
        //SetItemShortcut(8, rotateClockWiseShortCut, true);

        //// 9
        //AddItem("Rotate Anti-Clockwise", (int)SelectOptions.ROTATE_ANTI_CLOCKWISE);
        //SetItemShortcut(9, rotateAntiClockWiseShortCut, true);

        //// 10
        //AddItem("Flip in facing direction", (int)SelectOptions.FLIP);
        //SetItemShortcut(10, flipShortCut, true);
    }

    private enum SelectOptions
    {
        SELECT_ALL,
        DESELECT,
        INVERT_SELECTION,
        COPY_SELECTION,
        CUT_SELECTION,
        PASTE,
        DELETE_SELECTION,
        ROTATE_CLOCKWISE,
        ROTATE_ANTI_CLOCKWISE,
        FLIP,
    }
}
