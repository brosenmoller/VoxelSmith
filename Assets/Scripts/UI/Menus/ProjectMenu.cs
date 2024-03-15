using Godot;
using System;

public partial class ProjectMenu : PopupMenu
{
    [ExportGroup("Shortcuts")]
    [Export] private Shortcut newShortcut;
    [Export] private Shortcut saveShortcut;
    [Export] private Shortcut saveAsShortcut;
    [Export] private Shortcut openShortcut;
    [Export] private Shortcut importSchematicShortcut;
    [Export] private Shortcut refreshSchematicShortcut;

    public static event Action OnNewPressed;
    public static event Action OnSavePressed;
    public static event Action OnSaveAsPressed;
    public static event Action OnOpenPressed;
    public static event Action OnImportSchematicPressed;
    public static event Action OnRefreshSchematicPressed;

    public override void _Ready()
    {
        SetupMenu();
        IdPressed += OnMenuItemSelected;
    }

    private void OnMenuItemSelected(long id)
    {
        switch (id)
        {
            case (long)ProjectOptions.NEW: OnNewPressed?.Invoke(); break;
            case (long)ProjectOptions.SAVE: OnSavePressed?.Invoke(); break;
            case (long)ProjectOptions.SAVE_AS: OnSaveAsPressed?.Invoke(); break;
            case (long)ProjectOptions.OPEN: OnOpenPressed?.Invoke(); break;
            case (long)ProjectOptions.IMPORT_SCHEMATIC: OnImportSchematicPressed?.Invoke(); break;
            case (long)ProjectOptions.REFRESH_SCHEMATIC: OnRefreshSchematicPressed?.Invoke(); break;
        }
    }

    private void SetupMenu()
    {
        // 0
        AddItem("New", (int)ProjectOptions.NEW);
        SetItemShortcut(0, newShortcut, true);
        
        // 1
        AddItem("Save", (int)ProjectOptions.SAVE);
        SetItemShortcut(1, saveShortcut, true);

        // 2
        AddItem("Save As", (int)ProjectOptions.SAVE_AS);
        SetItemShortcut(2, saveAsShortcut, true);

        // 3
        AddItem("Open", (int)ProjectOptions.OPEN);
        SetItemShortcut(3, openShortcut, true);

        // 4
        SetupRecentsSubMenu();
        // 5
        SetupExportSubMenu();

        // 6
        AddItem("Import Schematic", (int)ProjectOptions.IMPORT_SCHEMATIC);
        SetItemShortcut(6, importSchematicShortcut, true);

        // 7
        AddItem("Refresh Schematic", (int)ProjectOptions.REFRESH_SCHEMATIC);
        SetItemShortcut(7, refreshSchematicShortcut, true);
    }

    private void SetupRecentsSubMenu()
    {
        PopupMenu recentsNestedMenu = new();
        recentsNestedMenu.Name = "RecentsNestedMenu";

        foreach (var recentProject in GameManager.DataManager.EditorData.savePaths)
        {
            recentsNestedMenu.AddItem(recentProject.Value, 0);
        }

        AddChild(recentsNestedMenu);
        AddSubmenuItem("Open Recent", recentsNestedMenu.Name);
    }

    private void SetupExportSubMenu()
    {
        PopupMenu exportNestedMenu = new();
        exportNestedMenu.Name = "ExportNestedMenu";
        exportNestedMenu.AddItem("Unity", 0);
        exportNestedMenu.AddItem("Godot", 1);
        AddChild(exportNestedMenu);
        AddSubmenuItem("Export", exportNestedMenu.Name);
    }

    private enum ProjectOptions
    {
        NEW,
        SAVE,
        SAVE_AS,
        OPEN,
        EXPORT,
        IMPORT_SCHEMATIC,
        REFRESH_SCHEMATIC,
    }
}
