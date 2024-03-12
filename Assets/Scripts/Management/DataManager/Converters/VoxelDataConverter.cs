using Godot;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

public class VoxelDataConverter : JsonConverter<VoxelData>
{
    public override VoxelData ReadJson(JsonReader reader, Type objectType, VoxelData existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        JObject jsonObject = JObject.Load(reader);
        return new VoxelData()
        {
            color = (Color)GD.StrToVar(jsonObject["color"].Value<string>()),
        };
    }

    public override void WriteJson(JsonWriter writer, VoxelData value, JsonSerializer serializer)
    {
        writer.WriteStartObject();
        writer.WritePropertyName("color");
        writer.WriteValue(GD.VarToStr(value.color));
        writer.WriteEndObject();
    }
}

