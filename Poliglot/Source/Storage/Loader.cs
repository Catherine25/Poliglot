using System.Text.Json;

namespace Poliglot.Source.Storage;

public class Loader
{
    public async Task<string> Load(string fileName)
    {
        using var stream = await FileSystem.OpenAppPackageFileAsync(fileName);
        using var reader = new StreamReader(stream);

        string result = reader.ReadToEnd();

        return result.Replace("\\r", "");
    }

    public async Task<T> Load<T>(string fileName)
    {
        var contents = await Load(fileName);
        return JsonSerializer.Deserialize<T>(contents);
    }
}