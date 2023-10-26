using System;
using System.IO;
using Newtonsoft.Json;

public partial class FileSave : Godot.RefCounted
{
    public void SaveObj(object data, string path)
    {
        var settings = new JsonSerializerSettings();
        settings.TypeNameHandling = TypeNameHandling.Objects;
        string jsonData = JsonConvert.SerializeObject(data, settings);

        File.WriteAllText(path, jsonData);
    }

    public object RetrieveObj(string path)
    {
        if (File.Exists(path))
        {
            string savedText = File.ReadAllText(path);

            var settings = new JsonSerializerSettings();
            settings.TypeNameHandling = TypeNameHandling.Objects;
            System.Object deserializedData = JsonConvert.DeserializeObject<System.Object>(savedText, settings);

            return deserializedData;
        }

        return null;
    }
}