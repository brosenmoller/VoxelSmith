using System;
using System.Collections.Generic;

[Serializable]
public class EditorData
{
    public string version;
    public Guid? lastProject;
    public Dictionary<Guid, string> palettePaths;
    public Dictionary<Guid, string> savePaths;
    public Dictionary<Guid, ExportSettings> exportPaths;
    public Dictionary<Guid, ImportSettings> importPaths;

    public EditorData() 
    {
        version = "0.1";
        lastProject = null;
        palettePaths = new Dictionary<Guid, string>();
        savePaths = new Dictionary<Guid, string>();
        exportPaths = new Dictionary<Guid, ExportSettings>();
        importPaths = new Dictionary<Guid, ImportSettings>();
    }

    [Serializable]
    public class ExportSettings
    {
        public string directoryPath;
        public string fileName;
        public int exportType;
    }

    [Serializable]
    public class ImportSettings
    {
        public string path;
        public int importType;
    }
}

