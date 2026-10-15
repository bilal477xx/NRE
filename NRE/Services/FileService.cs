using System.IO;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace NRE.Services;

public static class FileService
{
    private static readonly JsonSerializerOptions Options = new()
    {
        WriteIndented = true,
        ReferenceHandler = ReferenceHandler.Preserve
    };

    private static readonly DataContractSerializerSettings Settings = new()
    {
        PreserveObjectReferences = true
    };

    public static void Save<T>(string path, T data)
    {
        using FileStream stream = File.Create(path);
        JsonSerializer.Serialize(stream, data, Options);
    }

    public static T? Load<T>(string path)
    {
        using FileStream stream = File.OpenRead(path);
        return JsonSerializer.Deserialize<T>(stream, Options);
    }
    
    public static string ToJson<T>(T data) => JsonSerializer.Serialize(data, Options);
    
    public static void SaveAsXml<T>(string path, T data)
    {
        using FileStream stream = File.Create(path);
        var dataContractSerializer = new DataContractSerializer(typeof(T), Settings);
        dataContractSerializer.WriteObject(stream, data);
    }

    public static T? LoadFromXml<T>(string path)
    {
        using FileStream stream = File.OpenRead(path);
        var dataContractSerializer = new DataContractSerializer(typeof(T), Settings);
        return (T?)dataContractSerializer.ReadObject(stream);
    }
}