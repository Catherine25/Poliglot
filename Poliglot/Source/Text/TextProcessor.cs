namespace Poliglot.Source.Text;

public class TextProcessor
{
    public string[] ExtractSentences(string text)
    {
        return text.Split(new char[] { '\r', '\n', '.' }, StringSplitOptions.RemoveEmptyEntries);
    }

    public string[] ExtractWords(string sentence)
    {
        return sentence.Split(new char[]{' ', ','}, StringSplitOptions.RemoveEmptyEntries);
    }
}
