using System;
using System.Collections.Generic;

[Serializable]
public class EditorData
{
    public string version;
    public Guid? lastProject;
    public Dictionary<Guid, string> palettePaths;
    public Dictionary<Guid, string> savePaths;
    public Dictionary<Guid, ExportPathData> exportPaths;
    public Dictionary<Guid, ImportPathData> importPaths;
    public List<Guid> recentProjects;

    public EditorData() 
    {
        version = "0.1";
        lastProject = null;
        palettePaths = new Dictionary<Guid, string>();
        savePaths = new Dictionary<Guid, string>();
        exportPaths = new Dictionary<Guid, ExportPathData>();
        importPaths = new Dictionary<Guid, ImportPathData>();
        recentProjects = new List<Guid>();
    }

    [Serializable]
    public class ExportPathData
    {
        public string directoryPath;
        public string fileName;
    }

    [Serializable]
    public class ImportPathData
    {
        public string path;
        public int importType;
    }
}