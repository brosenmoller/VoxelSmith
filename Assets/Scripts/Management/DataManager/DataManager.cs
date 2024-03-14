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

    private const string LOCAL_EDITOR_SAVE_PATH = "user://VoxelSmithConfig.json";
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
        catch { editorDataHolder.Data = new EditorData(); GD.Print("Couldn't load editor data"); }

        try
        {
            LoadProject("C:\\Users\\Ben\\AppData\\LocalLow\\Nebaj\\test.json");
        }
        catch (Exception e)
        {
            GD.Print("Failed to load project data: " + e.ToString());

            // TODO: New Project Popup

            projectDataHolder.Data = new ProjectData("Name", Guid.NewGuid());
        }

        GameManager.SurfaceMesh.UpdateMesh();
    }

    public void CreateNewProject(string name)
    {
        if (ProjectData != null)
        {
            // TODO: Warn User about unsaved data
        }

        //projectDataHolder.Data = new ProjectData(name);
    }

    public void SaveProject()
    {
        projectDataHolder.Data.playerPosition = GameManager.Player.GlobalPosition;
        projectDataHolder.Data.cameraRotation = camera.Rotation;
        projectDataHolder.Data.cameraPivotRotation = cameraPivot.Rotation;

        projectDataHolder.Save("C:\\Users\\Ben\\AppData\\LocalLow\\Nebaj\\test.json");
    }

    public void LoadProject(string path)
    {
        //projectDataHolder.Load(editorDataHolder.Data.savePaths[editorDataHolder.Data.lastProject.Value]);
        projectDataHolder.Load(path);

        GameManager.Player.GlobalPosition = projectDataHolder.Data.playerPosition;
        camera.Rotation = projectDataHolder.Data.cameraRotation;
        cameraPivot.Rotation = projectDataHolder.Data.cameraPivotRotation;
    }
}

