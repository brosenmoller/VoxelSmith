using Godot;
using System;
using System.Collections.Generic;
using System.IO;
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
    private PopupMenu exportNestedMenu;

    public override void _Ready()
    {
        SetupMenu();
        IdPressed += OnMenuItemSelected;

        DataManager.OnProjectLoad += SetupMenu;
    }

    private void OnMenuItemSelected(long id)
    {
        if (GameManager.DataManager.ProjectData == null) { return; }

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

    public void SetupMenu()
    {
        Clear();

        AddItem("New", (int)ProjectOptions.NEW);
        SetItemShortcut(0, newShortcut, true);
        
        AddItem("Save", (int)ProjectOptions.SAVE);
        SetItemShortcut(1, saveShortcut, true);

        AddItem("Save As", (int)ProjectOptions.SAVE_AS);
        SetItemShortcut(2, saveAsShortcut, true);

        AddItem("Open", (int)ProjectOptions.OPEN);
        SetItemShortcut(3, openShortcut, true);

        SetupRecentsSubMenu("Open Recent", (int)ProjectOptions.RECENTS);

        AddItem("Export", (int)ProjectOptions.EXPORT);
        SetItemShortcut(5, exportShortcut, true);

        AddItem("Load Autosave", (int)ProjectOptions.LOAD_AUTOSAVE);
        SetItemShortcut(6, loadAutoSaveShortcut, true);

        AddItem("Import Schematic", (int)ProjectOptions.IMPORT_SCHEMATIC);
        SetItemShortcut(7, importSchematicShortcut, true);
    }

    private void SetupRecentsSubMenu(string name, int id)
    {
        if (recentsNestedMenu == null)
        {
            recentsNestedMenu = new();
            recentsNestedMenu.IdPressed += OnRecentsMenuItemSelected;
            recentsNestedMenu.Name = "RecentsNestedMenu";

            AddChild(recentsNestedMenu);
        }
        else
        {
            recentsNestedMenu.Clear();
        }

        AddSubmenuNodeItem(name, recentsNestedMenu, id);

        for (int i = 1; i < GameManager.DataManager.EditorData.recentProjects.Count; i++)
        {
            if (GameManager.DataManager.EditorData.savePaths.TryGetValue(GameManager.DataManager.EditorData.recentProjects[i], out string savePath))
            {
                recentsNestedMenu.AddItem($"{Path.GetFileNameWithoutExtension(savePath)} - {savePath}", i);
            }
        }
    }

    private void OnRecentsMenuItemSelected(long id)
    {
        if (GameManager.DataManager.EditorData.savePaths.TryGetValue(GameManager.DataManager.EditorData.recentProjects[(int)id], out string savePath))
        {
            GameManager.DataManager.LoadProject(savePath);
        }
    }

    private enum ProjectOptions
    {
        NEW = 0,
        SAVE = 1,
        SAVE_AS = 2,
        OPEN = 3,
        RECENTS = 4,
        EXPORT = 5,
        LOAD_AUTOSAVE = 6,
        IMPORT_SCHEMATIC = 7,
    }
}