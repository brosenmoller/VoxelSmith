using Godot;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

public class Vector3Converter : JsonConverter<Vector3>
{
    public override Vector3 ReadJson(JsonReader reader, Type objectType, Vector3 existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        JObject obj = JObject.Load(reader);
        float x = obj["x"].ToObject<float>();
        float y = obj["y"].ToObject<float>();
        float z = obj["z"].ToObject<float>();

        return new Vector3(x, y, z);
    }

    public override void WriteJson(JsonWriter writer, Vector3 value, JsonSerializer serializer)
    {
        JObject obj = new()
        {
            { "x", value.X },
            { "y", value.Y },
            { "z", value.Z }
        };

        obj.WriteTo(writer);
    }
}
