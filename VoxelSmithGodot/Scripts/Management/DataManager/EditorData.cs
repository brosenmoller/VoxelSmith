using System;
using System.Collections.Generic;

[Serializable]
public class EditorData
{
    public string Version;
    public Dictionary<Guid, string> SavePaths;
    public Dictionary<Guid, string> exportPaths;
    public EditorData() 
    {
        Version = "0.1";
        SavePaths = new Dictionary<Guid, string>();
        exportPaths = new Dictionary<Guid, string>();
    }

    public static EditorData Default()
    {
        return new EditorData();
    }
}

