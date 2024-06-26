﻿using Godot;
using System;
using System.IO;
using System.Linq;

public class DataManager : Manager
{
    public ProjectData ProjectData => projectDataHolder.Data;
    public EditorData EditorData => editorDataHolder.Data;
    public PaletteData PaletteData => projectDataHolder.Data.palette;

    private DataHolder<ProjectData> projectDataHolder;
    private DataHolder<EditorData> editorDataHolder;
    private DataHolder<PaletteData> paletteDataHolder;

    private const string PROJECT_FILE_EXTENSION = ".vxsProject";
    private const string PALETTE_FILE_EXTENSION = ".vxsPalette";
    private const string CONFIG_FILE_EXTENSION = ".vxsConfig";
    private const string LOCAL_EDITOR_SAVE_PATH = "user://editor" + CONFIG_FILE_EXTENSION;
    private string GLOBAL_EDITOR_SAVE_PATH;

    private Camera3D camera;
    private FirstPersonCamera cameraPivot;

    public static event Action OnProjectLoad;

    public override void Setup()
    {
        camera = GameManager.Player.GetChildByType<Camera3D>();
        cameraPivot = GameManager.Player.GetChildByType<FirstPersonCamera>();

        GLOBAL_EDITOR_SAVE_PATH = ProjectSettings.GlobalizePath(LOCAL_EDITOR_SAVE_PATH);

        projectDataHolder = new DataHolder<ProjectData>();
        editorDataHolder = new DataHolder<EditorData>();
        paletteDataHolder = new DataHolder<PaletteData>();

        LoadUpApplication();
    }
    
    private void LoadUpApplication()
    {
        try { editorDataHolder.Load(GLOBAL_EDITOR_SAVE_PATH); }
        catch 
        { 
            GD.PushWarning("Couldn't load editor data");
            editorDataHolder.Data = new EditorData(); 
            editorDataHolder.Save(GLOBAL_EDITOR_SAVE_PATH);
        }

        if (editorDataHolder.Data.lastProject.HasValue)
        {
            LoadProject(editorDataHolder.Data.savePaths[editorDataHolder.Data.lastProject.Value]);
        }
        else
        {
            GameManager.UIController.startWindow.Show();
        }
    }

    #region Editor

    public void SaveEditorData()
    {
        editorDataHolder.Save(GLOBAL_EDITOR_SAVE_PATH);
    }

    #endregion

    #region Project
    public void CreateNewProject(string fileName, string directoryPath, NewProjectWindow.PaletteOption option)
    {
        // TODO: Warn User if there is unsaved data
        if (projectDataHolder.Data != null) { SaveProject(); }

        string name = fileName;
        if (fileName.Contains('.')) { name = fileName[..fileName.IndexOf('.')]; }

        string path = Path.Combine(directoryPath, name + PROJECT_FILE_EXTENSION);

        projectDataHolder.Data = new ProjectData(name);

        if (option == NewProjectWindow.PaletteOption.Default)
        {
            projectDataHolder.Data.palette = PaletteDataFactory.GetDefaultPalette();
        }
        else
        {
            projectDataHolder.Data.palette = new PaletteData();
        }
        
        if (projectDataHolder.Data.palette.paletteColors.Count > 0)
        {
            projectDataHolder.Data.selectedPaletteType = PaletteType.Color;
            projectDataHolder.Data.selectedPaletteSwatchId = projectDataHolder.Data.palette.paletteColors.First().Key;
        }
        else if (projectDataHolder.Data.palette.palletePrefabs.Count > 0)
        {
            projectDataHolder.Data.selectedPaletteType = PaletteType.Prefab;
            projectDataHolder.Data.selectedPaletteSwatchId = projectDataHolder.Data.palette.palletePrefabs.First().Key;
        }

        OnProjectLoad?.Invoke();

        SaveProjectAs(path);

        GameManager.PaletteUI.Update();
        GameManager.SurfaceMesh.UpdateAll();
        GameManager.PrefabMesh.UpdateAll();
    }

    public void SaveProject()
    {
        if (editorDataHolder.Data.savePaths.ContainsKey(projectDataHolder.Data.id))
        {
            SaveProjectAs(editorDataHolder.Data.savePaths[projectDataHolder.Data.id]);
        }
    }

    public void SaveProjectAs(string path)
    {
        projectDataHolder.Data.playerPosition = GameManager.Player.GlobalPosition;
        projectDataHolder.Data.cameraRotation = camera.Rotation;
        projectDataHolder.Data.cameraPivotRotation = cameraPivot.Rotation;

        if (editorDataHolder.Data.savePaths.ContainsKey(projectDataHolder.Data.id))
        {
            editorDataHolder.Data.savePaths[projectDataHolder.Data.id] = path;
        }
        else
        {
            editorDataHolder.Data.savePaths.Add(projectDataHolder.Data.id, path);
        }

        editorDataHolder.Data.lastProject = projectDataHolder.Data.id;
        editorDataHolder.Save(GLOBAL_EDITOR_SAVE_PATH);

        projectDataHolder.Save(path);
    }

    public void LoadProject(string path)
    {
        // TODO: Warn User if there is unsaved data (Also in Palette)

        try
        {
            projectDataHolder.Load(path);

            GameManager.Player.GlobalPosition = projectDataHolder.Data.playerPosition;
            camera.Rotation = projectDataHolder.Data.cameraRotation;
            cameraPivot.Rotation = projectDataHolder.Data.cameraPivotRotation;

            GameManager.SurfaceMesh.UpdateAll();
            GameManager.PrefabMesh.UpdateAll();

            if (!editorDataHolder.Data.savePaths.ContainsKey(projectDataHolder.Data.id))
            {
                editorDataHolder.Data.savePaths.Add(projectDataHolder.Data.id, path);
            }
            else
            {
                editorDataHolder.Data.savePaths[projectDataHolder.Data.id] = path;
            }

            editorDataHolder.Data.lastProject = projectDataHolder.Data.id;
            editorDataHolder.Save(GLOBAL_EDITOR_SAVE_PATH);

            if (projectDataHolder.Data.palette == null) 
            {
                projectDataHolder.Data.palette = new PaletteData();
            }

            OnProjectLoad?.Invoke();

            GameManager.PaletteUI.Update();
            GameManager.UIController.startWindow.Hide();
        }
        catch (Exception e)
        {
            GD.PushWarning("Failed to load project data: \n" + e.ToString());

            if (editorDataHolder.Data.savePaths.ContainsValue(path))
            {
                Guid? projectID = editorDataHolder.Data.savePaths.FirstOrDefault(x => x.Value == path).Key;
                if (projectID != null)
                {
                    editorDataHolder.Data.savePaths.Remove(projectID.Value);

                    if (editorDataHolder.Data.lastProject == projectID.Value)
                    {
                        editorDataHolder.Data.lastProject = null;
                    }

                    editorDataHolder.Save(GLOBAL_EDITOR_SAVE_PATH);
                }
            }

            if (projectDataHolder.Data == null)
            {
                GameManager.UIController.startWindow.Show();
            }

            return;
        }
    }
    #endregion


    public void ImportPaletteFromProject(string path)
    {
        DataHolder<ProjectData> importedProject = new();

        try
        {
            importedProject.Load(path);

            foreach (var paletteItem in importedProject.Data.palette.paletteColors)
            {
                if (!PaletteData.paletteColors.ContainsKey(paletteItem.Key))
                {
                    PaletteData.paletteColors.Add(paletteItem.Key, paletteItem.Value);
                }
            }

            foreach (var paletteItem in importedProject.Data.palette.palletePrefabs)
            {
                if (!PaletteData.palletePrefabs.ContainsKey(paletteItem.Key))
                {
                    PaletteData.palletePrefabs.Add(paletteItem.Key, paletteItem.Value);
                }
            }

            GameManager.PaletteUI.Update();
        }
        catch
        {
            GD.PrintErr("Couldn't load projecﭏ");
            // TODO : SHow Error
        }
    }

    // -------------------------------------
    // Excluded from Beta
    // -------------------------------------
    #region Palette

    public void CreateNewPalette(string filePath)
    {
        // TODO: Warn User if there is unsaved data
        if (paletteDataHolder.Data != null) { SavePalette(); }

        string path = Path.Combine(filePath + PALETTE_FILE_EXTENSION);

        paletteDataHolder.Data = new PaletteData();

        SavePaletteAs(path);

        GameManager.PaletteUI.Update();
    }

    public void SavePalette()
    {
        if (editorDataHolder.Data.palettePaths.ContainsKey(paletteDataHolder.Data.id))
        {
            SavePaletteAs(editorDataHolder.Data.palettePaths[paletteDataHolder.Data.id]);
        }
    }

    public void SavePaletteAs(string path)
    {
        if (editorDataHolder.Data.palettePaths.ContainsKey(paletteDataHolder.Data.id))
        {
            editorDataHolder.Data.palettePaths[paletteDataHolder.Data.id] = path;
        }
        else
        {
            editorDataHolder.Data.palettePaths.Add(paletteDataHolder.Data.id, path);
        }

        editorDataHolder.Save(GLOBAL_EDITOR_SAVE_PATH);

        paletteDataHolder.Save(path);
    }

    public void LoadPalette(string path)
    {
        // TODO: Warn User if there is unsaved data

        try
        {
            paletteDataHolder.Load(path);

            GameManager.PaletteUI.Update();

            if (!editorDataHolder.Data.palettePaths.ContainsKey(paletteDataHolder.Data.id))
            {
                editorDataHolder.Data.palettePaths.Add(paletteDataHolder.Data.id, path);
            }
            editorDataHolder.Save(GLOBAL_EDITOR_SAVE_PATH);

            if (projectDataHolder.Data != null)
            {
                ProjectData.palette = paletteDataHolder.Data;
                SaveProject();
            }
        }
        catch (Exception e)
        {
            GD.PushWarning("Failed to load palette data: \n" + e.ToString());

            if (editorDataHolder.Data.palettePaths.ContainsValue(path))
            {
                Guid? paletteID = editorDataHolder.Data.palettePaths.FirstOrDefault(x => x.Value == path).Key;
                if (paletteID != null)
                {
                    editorDataHolder.Data.palettePaths.Remove(paletteID.Value);

                    editorDataHolder.Save(GLOBAL_EDITOR_SAVE_PATH);
                }
            }

            if (paletteDataHolder.Data == null)
            {
                // TODO : Show Error
                GameManager.UIController.ShowLoadPaletteDialog();
            }
        }
    }

    #endregion
}

