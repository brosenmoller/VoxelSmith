using System.IO;
using System.Text;
using Newtonsoft.Json;

public class DataHolder<T>
{
    public T Data { get; set; }

    public void Load(string path)
    {
        try
        {
            string jsonString = File.ReadAllText(path);
            Data = JsonConvert.DeserializeObject<T>(jsonString, new Vector3IConverter(), new ProjectDataConverter(), new VoxelDataConverter());
        }
        catch
        {
            throw;
        }
    }

    public void Save(string path)
    {
        string jsonString = JsonConvert.SerializeObject(Data, new Vector3IConverter(), new ProjectDataConverter(), new VoxelDataConverter());

        File.WriteAllText(path, string.Empty);

        using (FileStream fileStream = File.Open(path, FileMode.OpenOrCreate))
        {
            byte[] info = new UTF8Encoding(true).GetBytes(jsonString);
            fileStream.Write(info, 0, info.Length);
        }
    }
}

