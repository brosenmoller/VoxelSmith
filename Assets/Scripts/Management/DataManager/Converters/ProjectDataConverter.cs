using Godot;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

public class ProjectDataConverter : JsonConverter<ProjectData>
{
    public override ProjectData ReadJson(JsonReader reader, Type objectType, ProjectData existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        SaveableProjectData saveData = serializer.Deserialize<SaveableProjectData>(reader);
        GD.Print("Voxel Count: " + saveData.voxelLocations.Count);
        return ProjectDataFromSave(saveData);
    }

    public override void WriteJson(JsonWriter writer, ProjectData value, JsonSerializer serializer)
    {
        SaveableProjectData saveData = SaveDataFromProjectData(value);
        GD.Print("Voxel Count: " + saveData.voxelLocations.Count);
        serializer.Serialize(writer, saveData);
    }

    public SaveableProjectData SaveDataFromProjectData(ProjectData projectData)
    {
        return new SaveableProjectData()
        {
            name = projectData.name,
            projectID = projectData.projectID,
            palleteID = projectData.palleteID,
            voxelLocations = projectData.voxels.Keys.ToList(),
            voxelData = projectData.voxels.Values.ToList(),
        };
    }

    public ProjectData ProjectDataFromSave(SaveableProjectData data)
    {
        Dictionary<Vector3I, VoxelData> voxels = new();

        for (int i = 0; i < data.voxelLocations.Count; i++)
        {
            voxels.Add(data.voxelLocations[i], data.voxelData[i]);
        }

        return new ProjectData()
        {
            name = data.name,
            projectID = data.projectID,
            palleteID = data.palleteID,
            voxels = voxels
        };
    }

    [Serializable]
    public class SaveableProjectData
    {
        public string name;
        public Guid projectID;
        public Guid palleteID;
        public List<Vector3I> voxelLocations;
        public List<VoxelData> voxelData;
    }
}

