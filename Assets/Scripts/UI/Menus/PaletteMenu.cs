using Godot;
using System;

public partial class PaletteMenu : PopupMenu
{
    [Export] private Shortcut ImportFromProjectShortcut;

    public static event Action OnNewPressed;
    public static event Action OnOpenPressed;
    public static event Action OnSavePressed;
    public static event Action OnSaveAsPressed;
    public static event Action OnImportFromProjectPressed;

    public override void _Ready()
    {
        IdPressed += OnMenuItemSelected;
        SetupMenu();
    }

    private void OnMenuItemSelected(long id)
    {
        switch (id)
        {
            case (long)PaletteOptions.NEW: OnNewPressed?.Invoke(); break;
            case (long)PaletteOptions.OPEN: OnOpenPressed?.Invoke(); break;
            case (long)PaletteOptions.SAVE: OnSavePressed?.Invoke(); break;
            case (long)PaletteOptions.SAVE_AS: OnSaveAsPressed?.Invoke(); break;
            case (long)PaletteOptions.IMPORT_FROM_PROJECT: OnImportFromProjectPressed?.Invoke(); break;
        }
    }

    private void SetupMenu()
    {

        // Disabled these options for now for simplicity

        //// 0
        //AddItem("New", (int)PaletteOptions.NEW);

        //// 1
        //AddItem("Open", (int)PaletteOptions.OPEN);

        //// 2
        //AddItem("Save", (int)PaletteOptions.SAVE);

        //// 3
        //AddItem("Save As", (int)PaletteOptions.SAVE_AS);

        // 4
        AddItem("Import", (int)PaletteOptions.IMPORT_FROM_PROJECT);
        AddShortcut(ImportFromProjectShortcut, (int)PaletteOptions.IMPORT_FROM_PROJECT);
    }

    private enum PaletteOptions
    {
        IMPORT_FROM_PROJECT,
        NEW,
        OPEN,
        SAVE,
        SAVE_AS
    }
}
