using Godot;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

public class ProjectDataConverter : JsonConverter<ProjectData>
{
    public override void WriteJson(JsonWriter writer, ProjectData value, JsonSerializer serializer)
    {
        JObject obj = new()
        {
            { "name", value.name },
            { "projectID", value.projectID.ToString() },
            { "palleteID", value.palleteID.ToString() },
            { "player_position", JToken.FromObject(value.playerPosition, serializer) },
            { "camera_rotation", JToken.FromObject(value.cameraRotation, serializer) },
            { "camera_pivot_rotation", JToken.FromObject(value.cameraPivotRotation, serializer) }
        };

        JArray voxelsArray = new();
        foreach (var kvp in value.voxels)
        {
            JObject voxelObj = new()
            {
                { "position", JToken.FromObject(kvp.Key, serializer) },
                { "voxelData", JToken.FromObject(kvp.Value, serializer) }
            };
            voxelsArray.Add(voxelObj);
        }
        obj.Add("voxels", voxelsArray);

        obj.WriteTo(writer);
    }

    public override ProjectData ReadJson(JsonReader reader, Type objectType, ProjectData existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        JObject obj = JObject.Load(reader);
        ProjectData projectData = new()
        {
            name = obj["name"].ToObject<string>(),
            projectID = Guid.Parse(obj["projectID"].ToObject<string>()),
            palleteID = Guid.Parse(obj["palleteID"].ToObject<string>()),
            playerPosition = obj["player_position"].ToObject<Vector3>(serializer),
            cameraRotation = obj["camera_rotation"].ToObject<Vector3>(serializer),
            cameraPivotRotation = obj["camera_pivot_rotation"].ToObject<Vector3>(serializer),

            voxels = new Dictionary<Vector3I, VoxelData>()
        };

        JArray voxelsArray = (JArray)obj["voxels"];
        foreach (JObject voxelObj in voxelsArray)
        {
            Vector3I position = voxelObj["position"].ToObject<Vector3I>(serializer);
            VoxelData voxelData = voxelObj["voxelData"].ToObject<VoxelData>(serializer);
            projectData.voxels.Add(position, voxelData);
        }

        return projectData;
    }
}

