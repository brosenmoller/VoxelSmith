using Godot;
using System;

public class DataManager : Manager
{
    public ProjectData ProjectData => projectDataHolder.Data;
    public EditorData EditorData => editorDataHolder.Data;
    public PaletteData PalleteData => paletteDataHolder.Data;

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
            editorDataHolder.Data = new EditorData(); 
            GD.PrintErr("Couldn't load editor data");
            editorDataHolder.Save(GLOBAL_EDITOR_SAVE_PATH);
        }

        try
        {
            LoadProject(editorDataHolder.Data.savePaths[editorDataHolder.Data.lastProject.Value]);
        }
        catch (Exception e)
        {
            GD.PrintErr("Failed to load project data: \n" + e.ToString());
            GameManager.UIController.startWindow.Show();
        }
    }

    public void CreateNewProject(string name, string dirPath, Guid paletteGUID)
    {
        // TODO: Warn User if there is unsaved data

        string path = dirPath + name + PROJECT_FILE_EXTENSION;
        projectDataHolder.Data = new ProjectData(name, paletteGUID);
        SaveProjectAs(path);
    }

    public void SaveProject()
    {
        SaveProjectAs(editorDataHolder.Data.savePaths[projectDataHolder.Data.projectID]);
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

            editorDataHolder.Data.lastProject = projectDataHolder.Data.projectID;
            editorDataHolder.Save(GLOBAL_EDITOR_SAVE_PATH);
        }
        catch (Exception e)
        {
            GD.PrintErr("Failed to load project data: \n" + e.ToString());

            if (projectDataHolder.Data == null)
            {
                GameManager.UIController.startWindow.Show();
            }
        }
    }
}

