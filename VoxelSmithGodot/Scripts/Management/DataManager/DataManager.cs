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

        //projectDataHolder = new DataHolder<ProjectData>();
    }

    public void CreateNewProject(string name)
    {
        if (CurrentProjectData != null)
        {
            // TODO: Warn User about unsaved data
        }

        //CurrentProjectData = new ProjectData(name);
    }

    public void SaveCurrentProject(string path)
    {

    }

    public void LoadProject(string path)
    {

    }
}

