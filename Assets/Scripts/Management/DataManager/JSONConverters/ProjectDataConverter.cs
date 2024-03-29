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
            { "palette", JToken.FromObject(value.palette, serializer) },
            { "player_position", JToken.FromObject(value.playerPosition, serializer) },
            { "camera_rotation", JToken.FromObject(value.cameraRotation, serializer) },
            { "camera_pivot_rotation", JToken.FromObject(value.cameraPivotRotation, serializer) },
            { "movement_state", (int)value.movementState },
            { "selected_palette_type", (int)value.selectedPaletteType },
            { "selected_palette_swatch_id", value.selectedPaletteSwatchId.ToString() },
        };

        JArray voxelColorArray = new();
        foreach (var kvp in value.voxelColors)
        {
            JObject voxelObj = new()
            {
                { "position", JToken.FromObject(kvp.Key, serializer) },
                { "palette_id", kvp.Value.ToString() }
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
                { "palette_id", kvp.Value.ToString() }
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
            palette = obj["palette"].ToObject<PaletteData>(serializer),

            playerPosition = obj["player_position"].ToObject<Vector3>(serializer),
            cameraRotation = obj["camera_rotation"].ToObject<Vector3>(serializer),
            cameraPivotRotation = obj["camera_pivot_rotation"].ToObject<Vector3>(serializer),

            movementState = (PlayerMovementState)obj["movement_state"].ToObject<int>(),

            selectedPaletteType = (PaletteType)obj["selected_palette_type"].ToObject<int>(),
            selectedPaletteSwatchId= Guid.Parse(obj["selected_palette_swatch_id"].ToObject<string>()),

            voxelColors = new Dictionary<Vector3I, Guid>(),
            voxelPrefabs = new Dictionary<Vector3I, Guid>()
        };

        JArray voxelColorArray = (JArray)obj["voxelColors"];
        foreach (JObject voxelObj in voxelColorArray)
        {
            Vector3I position = voxelObj["position"].ToObject<Vector3I>(serializer);
            Guid paletteID = Guid.Parse(voxelObj["palette_id"].ToObject<string>());
            projectData.voxelColors.Add(position, paletteID);
        }

        JArray voxelPrefabArray = (JArray)obj["voxelPrefabs"];
        foreach (JObject voxelObj in voxelPrefabArray)
        {
            Vector3I position = voxelObj["position"].ToObject<Vector3I>(serializer);
            Guid paletteID = Guid.Parse(voxelObj["palette_id"].ToObject<string>());
            projectData.voxelPrefabs.Add(position, paletteID);
        }

        return projectData;
    }
}

