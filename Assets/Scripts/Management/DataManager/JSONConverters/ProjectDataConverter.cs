using Godot;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

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
            { "selected_palette_type", (int)value.selectedPaletteType },
            { "selected_palette_swatch_id", value.selectedPaletteSwatchId.ToString() },
            { "voxelColors", SerializeVoxelDictionary(value.voxelColors, serializer) },
            { "voxelPrefabs", SerializeVoxelDictionary(value.voxelPrefabs, serializer) },
            { "snapping_points", JArray.FromObject(value.snappingPoints ?? [], serializer) },
            { "export_settings", JToken.FromObject(value.exportSettings ?? new(), serializer) },
        };

        obj.WriteTo(writer);
    }

    private static JArray SerializeVoxelDictionary(Dictionary<Vector3I, Guid> dictionary, JsonSerializer serializer)
    {
        JArray array = new();
        foreach (KeyValuePair<Vector3I, Guid> voxel in dictionary)
        {
            // Avoid JToken.FromObject for performace reasons
            array.Add(new JObject
            {
                ["position"] = new JObject
                {
                    ["x"] = voxel.Key.X,
                    ["y"] = voxel.Key.Y,
                    ["z"] = voxel.Key.Z
                },
                ["palette_id"] = voxel.Value.ToString()
            });
        }
        return array;
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

            selectedPaletteType = (PaletteType)obj["selected_palette_type"].ToObject<int>(),
            selectedPaletteSwatchId= Guid.Parse(obj["selected_palette_swatch_id"].ToObject<string>()),

            voxelColors = DeserializeVoxelDictionary(obj, "voxelColors", serializer),
            voxelPrefabs = DeserializeVoxelDictionary(obj, "voxelPrefabs", serializer),
            snappingPoints = DeserializeVector3List(obj, "snapping_points", serializer),
            exportSettings = obj.TryGetValue("export_settings", out var exportToken) ? exportToken.ToObject<ExportSettingsData>(serializer) : null
        };

        return projectData;
    }

    private static Dictionary<Vector3I, Guid> DeserializeVoxelDictionary(JObject obj, string key, JsonSerializer serializer)
    {
        Dictionary<Vector3I, Guid> result = [];
        if (!obj.TryGetValue(key, out var arrayToken) || arrayToken is not JArray array) { return result; }

        foreach (JObject voxelObj in array)
        {
            Vector3I position = voxelObj["position"].ToObject<Vector3I>(serializer);
            Guid paletteID =  Guid.Parse(voxelObj["palette_id"].ToObject<string>());
            result[position] = paletteID;
        }

        return result;
    }

    private static List<Vector3> DeserializeVector3List(JObject obj, string key, JsonSerializer serializer)
    {
        List<Vector3> list = [];
        if (!obj.TryGetValue(key, out var arrayToken) || arrayToken is not JArray array) { return list; }

        foreach (var token in array) 
        { 
            list.Add(token.ToObject<Vector3>(serializer)); 
        }

        return list;
    }
}