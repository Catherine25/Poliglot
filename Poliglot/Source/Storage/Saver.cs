using CommunityToolkit.Maui.Storage;
using System.Text;
using System.Text.Json;

namespace Poliglot.Source.Storage;

public class Saver
{
    private readonly IFileSaver fileSaver;

    public Saver(IFileSaver fileSaver)
    {
        this.fileSaver = fileSaver;
    }

    public async Task<bool> Save(string fileName, string contents)
    {
        var cts = new CancellationTokenSource();
        var buffer = Encoding.Default.GetBytes(contents);
        var stream = new MemoryStream(buffer);

        FileSaverResult result = await fileSaver.SaveAsync(fileName, stream, cts.Token);

        return result.IsSuccessful;
    }

    public async Task<bool> Save(string fileName, object obj)
    {
        string contents = JsonSerializer.Serialize(obj);

        return await Save(fileName, contents);
    }
}