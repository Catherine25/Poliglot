using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

public class SerializationOptions
{
    public readonly JsonSerializerOptions JsonSerializerOptions;

    public SerializationOptions()
    {
        JsonSerializerOptions = new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
            WriteIndented = true
        };
    }
}