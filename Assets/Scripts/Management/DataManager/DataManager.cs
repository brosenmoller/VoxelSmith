using Godot;
using System;
using System.IO;
using System.Linq;

public class DataManager : Manager
{
    public ProjectData ProjectData => projectDataHolder.Data;
    public EditorData EditorData => editorDataHolder.Data;
    public PaletteData PaletteData => paletteDataHolder.Data;

    private DataHolder<ProjectData> projectDataHolder;
    private DataHolder<EditorData> editorDataHolder;
    private DataHolder<PaletteData> paletteDataHolder;

    private const string PROJECT_FILE_EXTENSION = ".vxsProject";
    private const string PALETTE_FILE_EXTENSION = ".vxsPalette";
    private const string LOCAL_EDITOR_SAVE_PATH = "user://editor.vxsConfig";
    private string GLOBAL_EDITOR_SAVE_PATH;

    private Camera3D camera;
    private FirstPersonCamera cameraPivot;

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

        try
        {
            GD.Print(editorDataHolder.Data.savePaths[editorDataHolder.Data.lastProject.Value]);
            LoadProject(editorDataHolder.Data.savePaths[editorDataHolder.Data.lastProject.Value]);
        }
        catch (Exception e)
        {
            GD.PushWarning("Failed to load project data: \n" + e.ToString());
            GameManager.UIController.startWindow.Show();
        }

        paletteDataHolder.Data = new PaletteData();
        paletteDataHolder.Data.palleteColors.Add(new VoxelColor() { color = Color.Color8(0, 0, 0) });
        paletteDataHolder.Data.palleteColors.Add(new VoxelColor() { color = Color.Color8(255, 0, 0) });
        paletteDataHolder.Data.palleteColors.Add(new VoxelColor() { color = Color.Color8(0, 255, 0) });
        paletteDataHolder.Data.palleteColors.Add(new VoxelColor() { color = Color.Color8(0, 0, 255) });
        paletteDataHolder.Data.palleteColors.Add(new VoxelColor() { color = Color.Color8(255, 255, 0) });
        paletteDataHolder.Data.palleteColors.Add(new VoxelColor() { color = Color.Color8(255, 0, 255) });
        paletteDataHolder.Data.palleteColors.Add(new VoxelColor() { color = Color.Color8(0, 255, 255) });
        paletteDataHolder.Data.palleteColors.Add(new VoxelColor() { color = Color.Color8(255, 255, 255) });

        paletteDataHolder.Data.palletePrefabs.Add(new VoxelPrefab() { color = Color.Color8(100, 200, 0), unityPrefabGuid = "67ce479430c155e4cbcd3bb0ef4f4954", prefabName = "TestSpehere", unityPrefabTransformFileId = "726921523353226827" });

        projectDataHolder.Data.selectedPaletteIndex = 0;
        projectDataHolder.Data.selectedPaletteSwatchIndex = 3;
        GameManager.PaletteUI.Update();
    }

    #region Editor

    public void SaveEditorData()
    {
        editorDataHolder.Save(GLOBAL_EDITOR_SAVE_PATH);
    }

    #endregion

    #region Project
    public void CreateNewProject(string fileName, string directoryPath, Guid paletteGUID)
    {
        // TODO: Warn User if there is unsaved data
        if (projectDataHolder.Data != null) { SaveProject(); }

        string name = fileName;
        if (fileName.Contains('.')) { name = fileName[..fileName.IndexOf(".")]; }

        string path = Path.Combine(directoryPath, name + PROJECT_FILE_EXTENSION);
        projectDataHolder.Data = new ProjectData(name, paletteGUID);

        SaveProjectAs(path);

        GameManager.SurfaceMesh.UpdateMesh();
        GameManager.PrefabMesh.UpdateMesh();
    }

    public void SaveProject()
    {
        if (editorDataHolder.Data.savePaths.ContainsKey(projectDataHolder.Data.projectID))
        {
            SaveProjectAs(editorDataHolder.Data.savePaths[projectDataHolder.Data.projectID]);
        }
    }

    public void SaveProjectAs(string path)
    {
        projectDataHolder.Data.playerPosition = GameManager.Player.GlobalPosition;
        projectDataHolder.Data.cameraRotation = camera.Rotation;
        projectDataHolder.Data.cameraPivotRotation = cameraPivot.Rotation;

        if (editorDataHolder.Data.savePaths.ContainsKey(projectDataHolder.Data.projectID))
        {
            editorDataHolder.Data.savePaths[projectDataHolder.Data.projectID] = path;
        }
        else
        {
            editorDataHolder.Data.savePaths.Add(projectDataHolder.Data.projectID, path);
        }

        editorDataHolder.Data.lastProject = projectDataHolder.Data.projectID;
        editorDataHolder.Save(GLOBAL_EDITOR_SAVE_PATH);

        projectDataHolder.Save(path);
    }

    public void LoadProject(string path)
    {
        // TODO: Warn User if there is unsaved data

        try
        {
            projectDataHolder.Load(path);

            GameManager.Player.GlobalPosition = projectDataHolder.Data.playerPosition;
            camera.Rotation = projectDataHolder.Data.cameraRotation;
            cameraPivot.Rotation = projectDataHolder.Data.cameraPivotRotation;

            GameManager.SurfaceMesh.UpdateMesh();
            GameManager.PrefabMesh.UpdateMesh();

            if (!editorDataHolder.Data.savePaths.ContainsKey(projectDataHolder.Data.projectID))
            {
                editorDataHolder.Data.savePaths.Add(projectDataHolder.Data.projectID, path);
            }
            editorDataHolder.Data.lastProject = projectDataHolder.Data.projectID;
            editorDataHolder.Save(GLOBAL_EDITOR_SAVE_PATH);

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

        if (EditorData.palettePaths.ContainsKey(ProjectData.projectID))
        {
            LoadPalette(EditorData.palettePaths[ProjectData.palleteID]);
        }
    }
    #endregion

    #region Palette

    public void CreateNewPalette(string path)
    {
        // TODO: Warn User if there is unsaved data
        if (paletteDataHolder.Data != null) { SavePalette(); }

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
        }
    }

    #endregion
}

