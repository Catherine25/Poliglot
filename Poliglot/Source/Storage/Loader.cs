using System.Text.Json;

namespace Poliglot.Source.Storage;

public class Loader
{
    public async Task<string> Load(string fileName)
    {
        using var stream = await FileSystem.OpenAppPackageFileAsync(fileName);
        using var reader = new StreamReader(stream);

        return reader.ReadToEnd();
    }

    public async Task<T> Load<T>(string fileName)
    {
        var contents = await Load(fileName);

        if (contents == string.Empty)
            contents = "{}";

        return JsonSerializer.Deserialize<T>(contents);
    }
}