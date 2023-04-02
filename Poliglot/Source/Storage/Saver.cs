using CommunityToolkit.Maui.Storage;
using System.Text;
using System.Text.Json;

namespace Poliglot.Source.Storage;

public class Saver
{
    private readonly IFileSaver fileSaver;
    private readonly SerializationOptions options;

    public Saver(IFileSaver fileSaver, SerializationOptions serializationOptions)
    {
        this.fileSaver = fileSaver;
        this.options = serializationOptions;
    }

    public async void Save(string fileName, string contents)
    {
        var cts = new CancellationTokenSource();
        var buffer = Encoding.Default.GetBytes(contents);
        var stream = new MemoryStream(buffer);

        await fileSaver.SaveAsync(fileName, stream, cts.Token);
    }

    public void Save(string fileName, object obj)
    {
        string contents = JsonSerializer.Serialize(obj, options.JsonSerializerOptions);

        Save(fileName, contents);
    }
}