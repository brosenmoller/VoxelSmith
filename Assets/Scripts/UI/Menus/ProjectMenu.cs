using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class ProjectMenu : PopupMenu
{
    [ExportGroup("Shortcuts")]
    [Export] private Shortcut newShortcut;
    [Export] private Shortcut saveShortcut;
    [Export] private Shortcut saveAsShortcut;
    [Export] private Shortcut openShortcut;
    [Export] private Shortcut exportShortcut;
    [Export] private Shortcut loadAutoSaveShortcut;
    [Export] private Shortcut importSchematicShortcut;
    [Export] private Shortcut refreshSchematicShortcut;

    public static event Action OnNewPressed;
    public static event Action OnSavePressed;
    public static event Action OnSaveAsPressed;
    public static event Action OnOpenPressed;
    public static event Action OnExportPressed;
    public static event Action OnLoadAutoSavePressed;
    public static event Action OnImportSchematicPressed;

    private PopupMenu recentsNestedMenu;
    private List<string> recentPaths;

    private PopupMenu exportNestedMenu;

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
            case (long)ProjectOptions.EXPORT: OnExportPressed?.Invoke(); break;
            case (long)ProjectOptions.LOAD_AUTOSAVE: OnLoadAutoSavePressed?.Invoke(); break;
            case (long)ProjectOptions.IMPORT_SCHEMATIC: OnImportSchematicPressed?.Invoke(); break;
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
        AddItem("Export", (int)ProjectOptions.EXPORT);
        SetItemShortcut(5, exportShortcut, true);

        // 6
        AddItem("Load Autosave", (int)ProjectOptions.LOAD_AUTOSAVE);
        SetItemShortcut(6, loadAutoSaveShortcut, true);

        // 7
        AddItem("Import Schematic", (int)ProjectOptions.IMPORT_SCHEMATIC);
        SetItemShortcut(7, importSchematicShortcut, true);
    }

    private void SetupRecentsSubMenu()
    {
        recentsNestedMenu?.QueueFree();

        recentsNestedMenu = new();
        recentsNestedMenu.IdPressed += OnRecentsMenuItemSelected;
        recentsNestedMenu.Name = "RecentsNestedMenu";

        recentPaths = GameManager.DataManager.EditorData.savePaths.Values.ToList();

        for (int i = 0; i < recentPaths.Count; i++)
        {
            recentsNestedMenu.AddItem(recentPaths[i], i);
        }

        AddChild(recentsNestedMenu);
        AddSubmenuNodeItem("Open Recent", recentsNestedMenu);
    }

    private void OnRecentsMenuItemSelected(long id)
    {
        GameManager.DataManager.LoadProject(recentPaths[(int)id]);
    }

    private enum ProjectOptions
    {
        NEW,
        SAVE,
        SAVE_AS,
        OPEN,
        EXPORT,
        LOAD_AUTOSAVE,
        IMPORT_SCHEMATIC,
    }
}