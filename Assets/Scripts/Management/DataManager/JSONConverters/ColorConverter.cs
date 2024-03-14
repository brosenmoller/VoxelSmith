using Godot;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

public class ColorConverter : JsonConverter<Color>
{
    public override Color ReadJson(JsonReader reader, Type objectType, Color existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        JObject obj = JObject.Load(reader);
        byte r = obj["r"].ToObject<byte>();
        byte g = obj["g"].ToObject<byte>();
        byte b = obj["b"].ToObject<byte>();
        byte a = obj["a"].ToObject<byte>();

        return new Color(r / 255f, g / 255f, b / 255f, a / 255f);
    }

    public override void WriteJson(JsonWriter writer, Color value, JsonSerializer serializer)
    {
        JObject obj = new()
        {
            { "r", (byte)(value.R * 255f) },
            { "g", (byte)(value.G * 255f) },
            { "b", (byte)(value.B * 255f) },
            { "a", (byte)(value.A * 255f) }
        };

        obj.WriteTo(writer);
    }
}