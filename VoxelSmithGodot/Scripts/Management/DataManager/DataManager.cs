using Godot;

public class DataManager : Manager
{
    public ProjectData CurrentProjectData => projectDataHolder.Data;
    public EditorData CurrentEditorData => editorDataHolder.Data;
    public PaletteData CurrentPalleteData => paletteDataHolder.Data;

    private DataHolder<ProjectData> projectDataHolder;
    private DataHolder<EditorData> editorDataHolder;
    private DataHolder<PaletteData> paletteDataHolder;

    private const string LOCAL_EDITOR_SAVE_PATH = "user://VoxelSmithConfig.json";
    private string GLOBAL_EDITOR_SAVE_PATH;

    public override void Setup()
    {
        GLOBAL_EDITOR_SAVE_PATH = ProjectSettings.GlobalizePath(LOCAL_EDITOR_SAVE_PATH);

        projectDataHolder = new DataHolder<ProjectData>();
        editorDataHolder = new DataHolder<EditorData>();
        paletteDataHolder = new DataHolder<PaletteData>();

        LoadUpApplication();
    }
    
    private void LoadUpApplication()
    {
        try { editorDataHolder.Load(GLOBAL_EDITOR_SAVE_PATH); }
        catch { editorDataHolder.Data = new EditorData(); }

        try 
        {
            projectDataHolder.Load(editorDataHolder.Data.savePaths[editorDataHolder.Data.lastProject.Value]);


        }
        catch
        {
            // TODO: New Project Popup
        }
    }

    public void CreateNewProject(string name)
    {
        if (CurrentProjectData != null)
        {
            // TODO: Warn User about unsaved data
        }

        //projectDataHolder.Data = new ProjectData(name);
    }

    public void SaveProject(string path)
    {
        projectDataHolder.Save(path);
    }

    public void LoadProject(string path)
    {
        projectDataHolder.Load(path);
    }
}

