using Godot;
using System;
using System.IO;
using System.Text;
using System.Text.Json;

public class DataManager : Manager
{
    public ProjectData CurrentProjectData { get; private set; }
    public EditorData CurrentEditorData { get; private set; }

    private const string LOCAL_EDITOR_SAVE_PATH = "user://VoxelSmithConfig.json";
    private string GLOBAL_EDITOR_SAVE_PATH;

    public override void Setup()
    {
        GLOBAL_EDITOR_SAVE_PATH = ProjectSettings.GlobalizePath(LOCAL_EDITOR_SAVE_PATH);
        LoadEditorConfig();
    }

    public void CreateNewProject(string name)
    {
        if (CurrentProjectData != null)
        {
            // TODO: Warn User about unsaved data
        }

        CurrentProjectData = new ProjectData(name);
    }

    public void SaveCurrentProject(string path)
    {

    }

    public void LoadProject(string path)
    {

    }

    private void LoadEditorConfig()
    {
        if (!File.Exists(GLOBAL_EDITOR_SAVE_PATH))
        {
            SaveEditorConfig();
            return;
        }

        try
        {
            string jsonString = File.ReadAllText(GLOBAL_EDITOR_SAVE_PATH);
            JsonSerializerOptions serializerOptions = new() { IncludeFields = true };
            CurrentEditorData = JsonSerializer.Deserialize<EditorData>(jsonString, serializerOptions);
        }
        catch (Exception exception)
        {
            GD.Print(exception);
        }
    }

    private void SaveEditorConfig() 
    {
        CurrentEditorData ??= EditorData.Default();

        JsonSerializerOptions serializerOptions = new() { IncludeFields = true };

        string jsonString = JsonSerializer.Serialize(CurrentEditorData, serializerOptions);

        using (FileStream fileStream = File.Open(GLOBAL_EDITOR_SAVE_PATH, FileMode.OpenOrCreate))
        {
            byte[] info = new UTF8Encoding(true).GetBytes(jsonString);
            fileStream.Write(info, 0, info.Length);
        }
    }
}

