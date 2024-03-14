using Godot;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

public class Vector3IConverter : JsonConverter<Vector3I>
{
    public override Vector3I ReadJson(JsonReader reader, Type objectType, Vector3I existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        JObject obj = JObject.Load(reader);
        int x = obj["x"].ToObject<int>();
        int y = obj["y"].ToObject<int>();
        int z = obj["z"].ToObject<int>();

        return new Vector3I(x, y, z);
    }

    public override void WriteJson(JsonWriter writer, Vector3I value, JsonSerializer serializer)
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
