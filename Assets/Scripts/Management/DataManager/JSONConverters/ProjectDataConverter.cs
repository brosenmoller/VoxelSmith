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
            { "id", value.id.ToString() },
            { "paletteID", value.paletteID.ToString() },
            { "player_position", JToken.FromObject(value.playerPosition, serializer) },
            { "camera_rotation", JToken.FromObject(value.cameraRotation, serializer) },
            { "camera_pivot_rotation", JToken.FromObject(value.cameraPivotRotation, serializer) },
            { "selected_palette_index", JToken.FromObject(value.selectedPaletteIndex, serializer) },
            { "selected_palette_swatch_index", JToken.FromObject(value.selectedPaletteSwatchIndex, serializer) },
        };

        JArray voxelColorArray = new();
        foreach (var kvp in value.voxelColors)
        {
            JObject voxelObj = new()
            {
                { "position", JToken.FromObject(kvp.Key, serializer) },
                { "voxelColor", JToken.FromObject(kvp.Value, serializer) }
            };
            voxelColorArray.Add(voxelObj);
        }
        obj.Add("voxelColors", voxelColorArray);

        JArray voxelPrefabArray = new();
        foreach (var kvp in value.voxelPrefabs)
        {
            JObject voxelObj = new()
            {
                { "position", JToken.FromObject(kvp.Key, serializer) },
                { "voxelPrefab", JToken.FromObject(kvp.Value, serializer) }
            };
            voxelPrefabArray.Add(voxelObj);
        }
        obj.Add("voxelPrefabs", voxelPrefabArray);

        obj.WriteTo(writer);
    }

    public override ProjectData ReadJson(JsonReader reader, Type objectType, ProjectData existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        JObject obj = JObject.Load(reader);
        ProjectData projectData = new()
        {
            name = obj["name"].ToObject<string>(),
            id = Guid.Parse(obj["id"].ToObject<string>()),
            paletteID = Guid.Parse(obj["paletteID"].ToObject<string>()),
            playerPosition = obj["player_position"].ToObject<Vector3>(serializer),
            cameraRotation = obj["camera_rotation"].ToObject<Vector3>(serializer),
            cameraPivotRotation = obj["camera_pivot_rotation"].ToObject<Vector3>(serializer),
            selectedPaletteIndex = obj["selected_palette_index"].ToObject<int>(),
            selectedPaletteSwatchIndex= obj["selected_palette_swatch_index"].ToObject<int>(),

            voxelColors = new Dictionary<Vector3I, VoxelColor>(),
            voxelPrefabs = new Dictionary<Vector3I, VoxelPrefab>()
        };

        JArray voxelColorArray = (JArray)obj["voxelColors"];
        foreach (JObject voxelObj in voxelColorArray)
        {
            Vector3I position = voxelObj["position"].ToObject<Vector3I>(serializer);
            VoxelColor voxelColor = voxelObj["voxelColor"].ToObject<VoxelColor>(serializer);
            projectData.voxelColors.Add(position, voxelColor);
        }

        JArray voxelPrefabArray = (JArray)obj["voxelPrefabs"];
        foreach (JObject voxelObj in voxelPrefabArray)
        {
            Vector3I position = voxelObj["position"].ToObject<Vector3I>(serializer);
            VoxelPrefab voxelPrefab = voxelObj["voxelPrefab"].ToObject<VoxelPrefab>(serializer);
            projectData.voxelPrefabs.Add(position, voxelPrefab);
        }

        return projectData;
    }
}

