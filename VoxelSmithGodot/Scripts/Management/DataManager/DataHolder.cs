using System.IO;
using System.Text.Json;
using System.Text;

public class DataHolder<T>
{
    public T Data { get; set; }

    private string path;

    public DataHolder(string path, T defaultData)
    {
        this.path = path;

        try
        {
            Load();
        }
        catch 
        {
            Data = defaultData;
        }
    }

    public void Load()
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

    public void Save()
    {
        JsonSerializerOptions serializerOptions = new() { IncludeFields = true };

        string jsonString = JsonSerializer.Serialize(Data, serializerOptions);

        using (FileStream fileStream = File.Open(path, FileMode.OpenOrCreate))
        {
            byte[] info = new UTF8Encoding(true).GetBytes(jsonString);
            fileStream.Write(info, 0, info.Length);
        }
    }
}

