using Godot;
using Newtonsoft.Json;
using System;

public class Vector3IConverter : JsonConverter<Vector3I>
{
    public override Vector3I ReadJson(JsonReader reader, Type objectType, Vector3I existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        GD.Print("Read JSON");
        return (Vector3I)GD.StrToVar(reader.ReadAsString());
    }

    public override void WriteJson(JsonWriter writer, Vector3I value, JsonSerializer serializer)
    {
        GD.Print("Write JSON");
        writer.WriteValue(GD.VarToStr(value));

        //writer.WriteStartObject();
        //writer.WritePropertyName("x");
        //writer.WriteValue(value.X);
        //writer.WritePropertyName("y");
        //writer.WriteValue(value.Y);
        //writer.WritePropertyName("z");
        //writer.WriteValue(value.Z);
        //writer.WriteEndObject();
    }
}

