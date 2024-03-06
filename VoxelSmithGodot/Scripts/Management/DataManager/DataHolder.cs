using System.IO;
using System.Text.Json;
using System.Text;

public class DataHolder<T>
{
    public T Data { get; set; }

    public void Load(string path)
    {
        try
        {
            string jsonString = File.ReadAllText(path);
            JsonSerializerOptions serializerOptions = new() { IncludeFields = true };
            Data = JsonSerializer.Deserialize<T>(jsonString, serializerOptions);
        }
        catch
        {
            throw;
        }
    }

    public void Save(string path)
    {
        JsonSerializerOptions serializerOptions = new() { IncludeFields = true,  };

        string jsonString = JsonSerializer.Serialize(Data, serializerOptions);

        using (FileStream fileStream = File.Open(path, FileMode.OpenOrCreate))
        {
            byte[] info = new UTF8Encoding(true).GetBytes(jsonString);
            fileStream.Write(info, 0, info.Length);
        }
    }
}

