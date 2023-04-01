namespace Poliglot.Source.Text;

public class TextProcessor
{
    public string[] ExtractSentences(string text)
    {
        return text.Split('\n', '.');
    }

    public string[] ExtractWords(string sentence)
    {
        return sentence.Split(' ');
    }
}
