using Godot;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

public class ProjectDataConverter : JsonConverter<ProjectData>
{
    public override void WriteJson(JsonWriter writer, ProjectData value, JsonSerializer serializer)
    {
        writer.WriteStartObject();

        writer.WritePropertyName("name"); writer.WriteValue(value.name);
        writer.WritePropertyName("id"); writer.WriteValue(value.id.ToString());
        writer.WritePropertyName("palette"); JToken.FromObject(value.palette, serializer).WriteTo(writer);
        writer.WritePropertyName("player_position"); JToken.FromObject(value.playerPosition, serializer).WriteTo(writer);
        writer.WritePropertyName("camera_rotation"); JToken.FromObject(value.cameraRotation, serializer).WriteTo(writer);
        writer.WritePropertyName("camera_pivot_rotation"); JToken.FromObject(value.cameraPivotRotation, serializer).WriteTo(writer);
        writer.WritePropertyName("selected_palette_type"); writer.WriteValue((int)value.selectedPaletteType);
        writer.WritePropertyName("selected_palette_swatch_id"); writer.WriteValue(value.selectedPaletteSwatchId.ToString());

        writer.WritePropertyName("voxelColors");
        WriteVoxelDictionary(writer, value.voxelColors);

        writer.WritePropertyName("voxelPrefabs");
        WriteVoxelDictionary(writer, value.voxelPrefabs);

        writer.WritePropertyName("export_settings"); JToken.FromObject(value.exportSettings ?? new(), serializer).WriteTo(writer);
        writer.WritePropertyName("snapping_points"); JToken.FromObject(value.snappingPoints ?? [], serializer).WriteTo(writer);

        writer.WriteEndObject();
    }

    public static void WriteVoxelDictionary(JsonWriter writer, Dictionary<Vector3I, Guid> dictionary)
    {
        writer.WriteStartArray();
        foreach (KeyValuePair<Vector3I, Guid> voxel in dictionary)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("position");
            writer.WriteStartObject();
            writer.WritePropertyName("x"); writer.WriteValue(voxel.Key.X);
            writer.WritePropertyName("y"); writer.WriteValue(voxel.Key.Y);
            writer.WritePropertyName("z"); writer.WriteValue(voxel.Key.Z);
            writer.WriteEndObject();
            writer.WritePropertyName("palette_id"); writer.WriteValue(voxel.Value.ToString());
            writer.WriteEndObject();
        }
        writer.WriteEndArray();
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

            exportSettings = obj.TryGetValue("export_settings", out var exportToken) ? exportToken.ToObject<ExportSettingsData>(serializer) : null,
            snappingPoints = DeserializeVector3List(obj, "snapping_points", serializer),
        };

        return projectData;
    }

    private static Dictionary<Vector3I, Guid> DeserializeVoxelDictionary(JObject obj, string key, JsonSerializer serializer)
    {
        Dictionary<Vector3I, Guid> result = [];
        if (!obj.TryGetValue(key, out JToken arrayToken) || arrayToken is not JArray array) { return result; }

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
        if (!obj.TryGetValue(key, out JToken arrayToken) || arrayToken is not JArray array) { return list; }

        foreach (JToken token in array) 
        { 
            list.Add(token.ToObject<Vector3>(serializer)); 
        }

        return list;
    }
}