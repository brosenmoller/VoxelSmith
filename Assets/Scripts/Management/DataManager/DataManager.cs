using Godot;
using System;
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
            GD.PrintErr("Couldn't load editor data");
            editorDataHolder.Data = new EditorData(); 
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

        paletteDataHolder.Data = new PaletteData();
        paletteDataHolder.Data.palleteColors.Add(new VoxelColor() { color = Color.Color8(0, 0, 0) });
        paletteDataHolder.Data.palleteColors.Add(new VoxelColor() { color = Color.Color8(255, 0, 0) });
        paletteDataHolder.Data.palleteColors.Add(new VoxelColor() { color = Color.Color8(0, 255, 0) });
        paletteDataHolder.Data.palleteColors.Add(new VoxelColor() { color = Color.Color8(0, 0, 255) });
        paletteDataHolder.Data.palleteColors.Add(new VoxelColor() { color = Color.Color8(255, 255, 0) });
        paletteDataHolder.Data.palleteColors.Add(new VoxelColor() { color = Color.Color8(255, 0, 255) });
        paletteDataHolder.Data.palleteColors.Add(new VoxelColor() { color = Color.Color8(0, 255, 255) });
        paletteDataHolder.Data.palleteColors.Add(new VoxelColor() { color = Color.Color8(255, 255, 255) });

        projectDataHolder.Data.selectedPaletteIndex = 0;
        projectDataHolder.Data.selectedPaletteSwatchIndex = 3;
        GameManager.PaletteUI.Update();
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
            GD.PrintErr("Failed to load project data: \n" + e.ToString());

            if (editorDataHolder.Data.savePaths.ContainsValue(path))
            {
                Guid projectKey = editorDataHolder.Data.savePaths.FirstOrDefault(x => x.Value == path).Key;
                if (projectKey != default)
                {
                    editorDataHolder.Data.savePaths.Remove(projectKey);
                }

                if (editorDataHolder.Data.lastProject == projectKey)
                {
                    editorDataHolder.Data.lastProject = null;
                }

                editorDataHolder.Save(GLOBAL_EDITOR_SAVE_PATH);
            }

            if (projectDataHolder.Data == null)
            {
                GameManager.UIController.startWindow.Show();
            }
        }
    }
}

