using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using Godot;

public class VoxelDataConverter : JsonConverter<VoxelData>
{
    public override VoxelData ReadJson(JsonReader reader, Type objectType, VoxelData existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        JObject obj = JObject.Load(reader);

        VoxelData voxelData = objectType == typeof(VoxelColor) ? new VoxelColor() : new VoxelPrefab();

        voxelData.id = obj["id"]?.ToObject<Guid>() ?? Guid.NewGuid();
        voxelData.color = obj["color"]?.ToObject<Color>(serializer) ?? new Color();
        voxelData.referenceIds = obj["referenceIds"]?.ToObject<List<string>>(serializer) ?? obj["minecraftIDlist"]?.ToObject<List<string>>(serializer) ?? [];

        // Handle subclass-specific fields
        if (voxelData is VoxelPrefab prefab)
        {
            prefab.prefabName = obj["prefabName"]?.ToString();
            prefab.unityPrefabGuid = obj["unityPrefabGuid"]?.ToString();
            prefab.unityPrefabTransformFileId = obj["unityPrefabTransformFileId"]?.ToString();
            prefab.godotSceneID = obj["godotSceneID"]?.ToString();
        }

        return voxelData;
    }

    public override void WriteJson(JsonWriter writer, VoxelData value, JsonSerializer serializer)
    {
        writer.WriteStartObject();

        // Common base fields
        writer.WritePropertyName("id");
        writer.WriteValue(value.id.ToString());

        writer.WritePropertyName("color");
        JToken.FromObject(value.color, serializer).WriteTo(writer);

        writer.WritePropertyName("referenceIds");
        JToken.FromObject(value.referenceIds, serializer).WriteTo(writer);

        // Subclass-specific fields
        if (value is VoxelPrefab prefab)
        {
            writer.WritePropertyName("prefabName");
            writer.WriteValue(prefab.prefabName);

            writer.WritePropertyName("unityPrefabGuid");
            writer.WriteValue(prefab.unityPrefabGuid);

            writer.WritePropertyName("unityPrefabTransformFileId");
            writer.WriteValue(prefab.unityPrefabTransformFileId);

            writer.WritePropertyName("godotSceneID");
            writer.WriteValue(prefab.godotSceneID);
        }

        writer.WriteEndObject();
    }
}
